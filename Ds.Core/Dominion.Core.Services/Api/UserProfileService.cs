using Dominion.Authentication.Interface.Api;
using Dominion.Core.Services.Interfaces;
using Dominion.Core.Services.Api.Auth;
using Dominion.Core.Services.Api.Notification;
using Dominion.Core.Services.Api.DataServicesInjectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.OpResult;
using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Security;
using Dominion.Domain.Entities.User;
using Dominion.Utility.Containers;
using Dominion.Core.Services.Internal.Providers;
using Dominion.Core.Services.Mapping;
using Dominion.Utility.ExtensionMethods;
using Dominion.Authentication.Dto;

namespace Dominion.Core.Services.Api
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IBusinessApiSession _session;
        private readonly IDsDataServicesUserService _dsUser;
        private readonly ILoginService _loginService;
        private readonly ISecurityService _securityService;
        private readonly IDominionOnlyNotificationService _notificationService;
        private readonly AuthHelper _authHelper;
        private readonly IUserService _userService;
        private readonly IUserProvider _userProvider;

        public UserProfileService(
            IBusinessApiSession session, 
            IDsDataServicesUserService dsUser,
            ILoginService loginService,
            ISecurityService securityService,
            IDominionOnlyNotificationService notificationService,
            IUserService userService,
            IUserProvider userProvider)
        {
            _session = session;
            _dsUser = dsUser;
            _loginService = loginService;
            _securityService = securityService;
            _notificationService = notificationService;
            _authHelper = AuthHelper.Create(_loginService, _securityService, default);
            _userService = userService;
            _userProvider = userProvider;
        }

        /// <summary>
        /// This method takes a DTO that represents an existing user record from the user profile page. It checks to make sure the requesting
        /// user has authorization and then updates the record appropriately. It accounts for changing to/from supervisors and company admins, 
        /// adding/removing necessary security and company access records respectively. However, it does NOT handle adding a userpin for an
        /// upgrade from a usertype to a company admin. It will remove the existing user pins if necessary when changing from company admin to 
        /// something else. The user pin logic exists in a separate API call and can be referenced from the angular component UserProfileFormComponent.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IOpResult IUserProfileService.UpdateUser(InsertUpdateUserDto dto)
        {
            var result = new OpResult();
            var updateToSupervisor = false;
            var updateFromSupervisor = false;
            var updateToCompanyAdmin = false;
            var updateFromCompanyAdmin = false;

            _session.CanPerformAction(UserManagerActionType.UserReadWrite).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(dto.ClientId).MergeInto(result);

            if (result.HasError) return result;

            var curr = _session.UnitOfWork.UserRepository
                .QueryUsers()
                .ByUserId(dto.DsUserId)
                .ExecuteQueryAs(UserMaps.FromUser.ToUserDto.Instance)
                .FirstOrDefault();

            if (curr == null) return result.SetToFail("User not found. Please reload the page and try again.");

            #region Settings update strategy flags

            if (dto.DsUserType == (int)UserType.Supervisor || curr.UserTypeId == UserType.Supervisor)
            {
                updateToSupervisor = curr.UserTypeId != UserType.Supervisor && dto.DsUserType == (int)UserType.Supervisor;
                updateFromSupervisor = curr.UserTypeId == UserType.Supervisor && dto.DsUserType != (int)UserType.Supervisor;
            } 
            else if (dto.DsUserId == (int)UserType.CompanyAdmin || curr.UserTypeId == UserType.CompanyAdmin)
            {
                updateToCompanyAdmin = curr.UserTypeId != UserType.CompanyAdmin && dto.DsUserType == (int)UserType.CompanyAdmin;
                updateFromCompanyAdmin = curr.UserTypeId == UserType.CompanyAdmin && dto.DsUserType != (int)UserType.CompanyAdmin;
            }

            #endregion 

            #region Build disconnected User entity & registered changed properties to EF

            var e = new User
            {
                UserId = dto.DsUserId,
                UserTypeId = (UserType)dto.DsUserType,
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PasswordHash = dto.EncryptedPassword,
                EmailAddress = dto.Email,
                EmployeeId = dto.EmployeeID,
                ViewEmployeePayTypes = (UserViewEmployeePayType)dto.ViewEmployees,
                ViewEmployeeRateTypes = (UserViewEmployeePayType)dto.ViewRates,
                IsSecurityEnabled = dto.SecuritySettings,
                IsEmployeeSelfServiceViewOnly = dto.ViewOnly,
                IsEmployeeSelfServiceOnly = dto.EmployeeSelfServiceOnly,
                IsReportingOnly = dto.ReportingOnly,
                IsPayrollAccessBlocked = dto.BlockPayrollAccess,
                IsTimeclockEnabled = dto.Timeclock,
                IsHrBlocked = dto.BlockHR,
                IsEmployeeAccessOnly = dto.EmployeeOnly,
                IsApplicantTrackingAdmin = dto.ApplicantAdmin,
                TempEnableFromDate = dto.FromDate,
                TempEnableToDate = dto.ToDate,
                IsEditGlEnabled = dto.EditGL,
                TimeoutMinutes = dto.Timeout,
                CanViewTaxPackets = dto.ViewTaxPackets,
                MustChangePassword = dto.MustChangePWD,
                TimeclockAppOnly = dto.IsTimeClockDeviceUser
            };

            var props = new PropertyList<User>();
            var peProps = new PropertyList<UserPermissions>();

            if (curr.UserTypeId != e.UserTypeId)
                props.Include(x => x.UserTypeId);
            if (curr.UserName != e.UserName)
                props.Include(x => x.UserName);
            if (curr.FirstName != e.FirstName)
                props.Include(x => x.FirstName);
            if (curr.LastName != e.LastName)
                props.Include(x => x.LastName);
            if (!string.IsNullOrWhiteSpace(e.PasswordHash) && curr.Password != e.PasswordHash)
                props.Include(x => x.PasswordHash);
            if (curr.EmailAddress != e.EmailAddress)
                props.Include(x => x.EmailAddress);
            if (curr.EmployeeId != e.EmployeeId)
                props.Include(x => x.EmployeeId);
            if (curr.ViewEmployeePayTypes != e.ViewEmployeePayTypes)
                props.Include(x => x.ViewEmployeePayTypes);
            if (curr.ViewEmployeeRateTypes != e.ViewEmployeeRateTypes)
                props.Include(x => x.ViewEmployeeRateTypes);
            if (curr.IsSecurityEnabled != e.IsSecurityEnabled)
                props.Include(x => x.IsSecurityEnabled);
            if (curr.IsEmployeeSelfServiceViewOnly != e.IsEmployeeSelfServiceViewOnly)
                props.Include(x => x.IsEmployeeSelfServiceViewOnly);
            if (curr.IsEmployeeSelfServiceOnly != e.IsEmployeeSelfServiceOnly)
                props.Include(x => x.IsEmployeeSelfServiceOnly);
            if (curr.IsReportingOnly != e.IsReportingOnly)
                props.Include(x => x.IsReportingOnly);
            if (curr.IsPayrollAccessBlocked != e.IsPayrollAccessBlocked)
                props.Include(x => x.IsPayrollAccessBlocked);
            if (curr.IsTimeclockEnabled != e.IsTimeclockEnabled)
                props.Include(x => x.IsTimeclockEnabled);
            if (curr.IsHrBlocked != e.IsHrBlocked)
                props.Include(x => x.IsHrBlocked);
            if (curr.IsEmployeeAccessOnly != e.IsEmployeeAccessOnly)
                props.Include(x => x.IsEmployeeAccessOnly);
            if (curr.IsApplicantTrackingAdmin != e.IsApplicantTrackingAdmin)
                props.Include(x => x.IsApplicantTrackingAdmin);
            if (curr.TempEnableFromDate != e.TempEnableFromDate)
                props.Include(x => x.TempEnableFromDate);
            if (curr.TempEnableToDate != e.TempEnableToDate)
                props.Include(x => x.TempEnableToDate);
            if (curr.IsEditGlEnabled != e.IsEditGlEnabled)
                props.Include(x => x.IsEditGlEnabled);
            if (curr.TimeoutMinutes != e.TimeoutMinutes)
                props.Include(x => x.TimeoutMinutes);
            if (curr.CanViewTaxPackets != e.CanViewTaxPackets)
                props.Include(x => x.CanViewTaxPackets);
            if (curr.MustChangePassword != e.MustChangePassword)
                props.Include(x => x.MustChangePassword);
            if (curr.TimeClockAppOnly != e.TimeclockAppOnly)
                props.Include(x => x.TimeclockAppOnly);

            if (props.Any())
            {
                e.LastModifiedByUserId = _session.LoggedInUserInformation.UserId;
                _session.UnitOfWork.RegisterModified(e, props);
            }

            #endregion

            #region Check for changes to User Permissions table

            if (curr.Permissions != null && dto.Permissions != null)
            {
                var pe = new UserPermissions
                {
                    UserId = dto.DsUserId,
                    IsEmployeeNavigatorAdmin = dto.Permissions.IsEmployeeNavigatorAdmin
                };

                if (pe.IsEmployeeNavigatorAdmin != curr.Permissions.IsEmployeeNavigatorAdmin)
                    peProps.Include(x => x.IsEmployeeNavigatorAdmin);

                if (peProps.Any()) _session.UnitOfWork.RegisterModified(pe, peProps);
            }

            #endregion

            #region Update to Supervisor 

            if (updateToSupervisor)
            {
                result.TryCatch(() => _dsUser.InsertUserSupervisorSecurity(dto.DsUserId, dto.ClientId, 1, dto.EmployeeID));
            }

            #endregion
            #region Update from Supervisor

            else if (updateFromSupervisor)
            {
                result.TryCatch(() =>
                {
                    _dsUser.DeleteUserSupervisorSecurity(dto.DsUserId, dto.ClientId);
                    _dsUser.DeleteUserSupervisorSecuritySettings(dto.DsUserId);
                });
            }

            #endregion
            #region Update to Company Admin 

            else if (updateToCompanyAdmin)
            {
                // add company access record for the clients that the user will have access to
                var companyAccess = new List<UserClientAccessDto>()
                {
                    new UserClientAccessDto()
                    {
                        ClientId = dto.ClientId,
                        UserId = dto.DsUserId,
                        ClientName = null,
                        HasAccess = true,
                        IsBenefitAdmin = false,
                        IsClientAdmin = true
                    }
                };

                var updatedUserClients = _userProvider.SaveCompanyAdminAccessForUser(dto.DsUserId, companyAccess)
                    .MergeInto(result).Data.ToOrNewList();

                #region Update AuthClientId (-1 for multi-client or 1+ for single-client)

                if (result.HasNoError)
                {
                    var isClientAdmin = e.UserTypeId == UserType.CompanyAdmin;
                    var authClientID = 0;
                    if (isClientAdmin)
                    {
                        var isMultiClient = updatedUserClients.Count() > 1;
                        var isClientIdFound = updatedUserClients.Any();

                        if (isMultiClient)
                        {
                            authClientID = -1;
                        }
                        else if (isClientIdFound)
                        {
                            authClientID = updatedUserClients.First().ClientId;
                        }
                    }

                    if (authClientID == 0) //clientId not found on UserClient table
                    {
                        authClientID = dto.ClientId > 0 ? curr.LastClientId > 0 ? curr.LastClientId : -1 : -1;
                    }

                    var userSecurityDto = new UserSecurityDto()
                    {
                        AuthUserId = e?.AuthUserId ?? -1,
                        UserTypeId = (byte)e.UserTypeId,
                        DsUserId1 = e.UserId,
                        AuthClientId = authClientID,
                        ModifiedBy = _session.LoggedInUserInformation.AuthUserId
                    };

                    if (userSecurityDto.AuthUserId > 0)
                        _loginService.SetUserSecurityInformation(userSecurityDto).MergeInto(result);
                }

                #endregion
            }

            #endregion
            #region Update from Company Admin 

            else if (updateFromCompanyAdmin)
            {
                _userProvider.DeleteCompanyAdminAccess(dto.DsUserId).MergeInto(result);
                _userProvider.DeleteCompanyAdminUserPin(dto.DsUserId).MergeInto(result);
            }

            #endregion

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult<int> IUserProfileService.SaveNewUser(InsertUpdateUserDto dto)
        {
            var result = new OpResult<int>();

            _session.CanPerformAction(UserManagerActionType.UserReadWrite).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(dto.ClientId).MergeInto(result);

            if (result.HasError) return result;

            var newId = _dsUser.InsertUser(dto, _loginService, _securityService, _notificationService, _authHelper)
                .MergeInto(result)
                .Data ?? 0;

            if (result.HasError) return result;

            if (newId > 0)
            {
                dto.DsUserId = newId;
                dto.Permissions.UserId = newId;

                _userService.UpdateUserPermissions(dto.Permissions, true).MergeInto(result);

                result.SetDataOnSuccess(newId);
            }

            // add supervisor security record
            if (dto.DsUserType == (int)UserType.Supervisor)
            {
                var securityId = _dsUser.InsertUserSupervisorSecurity(dto.DsUserId, dto.ClientId, 1, dto.EmployeeID);
            } 
            else if (dto.DsUserType == (int)UserType.CompanyAdmin)
            {
                // add company access record for the clients that the user will have access to
                var companyAccess = new List<UserClientAccessDto>()
                {
                    new UserClientAccessDto()
                    {
                        ClientId = dto.ClientId,
                        UserId = dto.DsUserId,
                        ClientName = null,
                        HasAccess = true,
                        IsBenefitAdmin = false,
                        IsClientAdmin = true
                    }
                };

                _userService.SaveCompanyAdminAccessForUser(dto.DsUserId, companyAccess).MergeInto(result);
            }

            return result;
        }

        IOpResult<UserPinDto> IUserProfileService.SaveCompanyAdminUserPin(UserPinDto requestPin)
        {
            var result = new OpResult<UserPinDto>(requestPin);

            _session.CanPerformAction(UserManagerActionType.UserReadWrite).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(requestPin.ClientId).MergeInto(result);

            if (result.HasError) return result;

            var entity = new UserPin
            {
                ClientId = requestPin.ClientId,
                Pin = requestPin.Pin,
                UserId = requestPin.UserId,
                Modified = DateTime.Now,
                ModifiedBy = _session.LoggedInUserInformation.UserId
            };

            _session.UnitOfWork.RegisterNew(entity);
            _session.UnitOfWork.RegisterPostCommitAction(() => requestPin.UserPinId = entity.UserPinId);
            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }
    }
}
