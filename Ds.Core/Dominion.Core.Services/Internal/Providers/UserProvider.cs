using Dominion.Authentication.Dto.Enums;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Security;
using Dominion.Core.Dto.User;
using Dominion.Core.Services.Api;
using Dominion.Core.Services.Interfaces;
using Dominion.Utility.Dto;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.OpResult;
using Dominion.Utility.Query;
using Dominion.Utility.Query.LinqKit;
using Dominion.Utility.Security.Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Authentication.Dto;
using Dominion.Authentication.Interface.Api;
using Dominion.Utility.DateAndTime;
using Newtonsoft.Json;
using UserPerformanceDashboardUserDto = Dominion.Core.Dto.User.UserPerformanceDashboardUserDto;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.User;
using Dominion.Utility.Containers;
using System.IO;
using System.Net.Http;
using System.Web;
using Dominion.Core.Services.Api.Auth;
using Dominion.Core.Services.Api.DataServicesInjectors;
using Dominion.Authentication.Interface.Api.Providers;

namespace Dominion.Core.Services.Internal.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly IBusinessApiSession _session;
        private readonly IClientSettingProvider _clientSettings;
        private readonly ILoginService _loginService;
        private readonly IDsDataServicesUserService _dsUser;
        private readonly ILoginProvider _loginProvider;

        internal IUserProvider Self { get; set; }

        internal UserProvider(IBusinessApiSession session, IClientSettingProvider clientSettings,
            ILoginService loginService, IDsDataServicesUserService dsUser, ILoginProvider loginProvider)
        {
            Self = this;

            _session = session;
            _clientSettings = clientSettings;
            _loginService = loginService;
            _dsUser = dsUser;
            _loginProvider = loginProvider;
        }

        IOpResult<IEnumerable<UserClientAccessDto>> IUserProvider.GetCurrentUserClientAccessSettings(int userId,
            bool isClientAdmin)
        {
            return new OpResult<IEnumerable<UserClientAccessDto>>().TrySetData(() => _session.UnitOfWork.UserRepository
                .QueryUserClientSettings()
                .ByUserId(userId)
                .ExecuteQueryAs(x => new UserClientAccessDto
                {
                    UserId = x.UserId,
                    ClientId = x.ClientId,
                    ClientName = x.Client.ClientName,
                    IsBenefitAdmin = x.IsBenefitAdmin,
                    HasAccess = true,
                    IsClientAdmin =
                        isClientAdmin //logic from spGetClientAdminListByUser ... if UserClient record exists then assume they are an admin
                })
                .ToList());
        }

        IOpResult<EmployeeUserAccessInfoDto> IUserProvider.GetEmployeeUserAccessInfo(int employeeId)
        {
            return new OpResult<EmployeeUserAccessInfoDto>().TrySetData(() =>
            {
                var raw = _session.UnitOfWork.EmployeeRepository.QueryEmployees()
                    .ByEmployeeId(employeeId)
                    .Result
                    .MapTo(x => new
                    {
                        x.EmployeeId,
                        x.ClientId,
                        x.ClientDepartmentId,
                        PayInfo = x.EmployeePayInfo.Select(p => new {p.Type, p.EmployeeStatusId}).FirstOrDefault(),
                        ClientDepartmentCode = x.Department != null ? x.Department.Code : null,
                    })
                    .FirstOrDefault();

                return new EmployeeUserAccessInfoDto
                {
                    EmployeeId = raw.EmployeeId,
                    ClientId = raw.ClientId,
                    PayType = raw.PayInfo?.Type,
                    ClientDepartmentId = raw.ClientDepartmentId,
                    ClientDepartmentCode = raw.ClientDepartmentCode,
                    EmployeeStatus = raw.PayInfo?.EmployeeStatusId
                };
            });
        }

        /// <summary>
        /// Checks if the currently logged in user has access to the specified employee.  This
        /// method overload will lookup (db-call) the employee properties necessary to perform
        /// the permission check.
        /// </summary>
        /// <param name="employeeId">Employee to check access to.</param>
        /// <param name="checkRateAccess">If true, will check for access to employee's pay-rate info.
        ///     If false (default), will only check general employee info access.</param>
        /// <param name="includeSelfEmployee">If true (default), will include supervisor's employee record even if they 
        /// do not have access to it per supervisor security.</param>
        /// <returns></returns>
        IOpResult IUserProvider.CanAccessEmployee(int employeeId, bool checkRateAccess, bool includeSelfEmployee)
        {
            var result = new OpResult();

            var ee = Self.GetEmployeeUserAccessInfo(employeeId).MergeInto(result).Data;
            if (result.HasError)
                return result;

            return Self.CanAccessEmployee(ee, ee.ClientId, checkRateAccess, includeSelfEmployee);
        }


        /// <summary>
        /// Checks if the currently logged in user has access to the specified employee.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="employee">Employee to check access to.</param>
        /// <param name="clientId"></param>
        /// <param name="checkRateAccess">If true, will check for access to employee's pay-rate info.
        ///     If false (default), will only check general employee info access.</param>
        /// <param name="includeSelfEmployee">If true (default), will include supervisor's employee record even if they 
        /// do not have access to it per supervisor security.</param>
        /// <returns></returns>
        IOpResult IUserProvider.CanAccessEmployee<T>(T employee, int clientId, bool checkRateAccess,
            bool includeSelfEmployee)
        {
            var result = new OpResult();
            var isSelf = employee.EmployeeId == _session.LoggedInUserInformation.UserEmployeeId;

            //check rate access if necessary
            if (checkRateAccess && (!isSelf || !includeSelfEmployee))
            {
                var canViewHourlyRateEmployees =
                    _session.CanPerformAction(ClientRateActionType.ViewHourlyRates).Success;
                var canViewSalaryRateEmployees =
                    _session.CanPerformAction(ClientRateActionType.ViewSalaryRates).Success;

                if (!canViewSalaryRateEmployees && employee.PayType == PayType.Salary)
                {
                    return result
                        .AddMessage(new NotYourResourceMsg())
                        .SetToFail();
                }

                if (!canViewHourlyRateEmployees && employee.PayType == PayType.Hourly)
                {
                    return result
                        .AddMessage(new NotYourResourceMsg())
                        .SetToFail();
                }
            }

            //now check user-client access and/or supervisor security access as necessary
            var eeAccessList = Self
                .FilterEmployeesByUserAccess(new[] {employee}, _session.LoggedInUserInformation.UserId, clientId)
                .MergeInto(result).Data.ToOrNewList();

            //if no employees, then the employee was filtered out due to lack of permissions
            if (!eeAccessList.Any())
            {
                return result
                    .AddMessage(new NotYourResourceMsg())
                    .SetToFail();
            }

            return result;
        }


        /// <summary>
        /// Filters a group of employees down to those the user has access to based on supervisor security settings. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="allEmployees">Full list of employees to filter down.</param>
        /// <param name="userId">ID of the user the filter by.</param>
        /// <param name="clientId">Client the employees belong to.</param>
        /// <param name="employeeId">If specified, resulting list will either contain only this employee or no employees (based on user's access).</param>
        /// <param name="payType"></param>
        /// <param name="eeStatus"></param>
        /// <param name="includeSupervisorEmulation">If true and user is a supervisor, employees returned will include the
        /// employees of other supervisors the specified user can emulate.</param>
        /// <param name="includeSelfEmployee">If true (default), will include supervisor's employee record even if they 
        /// do not have access to it per supervisor security.</param>
        /// <returns></returns>
        /// <remarks>Mimics functionality of fnGetEmployeesByUserID</remarks>
        IOpResult<IEnumerable<T>> IUserProvider.FilterEmployeesByUserAccess<T>(IEnumerable<T> allEmployees, int userId,
            int? clientId,
            int? employeeId, PayType? payType, EmployeeStatusType? eeStatus, bool includeSupervisorEmulation,
            bool includeSelfEmployee)
        {
            var result = new OpResult<IEnumerable<T>>();

            var userInfo = _session.UnitOfWork.UserRepository.QueryUsers()
                .ByUserId(userId)
                .ExecuteQueryAs(x => new
                {
                    x.UserId,
                    x.UserTypeId,
                    x.ViewEmployeePayTypes,
                    UserEmployeeId = x.EmployeeId,
                    UserEmployeeClientId = x.Employee != null ? x.Employee.ClientId : default(int?)
                })
                .FirstOrDefault();

            if (userInfo == null || userInfo.UserTypeId == UserType.Applicant ||
                userInfo.UserTypeId == UserType.Undefined || !Enum.IsDefined(typeof(UserType), userInfo.UserTypeId))
            {
                result.SetToFail().AddMessage(new NotYourResourceMsg());
                return result;
            }

            if (userInfo.UserTypeId == UserType.Employee)
            {
                if (employeeId.HasValue && userInfo.UserEmployeeId != employeeId)
                    return result.SetDataOnSuccess(new List<T>());


                return result.TrySetData(() =>
                    allEmployees.Where(ee => ee.EmployeeId == userInfo.UserEmployeeId).ToList());
            }

            //Make sure supervisor or company admin has access to the specified client
            if (userInfo.UserTypeId == UserType.Supervisor || userInfo.UserTypeId == UserType.CompanyAdmin)
            {
                var clientAccess = Self.GetCurrentUserClientAccessSettings(userId, isClientAdmin: true)
                    .MergeInto(result).Data?.FirstOrDefault(x => x.ClientId == clientId);
                if (clientAccess == null)
                {
                    result.SetToFail().AddMessage(new NotYourResourceMsg());
                    return result;
                }
            }

            // supervisor security
            var self = default(T);
            if (userInfo.UserTypeId == UserType.Supervisor)
            {
                //todo: make dynamic...right now just mimic sproc logic
                var types = new List<UserSecurityGroupType>
                {
                    UserSecurityGroupType.Employee,
                    UserSecurityGroupType.ClientDepartment
                };

                var supervisorInfo =
                    Self.GetSupervisorInfo(clientId.Value, userId, includeSupervisorEmulation, groupsToEmulate: types)
                        .MergeInto(result).Data?.FirstOrDefault();
                var accessCheck = Self.BuildSupervisorEmployeeAccessChecker<T>(supervisorInfo, accessGroups: types);

                if (includeSelfEmployee)
                    self = allEmployees.FirstOrDefault(ee => ee.EmployeeId == userInfo.UserEmployeeId);

                allEmployees = allEmployees.Where(ee => accessCheck(ee));
            }

            //apply user view-rate/pay-type restrictions
            allEmployees =
                allEmployees.Where(ee => CanViewEmployeeByPayType(userInfo.ViewEmployeePayTypes, ee.PayType));

            //apply other filters from paramaters
            if (payType.HasValue && payType < (PayType.Hourly | PayType.Salary))
                allEmployees = allEmployees.Where(ee => ee.PayType == payType);
            if (eeStatus.HasValue)
                allEmployees = allEmployees.Where(ee => ee.EmployeeStatus == eeStatus);
            if (employeeId.HasValue)
                allEmployees = allEmployees.Where(ee => ee.EmployeeId == employeeId);

            var accessibleEmployees = allEmployees.ToList();
            if (includeSelfEmployee && self != null && accessibleEmployees.All(ee => ee.EmployeeId != self.EmployeeId))
                accessibleEmployees.Add(self);

            result.SetDataOnSuccess(accessibleEmployees);

            return result;
        }

        Func<T, bool> IUserProvider.BuildSupervisorEmployeeAccessChecker<T>(
            SupervisorInfoDto supervisor,
            IEnumerable<UserSecurityGroupType> accessGroups,
            ClientDepartmentSetupInfoDto deptConfig,
            bool includeSupervisorPayTypeCheck = true)
        {
            //default to original sproc logic
            accessGroups = accessGroups.NullCheckToList() ??
                           new[] {UserSecurityGroupType.ClientDepartment, UserSecurityGroupType.Employee}.ToList();

            //todo: revisit - this supports current logic that if NO ee or dept restriction exists supervisor has access to ALL employees
            var defaultAccess = true;
            Expression<Func<T, bool>> checker = (ee) => defaultAccess;

            if (supervisor != null && accessGroups.Contains(UserSecurityGroupType.Employee) && supervisor.SupervisedEmployees.Any())
            {
                defaultAccess = false;
                var employees = supervisor.SupervisedEmployees;
                checker = checker.Or(x => employees.ContainsKey(x.EmployeeId));
            }

            if (supervisor != null && accessGroups.Contains(UserSecurityGroupType.ClientDepartment) && supervisor.SupervisedDepartments.Any())
            {
                defaultAccess = false;
                deptConfig = deptConfig ?? _clientSettings.GetClientDepartmentSetupInfo(supervisor.ClientId).Data;
                if (deptConfig?.UseDepartmentsAcrossDivisions ?? false)
                {
                    var supervisedCodes = supervisor.SupervisedDepartments
                        .Select(x => deptConfig.DepartmentCodeLookup.TryGetValue(x.Key, out string deptCode) ? deptCode : null)
                        .Where(x => !(x is null))
                        .Distinct()
                        .ToList();
                    checker = checker.Or(ee =>
                        !string.IsNullOrWhiteSpace(ee.ClientDepartmentCode) &&
                        supervisedCodes.Any(code =>
                            code.Equals(ee.ClientDepartmentCode, StringComparison.OrdinalIgnoreCase)));
                }
                else
                {
                    var depts = supervisor.SupervisedDepartments;
                    checker = checker.Or(ee =>
                        ee.ClientDepartmentId.HasValue && depts.ContainsKey(ee.ClientDepartmentId.Value));
                }
            }

            //check rate access
            if (includeSupervisorPayTypeCheck)
            {
                checker = checker.And(ee => CanViewEmployeeByPayType(supervisor.ViewEmployeePayType, ee.PayType));
            }

            return checker.Compile();
        }

        private bool CanViewEmployeeByPayType(UserViewEmployeePayType viewAccess, PayType? payType)
        {
            if (viewAccess == UserViewEmployeePayType.Both)
                return true;

            if (viewAccess == UserViewEmployeePayType.None || payType == null)
                return false;

            var iPayType = (int) payType.Value;
            var iViewAccess = (int) viewAccess;

            return iPayType == iViewAccess;
        }


        public class SupervisorRawUserDto
        {
            public int UserId { get; set; }
            public UserViewEmployeePayType ViewEmployeePayTypes { get; set; }
            public List<SupervisorGroupDto> GroupAccess { get; set; }
        }

        /// <summary>
        /// Returns the employee and group info a user supervises.
        /// </summary>
        /// <param name="clientId">ID of client to get supervisor security info for.</param>
        /// <param name="userId">If specified, will only return security info for the given supervisor. 
        /// Otherwise (default) supervisor info for all supervisors for the client will be returned.</param>
        /// <param name="includeSupervisorEmulation">If true, results will also include employees/groups of 
        /// other supervisors the user can emulate.</param>
        /// <param name="groupsToEmulate">Specifies which group types to emulate. Default is to only emulate 
        /// <see cref="UserSecurityGroupType.Employee"/> and <see cref="UserSecurityGroupType.ClientDepartment"/>.</param>
        /// <returns></returns>
        IOpResult<IEnumerable<SupervisorInfoDto>> IUserProvider.GetSupervisorInfo(int clientId, int? userId,
            bool includeSupervisorEmulation, IEnumerable<UserSecurityGroupType> groupsToEmulate)
        {
            var result = new OpResult<IEnumerable<SupervisorInfoDto>>();

            //get supervisor security group access (e.g. department, employees, etc the supervisor(s) have access to)
            var supQuery = _session.UnitOfWork.UserRepository
                .QueryUserClientSettings()
                .ByUserType(UserType.Supervisor)
                .ByClientId(clientId);

            //if requesting info for a single supervisor...query accordingly
            if (userId.HasValue)
                supQuery.ByUserId(userId.Value);

            var supRawData = supQuery
                .ExecuteQueryAs(x => new SupervisorRawUserDto
                {
                    UserId = x.UserId,
                    ViewEmployeePayTypes = x.User.ViewEmployeePayTypes,
                    GroupAccess = x.User.UserSupervisorSecurityGroupAccess
                        .Where(g => g.ClientId == clientId)
                        .Select(g => new SupervisorGroupDto
                        {
                            ForeignKeyId = g.ForeignKeyId,
                            GroupType = g.UserSecurityGroupId
                        }).ToList()
                })
                .ToList();

            if (userId.HasValue && supRawData.Count == 0)
            {
                supRawData = _session.UnitOfWork.UserRepository.QueryUserClientSettings().ByClientId(clientId)
                    .ByUserId(userId.GetValueOrDefault()).ExecuteQueryAs(x => new SupervisorRawUserDto
                    {
                        UserId = x.UserId,
                        ViewEmployeePayTypes = x.User.ViewEmployeePayTypes,

                    }).ToList();
            }

            ////get supervisor security group access (e.g. department, employees, etc the supervisor(s) have access to)
            //var supAccessQuery = _session.UnitOfWork.UserRepository
            //    .QuerySupervisorSecurityGroupAccess()
            //    .ByClientId(clientId);

            ////if requesting info for a single supervisor...query accordingly
            //if(userId.HasValue)
            //    supAccessQuery.ByUserId(userId.Value);

            //var supGroups = supAccessQuery
            //    .ExecuteQueryAs(x => new SupervisorGroupDto 
            //    {
            //        UserId       = x.UserId,
            //        GroupType    = x.UserSecurityGroupId,
            //        ForeignKeyId = x.ForeignKeyId
            //    })
            //    .GroupBy(x => x.UserId)
            //    .ToList();

            //build group access lookup info for each supervisor
            var supervisors = new List<SupervisorInfoDto>();
            foreach (var supUser in supRawData)
            {
                var allGroupLookup = new Dictionary<UserSecurityGroupType, IDictionary<int, SupervisorGroupDto>>
                {
                    [UserSecurityGroupType.None] = new Dictionary<int, SupervisorGroupDto>(),
                    [UserSecurityGroupType.Employee] = new Dictionary<int, SupervisorGroupDto>(),
                    [UserSecurityGroupType.ClientDepartment] = new Dictionary<int, SupervisorGroupDto>(),
                    [UserSecurityGroupType.Report] = new Dictionary<int, SupervisorGroupDto>(),
                    [UserSecurityGroupType.ClientCostCenter] = new Dictionary<int, SupervisorGroupDto>(),
                    [UserSecurityGroupType.UserEmulation] = new Dictionary<int, SupervisorGroupDto>()
                };

                foreach (var group in supUser.GroupAccess.ToOrNewList())
                {
                    switch (group.GroupType)
                    {
                        case UserSecurityGroupType.None:
                        case UserSecurityGroupType.Employee:
                        case UserSecurityGroupType.ClientDepartment:
                        case UserSecurityGroupType.Report:
                        case UserSecurityGroupType.ClientCostCenter:
                        case UserSecurityGroupType.UserEmulation:

                            allGroupLookup[group.GroupType][group.ForeignKeyId] = group;

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                var sup = new SupervisorInfoDto
                {
                    ClientId = clientId,
                    UserId = supUser.UserId,
                    ViewEmployeePayType = supUser.ViewEmployeePayTypes,
                    AllGroups = allGroupLookup,
                };

                supervisors.Add(sup);
            }

            //if we're supposed to emulate supervisors loop back through each supervisor and 
            //include group access for emulated groups
            if (includeSupervisorEmulation)
            {
                foreach (var sup in supervisors)
                {
                    var emulatedSupervisors = new List<SupervisorInfoDto>();

                    //check if the supervisor is setup to emulate another supervisor
                    if (sup.AllGroups.ContainsKey(UserSecurityGroupType.UserEmulation))
                    {
                        //setup what groups to emulate
                        sup.EmulatedGroupTypes = groupsToEmulate ?? new[]
                        {
                            UserSecurityGroupType.ClientDepartment, UserSecurityGroupType.Employee
                        };

                        //loop through each emulated supervisor
                        foreach (var supItem in sup.AllGroups[UserSecurityGroupType.UserEmulation])
                        {
                            //get emulated supervisor info WITHOUT emulation
                            //if we've already gotten all client's supervisors use that one
                            //otherwise, reccursively get the emulated supervisor's info
                            var emulatedSup = userId.HasValue
                                ? supervisors.FirstOrDefault(s => s.UserId == supItem.Key)
                                : Self.GetSupervisorInfo(clientId, supItem.Key, includeSupervisorEmulation = false)
                                    .MergeInto(result).Data?.FirstOrDefault();
                            if (emulatedSup != null)
                            {
                                // add emulated info to main supervisor's group access
                                foreach (var emulatedGroupType in emulatedSup.AllGroups.Where(g =>
                                    sup.EmulatedGroupTypes.Contains(g.Key)))
                                {
                                    //make sure only the groups the emulated supervisor has direct access to are included
                                    //(i.e. don't emulate an emulated group)
                                    foreach (var emulatedGroup in emulatedGroupType.Value.Where(
                                        x => !x.Value.IsEmulated))
                                    {
                                        //only add the emulated group if the supervisor didn't already have access to it
                                        if (!sup.AllGroups[emulatedGroupType.Key].ContainsKey(emulatedGroup.Key))
                                        {
                                            sup.AllGroups[emulatedGroupType.Key][emulatedGroup.Key] =
                                                emulatedGroup.Value;
                                            emulatedGroup.Value.IsEmulated = true;
                                            emulatedGroup.Value.EmulatedUserId = emulatedSup.UserId;
                                        }
                                    }

                                }

                                emulatedSupervisors.Add(emulatedSup);
                            }
                        }
                    }

                    sup.EmulatedSupervisors = emulatedSupervisors;
                    sup.IncludesEmulatedGroups = true;
                }
            }

            result.SetDataOnSuccess(supervisors);

            return result;
        }

        public IOpResult<ICollection<UserNameDto>> GetSupervisorsForCurrentClient(int? clientId=null)
        {
            var result = new OpResult<ICollection<UserNameDto>>();
            if(clientId==null)
            {
                clientId = _session.LoggedInUserInformation.ClientId;
            }
            result.TrySetData(() =>
            {
                var query = _session.UnitOfWork.EmployeeRepository
                .GetEmployees(new QueryBuilder<Domain.Entities.Employee.Employee, UserNameDto>(x => new UserNameDto()
                {
                    FirstName = x.UserAccounts.FirstOrDefault().FirstName,
                    LastName = x.UserAccounts.FirstOrDefault().LastName,
                    UserId = x.UserAccounts.FirstOrDefault().UserId,
                    IsActive = x.EmployeePayInfo.Select(y => y.EmployeeStatus.IsActive).FirstOrDefault(),
                    IsUserDisabled = x.UserAccounts.FirstOrDefault().IsUserDisabled,
                    UserType = x.UserAccounts.FirstOrDefault().UserTypeId,
                    EmployeeId = x.EmployeeId
                }).FilterBy(x => x.ClientId == clientId && x.UserAccounts.FirstOrDefault().UserTypeId == UserType.Supervisor));
                
                return query.ToList();
            });
            return result;
        }

        public IOpResult<IEnumerable<UserNameDto>> GetCompanyAdministratorsForCurrentClient(int? clientId=null)
        {
            var result = new OpResult<IEnumerable<UserNameDto>>();
            if (clientId == null)
            {
                clientId = _session.LoggedInUserInformation.ClientId;
            }
            result.TrySetData(() => _session.UnitOfWork.UserRepository.GetUsers(
                new QueryBuilder<Domain.Entities.User.User, UserNameDto>(x => new UserNameDto()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserId = x.UserId,
                    IsActive = true,
                    IsUserDisabled = x.IsUserDisabled,
                    UserType = x.UserTypeId
                })
                .FilterBy(x => x.UserTypeId == UserType.CompanyAdmin && x.UserClientSettings.FirstOrDefault(y => y.ClientId == clientId) != null)));
            return result;
        }

        public IOpResult<UserNameDto> GetSupervisorForEmployee(int employeeId)
        {
            var result = new OpResult<UserNameDto>();

            result.TrySetData(() => _session.UnitOfWork.EmployeeRepository.GetEmployee(employeeId, x =>
                new UserNameDto()
                {
                    UserId = x.DirectSupervisorID,
                    FirstName = x.DirectSupervisor.FirstName,
                    LastName = x.DirectSupervisor.LastName
                }));

            return result;

        }

		#region Authorization

        IOpResult<IEnumerable<int>> IUserProvider.GetClientIdsForUser(int userId)
        {
            var opResult = new OpResult<IEnumerable<int>>();

            var user = _session.UnitOfWork.UserRepository.GetUser(userId);
            if (user.UserTypeId == UserType.CompanyAdmin)
            {
                var userClients = _session.UnitOfWork.UserRepository.QueryUserClientSettings()
                    .ByUserId(userId)
                    .ExecuteQueryAs(uc => uc.ClientId);

                opResult.SetToSuccess();
                opResult.SetDataOnSuccess(userClients);
                return opResult;
            }

            //account for when single client -- Userclient may not return results; rely on employee record.
            opResult.SetToSuccess();
            opResult.SetDataOnSuccess(new List<int>() {user.Employee.ClientId});
            return opResult;
        }

        IOpResult<IEnumerable<UserClientDto>> IUserProvider.GetAssociatedClientsForUser(int userId)
        {
            var opResult = new OpResult<IEnumerable<UserClientDto>>();

            opResult.TrySetData(() =>
            {
                return _session.UnitOfWork.UserRepository.QueryUserClientSettings()
                    .ByUserId(userId)
                    .ExecuteQueryAs(uc =>
                        new UserClientDto()
                        {
                            ClientId = uc.ClientId,
                            UserId = uc.UserId,
                            IsClientAdmin = uc.IsClientAdmin,
                            IsBenefitAdmin = uc.IsBenefitAdmin
                        });
            });

            return opResult;
        }

        IOpResult<UserChangeDto> IUserProvider.SetAuthStatus(int authUserId, AccessStatusId statusId, int modifiedBy)
        {
            var opResult = new OpResult<UserChangeDto>();

            _loginService.SetUserProperties(
                new UserChangeDto()
                {
                    AuthUserId = authUserId,
                    AccessStatusId = statusId,
                    ModifiedBy = modifiedBy
                }).MergeInto(opResult);

            return opResult;
        }

        IOpResult IUserProvider.IsAnyClientMfaRequiredForUser(int userId)
        {
            var opResult = new OpResult();
            var clientIds = new List<int>();
            var clientMfaSettings = new List<ClientMfaSettingsDto>();

            var result = (this as IUserProvider).GetClientIdsForUser(userId);

            //Get all the client MFA attributes
            clientIds.ForEach(c =>
                clientMfaSettings.Add(_loginService.GetAllClientMfaSettings(c).Data)
            );

            //check if any clients use MFA (loop through all the MFA settings and check each one for the required field)
            var isAnyClientMfaEnabled = clientMfaSettings.Any(mfa =>
                mfa.MfaSettings.Any(data =>
                    data.AuthUserTypeId == (int) AuthUserTypeId.CoAdmin &&
                    data.Required
                )
            );

            //can only disable MFA is no clients Company Admin MFA settings are enabled
            return isAnyClientMfaEnabled ? opResult.SetToSuccess() : opResult.SetToFail();
        }

		#endregion

        IOpResult<UserChangeDto> IUserProvider.DisableAuthUser(int authUserId, int modifiedByAuthUserId)
        {
            var opResult = new OpResult<UserChangeDto>();

            _loginService.SetUserProperties(
                new UserChangeDto()
                {
                    AuthUserId = authUserId,
                    Locked = true,
                    ModifiedBy = modifiedByAuthUserId
                }).MergeInto(opResult);

            return opResult;
        }

        IOpResult IUserProvider.UpdateAllowAccessUntil(int authUserId, DateTime? allowAccessUntilDate, AccessStatusId userAccessStatus, int modifiedByAuthUserId)
        {
            var r = new OpResult();
            var result = _loginService.GetUserSecuritySettings(authUserId);


            r.TryCatch(() =>
            {
                if (result.Success)
                {
                    var data = result.HasData ? result.Data.ToList() : new List<SecurityAttributeDto>();

                    //check if record already exists
                    var isRecordFound = data.Any(d => d.SecurityAttributeType == SecurityAttributeTypeId.AllowClientAccessUntil);
                    var deleteIfExists = userAccessStatus != AccessStatusId.ProvisionalAccessWithDate;
                    var isDeleteNeeded = isRecordFound && deleteIfExists;
                    var sa = new SecurityAttributeDto
                    {
                        AuthUserId = authUserId,
                        SecurityAttributeId = (byte)SecurityAttributeTypeId.AllowClientAccessUntil,
                        //review: jay: it would be nice if I added a generic SecurityAttributeDto that
                        //takes a generica object and then the call to SetClientSecuritySetting did the JSON 
                        //serialization, instead of me doing it here.
                        Data = userAccessStatus == AccessStatusId.ProvisionalAccessWithDate
                            ? JsonConvert.SerializeObject(new DateRange()
                            {
                                Start = DateTime.Now,
                                End = allowAccessUntilDate ?? DateTime.Today
                            })
                            : null,
                        IsDelete = isDeleteNeeded,
                        ModifiedBy = modifiedByAuthUserId,
                    };

                    if(userAccessStatus == AccessStatusId.ProvisionalAccessWithDate || isDeleteNeeded)
                        _loginService.SetUserSecuritySetting(sa).MergeInto(r);
                }
            });

            return r;
        }

        public IOpResult<ICollection<UserPerformanceDashboardUserDto>> GetSupervisorsForUserDashboard(int? clientId = null)
        {
            var result = new OpResult<ICollection<UserPerformanceDashboardUserDto>>();
            result.TrySetData(() => _session.UnitOfWork.UserRepository.GetUsers(
                new QueryBuilder<Domain.Entities.User.User, UserPerformanceDashboardUserDto>(x => new UserPerformanceDashboardUserDto()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserId = x.UserId,
                    LastLoginDate = x.Session != null ? x.Session.LastLogin : default,
                    IsUserDisabled = x.IsUserDisabled,
                    UserTypeId = x.UserTypeId,
                    EmployeeId = x.Employee.EmployeeId,
                    UserName = x.UserName,
                    IsSecurityEnabled = x.IsSecurityEnabled.ToString(),
                    IsPasswordEnabled = x.IsPasswordEnabled,
                    Department = x.Employees.Select(w => w.Department.Name).FirstOrDefault(),
                    SupervisorId = x.Employee.DirectSupervisorID,
                    HireDate = x.Employee.HireDate,
                    EmployeeStatus = x.Employee.EmployeePayInfo.Select(s => s.EmployeeStatus).FirstOrDefault().Description,
                    TempEnableFromDate = x.TempEnableFromDate,
                    TempEnableToDate = x.TempEnableToDate,
                    ViewEmployeePayTypes = x.ViewEmployeePayTypes.ToString(),
                    ViewEmployeeRateTypes = x.ViewEmployeeRateTypes.ToString(),
                    AssignedEmployee = "",
                    EmailAddress = x.EmailAddress,
                    CertifyI9 = x.Employees.Select(s => s.UserAccounts.Select(d => d.UserSupervisorSecuritySettings.FirstOrDefault().CertifyI9).FirstOrDefault()).FirstOrDefault(),
                    AddEmployee = x.Employees.Select(s => s.UserAccounts.Select(d => d.UserSupervisorSecuritySettings.FirstOrDefault().AddEmployee).FirstOrDefault()).FirstOrDefault(),
                    ResetPassword = x.Employees.Select(s => s.UserAccounts.Select(d => d.UserSupervisorSecuritySettings.FirstOrDefault().CanSendPasswords).FirstOrDefault()).FirstOrDefault(),
                    ApproveTimeCards = x.Employees.Select(s => s.UserAccounts.Select(d => d.UserSupervisorSecuritySettings.FirstOrDefault().IsAllowApproveHours).FirstOrDefault()).FirstOrDefault(),
                    IsActive = x.Employee.EmployeePayInfo.Select(w => w.EmployeeStatus.IsActive).FirstOrDefault(),
                    Supervisor = x.Employees.Select(w => w.DirectSupervisor.LastName + ", " + w.DirectSupervisor.FirstName).FirstOrDefault()
                })
                .FilterBy(x => x.UserTypeId == UserType.Supervisor && x.UserClientSettings.FirstOrDefault(y => y.ClientId == clientId) != null)).ToList());

            var temp = result.Data.ToList();

            foreach (var i in temp)
            {
                if (i.IsPasswordEnabled == true)
                {
                    i.AssignedEmployee = i.LastName + ", " + i.FirstName;
                }
                if (i.ViewEmployeePayTypes == "1")
                {
                    i.ViewEmployeePayTypes = "Hourly";
                }
                if (i.ViewEmployeePayTypes == "2")
                {
                    i.ViewEmployeePayTypes = "Salary";
                }
                if (i.ViewEmployeePayTypes == "3")
                {
                    i.ViewEmployeePayTypes = "All";
                }
                if (i.ViewEmployeePayTypes == "4")
                {
                    i.ViewEmployeePayTypes = "None";
                }
                if (i.IsEmployeeSelfServiceViewOnly == "0")
                {
                    i.IsEmployeeSelfServiceViewOnly = "Yes";
                }
                if (i.IsEmployeeSelfServiceViewOnly == "1")
                {
                    i.IsEmployeeSelfServiceViewOnly = "No";
                }
                if (i.IsSecurityEnabled == "True")
                {
                    i.IsSecurityEnabled = "Yes";
                }
                if (i.IsSecurityEnabled == "False")
                {
                    i.IsSecurityEnabled = "No";
                }
            }

            result.Data = temp;

            return result;
        }
        public IOpResult<ICollection<UserPerformanceDashboardUserDto>> GetCompanyAdministratorsForUserDashboard(int? clientId = null)
        {
            var result = new OpResult<ICollection<UserPerformanceDashboardUserDto>>();
            if (clientId == null)
            {
                clientId = _session.LoggedInUserInformation.ClientId;
            }
            result.TrySetData(() => _session.UnitOfWork.UserRepository.GetUsers(
                new QueryBuilder<Domain.Entities.User.User, UserPerformanceDashboardUserDto>(x => new UserPerformanceDashboardUserDto()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserId = x.UserId,
                    LastLoginDate = x.Session != null ? x.Session.LastLogin : default,
                    IsUserDisabled = x.IsUserDisabled,
                    UserTypeId = x.UserTypeId,
                    EmployeeId = x.Employee.EmployeeId,
                    UserName = x.UserName,
                    IsSecurityEnabled = x.IsSecurityEnabled.ToString(),
                    IsPasswordEnabled = x.IsPasswordEnabled,
                    Department = x.Employee.Department.Name,
                    SupervisorId = x.Employee.DirectSupervisorID,
                    HireDate = x.Employee.HireDate,
                    EmployeeStatus = x.Employee.EmployeePayInfo.Select(s => s.EmployeeStatus).FirstOrDefault().Description,
                    TempEnableFromDate = x.TempEnableFromDate,
                    TempEnableToDate = x.TempEnableToDate,
                    ViewEmployeePayTypes = x.ViewEmployeePayTypes.ToString(),
                    ViewEmployeeRateTypes = x.ViewEmployeeRateTypes.ToString(),
                    AssignedEmployee = "",
                    EmailAddress = x.EmailAddress,
                    IsActive = x.Employee.EmployeePayInfo.Select(w => w.EmployeeStatus.IsActive).FirstOrDefault(),
                    Supervisor = x.Employees.Select(w => w.DirectSupervisor.LastName + ", " + w.DirectSupervisor.FirstName).FirstOrDefault()
                })
                .FilterBy(x => x.UserTypeId == UserType.CompanyAdmin && x.UserClientSettings.FirstOrDefault(y => y.ClientId == clientId) != null)).ToList());

            var temp = result.Data.ToList();

            foreach (var i in temp)
            {
                if (i.IsPasswordEnabled == true)
                {
                    i.AssignedEmployee = i.LastName + ", " + i.FirstName;
                }
                if (i.ViewEmployeePayTypes == "1")
                {
                    i.ViewEmployeePayTypes = "Hourly";
                }
                if (i.ViewEmployeePayTypes == "2")
                {
                    i.ViewEmployeePayTypes = "Salary";
                }
                if (i.ViewEmployeePayTypes == "3")
                {
                    i.ViewEmployeePayTypes = "All";
                }
                if (i.ViewEmployeePayTypes == "4")
                {
                    i.ViewEmployeePayTypes = "None";
                }
                if (i.IsEmployeeSelfServiceViewOnly == "0")
                {
                    i.IsEmployeeSelfServiceViewOnly = "Yes";
                }
                if (i.IsEmployeeSelfServiceViewOnly == "1")
                {
                    i.IsEmployeeSelfServiceViewOnly = "No";
                }
                if (i.IsSecurityEnabled == "True")
                {
                    i.IsSecurityEnabled = "Yes";
                }
                if (i.IsSecurityEnabled == "False")
                {
                    i.IsSecurityEnabled = "No";
                }
            }

            result.Data = temp;

            return result;
        }
        public IOpResult<ICollection<UserPerformanceDashboardUserDto>> GetActiveEmployeesForUserDashboard(int? clientId = null)
        {
            var result = new OpResult<ICollection<UserPerformanceDashboardUserDto>>();
            if (clientId == null)
            {
                clientId = _session.LoggedInUserInformation.ClientId;
            }
            result.TrySetData(() => _session.UnitOfWork.UserRepository.GetUsers(
                new QueryBuilder<Domain.Entities.User.User, UserPerformanceDashboardUserDto>(x => new UserPerformanceDashboardUserDto()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserId = x.UserId,
                    LastLoginDate = x.Session != null ? x.Session.LastLogin : default,
                    IsUserDisabled = x.IsUserDisabled,
                    UserTypeId = x.UserTypeId,
                    EmployeeId = x.Employee.EmployeeId,
                    UserName = x.UserName,
                    IsSecurityEnabled = x.IsSecurityEnabled.ToString(),
                    IsPasswordEnabled = x.IsPasswordEnabled,
                    Department = x.Employee.Department.Name,
                    SupervisorId = x.Employee.DirectSupervisorID,
                    HireDate = x.Employee.HireDate,
                    EmployeeStatus = x.Employee.EmployeePayInfo.Select(s => s.EmployeeStatus).FirstOrDefault().Description,
                    TempEnableFromDate = x.TempEnableFromDate,
                    TempEnableToDate = x.TempEnableToDate,
                    ViewEmployeePayTypes = x.ViewEmployeePayTypes.ToString(),
                    ViewEmployeeRateTypes = x.ViewEmployeeRateTypes.ToString(),
                    AssignedEmployee = "",
                    EmailAddress = x.EmailAddress,
                    IsActive = x.Employee.EmployeePayInfo.Select(w => w.EmployeeStatus.IsActive).FirstOrDefault(),
                    Supervisor = x.Employees.Select(w => w.DirectSupervisor.LastName + ", " + w.DirectSupervisor.FirstName).FirstOrDefault()
                })
                .FilterBy(x => x.UserTypeId == UserType.Employee && x.UserClientSettings.FirstOrDefault(y => y.ClientId == clientId) != null)).ToList());

            var temp = result.Data.ToList();

            foreach (var i in temp)
            {
                if (i.IsPasswordEnabled == true)
                {
                    i.AssignedEmployee = i.LastName + ", " + i.FirstName;
                }
                if (i.ViewEmployeePayTypes == "1")
                {
                    i.ViewEmployeePayTypes = "Hourly";
                }
                if (i.ViewEmployeePayTypes == "2")
                {
                    i.ViewEmployeePayTypes = "Salary";
                }
                if (i.ViewEmployeePayTypes == "3")
                {
                    i.ViewEmployeePayTypes = "All";
                }
                if (i.ViewEmployeePayTypes == "4")
                {
                    i.ViewEmployeePayTypes = "None";
                }
                if (i.IsEmployeeSelfServiceViewOnly == "0")
                {
                    i.IsEmployeeSelfServiceViewOnly = "Yes";
                }
                if (i.IsEmployeeSelfServiceViewOnly == "1")
                {
                    i.IsEmployeeSelfServiceViewOnly = "No";
                }
                if (i.IsSecurityEnabled == "True")
                {
                    i.IsSecurityEnabled = "Yes";
                }
                if (i.IsSecurityEnabled == "False")
                {
                    i.IsSecurityEnabled = "No";
                }
            }

            result.Data = temp;
            return result;
        }

        IOpResult<UserBetaFeatureDto> IUserProvider.GetBetaFeature(int userId, int betaFeatureId)
        {
            var result = new OpResult<UserBetaFeatureDto>();
            var eubf = _session.UnitOfWork.UserRepository
                .UserBetaFeatureQuery()
                .ByUserId(userId)
                .ByBetaFeatureId(1)
                .FirstOrDefault();
            if(eubf==null)
            {
                var newUBF = new UserBetaFeature()
                {
                    UserId=userId,
                    BetaFeatureId=1,
                    IsBetaActive=true
                };
                _session.SetModifiedProperties(newUBF);
                _session.UnitOfWork.RegisterNew(newUBF);
                _session.UnitOfWork.Commit();
            }

            return result.TrySetData(() => _session.UnitOfWork.UserRepository
                .UserBetaFeatureQuery()
                .ByUserId(userId)
                .ByBetaFeatureId(betaFeatureId)
                .ExecuteQueryAs(x => new UserBetaFeatureDto
                {
                    UserBetaFeatureId = x.UserBetaFeatureId,
                    UserId = x.UserId,
                    BetaFeatureId = x.BetaFeatureId,
                    IsBetaActive = x.IsBetaActive,
                    BetaFeature = x.BetaFeature != null
                        ? new BetaFeatureDto
                        {
                            BetaFeatureId = x.BetaFeature.BetaFeatureId,
                            Name = x.BetaFeature.Name,
                            Description = x.BetaFeature.Name,
                            StartDate = x.BetaFeature.StartDate,
                            EndDate = x.BetaFeature.EndDate,
                            DeletedAt = x.BetaFeature.DeletedAt
                        }
                        : default(BetaFeatureDto)
                })
                .FirstOrDefault());
        }

        IOpResult<UserTermsAndConditionsDto> IUserProvider.GetUserTermsAndConditions(int userId)
        {
            var result = new OpResult<UserTermsAndConditionsDto>();

            result.TrySetData(() =>
            {
                return _session.UnitOfWork.UserRepository
                .QueryUserTermsAndConditions()
                .ByUserId(userId)
                .ExecuteQueryAs(x => new UserTermsAndConditionsDto()
                {
                    AcceptDate = x.AcceptDate,
                    TermsAndConditionsVersionId = x.TermsAndConditionsVersionId,
                    UserAccepted = x.UserAccepted,
                    UserId = x.UserId,
                    UserTermsAndConditionsID = x.UserTermsAndConditionsID
                })
                .FirstOrDefault();
            });
            return result;
        }

        IOpResult<TermsAndConditionsVersionsDto> IUserProvider.GetTermsAndConditionsVersions()
        {
            var result = new OpResult<TermsAndConditionsVersionsDto>();

            result.TrySetData(() =>
            {
                var data = _session.UnitOfWork.CoreRepository
                .QueryTermsAndConditionsVersions()
                .ExecuteQueryAs(x => new TermsAndConditionsVersionsDto
                {
                    EffectiveDate = x.EffectiveDate,
                    FileLocation = x.FileLocation,
                    FileName = x.FileName,
                    LastEffectiveDate = x.LastEffectiveDate,
                    TermsAndConditionsVersionID = x.TermsAndConditionsVersionID
                })
                .OrderByDescending(x => x.EffectiveDate);

                var dto = data.FirstOrDefault();

                dto.File = new StreamContent(new FileStream(dto.FileLocation, FileMode.Open, FileAccess.Read));

                return dto;
            });

            return result;
        }

        IOpResult<IEnumerable<TermsAndConditionsVersionsDto>> IUserProvider.GetAllTermsAndConditionsVersions()
        {
            var result = new OpResult<IEnumerable<TermsAndConditionsVersionsDto>>();

            result.TrySetData(() =>
            {
                var data = _session.UnitOfWork.CoreRepository
                .QueryTermsAndConditionsVersions()
                .ExecuteQueryAs(x => new TermsAndConditionsVersionsDto
                {
                    EffectiveDate               = x.EffectiveDate,
                    FileLocation                = x.FileLocation,
                    FileName                    = x.FileName,
                    LastEffectiveDate           = x.LastEffectiveDate,
                    TermsAndConditionsVersionID = x.TermsAndConditionsVersionID
                }).OrderByDescending(x => x.EffectiveDate);
                
                return data;
            });

            return result;
        }

        IOpResult IUserProvider.SaveUserProfile(UserProfileDto dto)
        {
            var result = new OpResult();

            return result.TryCatch(() =>
            {
                User e;

                if (dto.UserId > 0)
                {
                    Self.UpdateUserProfile(dto).MergeInto(result);
                }
                else
                {
                    e = MapDtoToUserProfileEntity(dto);

                    Self.CreateUserProfile(dto).MergeInto(result);
                    _session.UnitOfWork.RegisterPostCommitAction(() => dto.UserId = e.UserId);
                }

                _session.UnitOfWork.Commit().MergeInto(result);
            });
        }

        IOpResult IUserProvider.UpdateUserProfile(UserProfileDto dto)
        {
            var result = new OpResult();
            var clientId = _session.LoggedInUserInformation.ClientId.GetValueOrDefault(0);

            #region Get Existing User

            var user = _session.UnitOfWork.UserRepository
                .QueryUsers()
                .ByUserId(dto.UserId)
                .ExecuteQueryAs(x => new
                {
                    x.UserId, 
                    x.AuthUserId,
                    x.UserTypeId,
                    x.FirstName,
                    x.LastName,
                    x.EmployeeId,
                    x.EmailAddress,
                    x.MustChangePassword,
                    x.IsPasswordEnabled,
                    x.IsUserDisabled,
                    x.TimeoutMinutes,
                    x.ViewEmployeePayTypes,
                    x.ViewEmployeeRateTypes,
                    x.IsEmployeeSelfServiceViewOnly,
                    x.IsHrBlocked,
                    x.IsEmployeeSelfServiceOnly,
                    x.IsReportingOnly,
                    x.IsEditGlEnabled,
                    x.IsPayrollAccessBlocked,
                    x.CanViewTaxPackets,
                    x.IsApplicantTrackingAdmin,
                    x.IsTimeclockEnabled,
                    x.TimeclockAppOnly,
                    UserPin = x.UserPin != null ? new UserPinDto
                    {
                        UserPinId = x.UserPin.UserPinId,
                        ClientId = x.UserPin.ClientId,
                        Pin = x.UserPin.Pin,
                        ClientContactId = x.UserPin.ClientContactId,
                        UserId = x.UserPin.UserId
                    } : default,
                    Permissions = x.Permissions != null ? new UserPermissionsDto
                    {
                        UserId = x.Permissions.UserId,
                        IsEmployeeNavigatorAdmin = x.Permissions.IsEmployeeNavigatorAdmin
                    } : default,
                })
                .FirstOrDefault();

            if (user == null)
            {
                return result.SetToFail("Could not find specified user. Please reload page and try again.");
            }

            #endregion

            return result.TryCatch(() =>
            {
                var entity = MapDtoToUserProfileEntity(dto);

                #region User Pin

                if (!string.IsNullOrWhiteSpace(dto.UserPin))
                {
                    var userPinEntity = new UserPin
                    {
                        UserPinId = user.UserPin != null ? user.UserPin.UserPinId : default,
                        UserId = user.UserId,
                        Pin = dto.UserPin
                    };

                    if (userPinEntity.UserPinId > 0)
                    {
                        var up = new PropertyList<UserPin>();

                        if (user.UserPin != null && user.UserPin.Pin != dto.UserPin)
                            up.Include(x => x.Pin);

                        if (up.Any())
                            _session.UnitOfWork.RegisterModified(userPinEntity, up);
                    }
                    else
                    {
                        _session.UnitOfWork.RegisterPostCommitAction(() => user.UserPin.UserPinId = userPinEntity.UserPinId);
                        _session.UnitOfWork.RegisterNew(userPinEntity);
                    }
                }

                #endregion

                #region User Permissions

                var permissionEntity = new UserPermissions
                {
                    UserId = user.UserId,
                    IsEmployeeNavigatorAdmin = dto.IsEmployeeNavigatorAdmin
                };

                if (user.Permissions != null && user.Permissions.UserId > 0)
                {
                    var permissionProps = new PropertyList<UserPermissions>();

                    if (user.Permissions.IsEmployeeNavigatorAdmin != dto.IsEmployeeNavigatorAdmin)
                        permissionProps.Include(x => x.IsEmployeeNavigatorAdmin);

                    if (permissionProps.Any())
                        _session.UnitOfWork.RegisterModified(permissionEntity, permissionProps);
                }
                else
                {
                    _session.UnitOfWork.RegisterNew(permissionEntity);
                }

                #endregion

                #region User 

                var userEntity = MapDtoToUserProfileEntity(dto);

                var props = new PropertyList<User>();

                if (user.UserTypeId != dto.UserType)
                    props.Include(x => x.UserTypeId);
                if (user.FirstName != dto.FirstName)
                    props.Include(x => x.FirstName);
                if (user.LastName != dto.LastName)
                    props.Include(x => x.LastName);
                if (user.EmployeeId != dto.EmployeeId)
                    props.Include(x => x.EmployeeId);
                if (user.EmailAddress != dto.Email)
                    props.Include(x => x.EmailAddress);
                if (user.MustChangePassword != dto.ForceUserPasswordReset)
                    props.Include(x => x.MustChangePassword);
                if (user.IsPasswordEnabled != dto.IsAccountEnabled)
                    props.Include(x => x.IsPasswordEnabled);
                if (user.IsUserDisabled != dto.IsUserDisabled)
                    props.Include(x => x.IsUserDisabled);
                if (user.TimeoutMinutes != dto.SessionTimeout)
                    props.Include(x => x.TimeoutMinutes);
                if (user.ViewEmployeePayTypes != dto.ViewEmployeesType)
                    props.Include(x => x.ViewEmployeePayTypes);
                if (user.ViewEmployeeRateTypes != dto.ViewRatesType)
                    props.Include(x => x.ViewEmployeeRateTypes);
                if (user.IsEmployeeSelfServiceViewOnly != dto.IsEssViewOnly)
                    props.Include(x => x.IsEmployeeSelfServiceViewOnly);
                if (user.IsHrBlocked != dto.BlockHr)
                    props.Include(x => x.IsHrBlocked);
                if (user.IsEmployeeSelfServiceOnly != dto.HasEssSelfService)
                    props.Include(x => x.IsEmployeeSelfServiceOnly);
                if (user.IsReportingOnly != dto.IsReportingAccessOnly)
                    props.Include(x => x.IsReportingOnly);
                if (user.IsEditGlEnabled != dto.HasGLAccess)
                    props.Include(x => x.IsEditGlEnabled);
                if (user.IsPayrollAccessBlocked != dto.BlockPayrollAccess)
                    props.Include(x => x.IsPayrollAccessBlocked);
                if (user.CanViewTaxPackets != dto.HasTaxPacketsAccess)
                    props.Include(x => x.CanViewTaxPackets);
                if (user.IsApplicantTrackingAdmin != dto.isApplicantTrackingAdmin)
                    props.Include(x => x.IsApplicantTrackingAdmin);
                if (user.IsTimeclockEnabled != dto.HasTimeAndAttAccess)
                    props.Include(x => x.IsTimeclockEnabled);
                if (user.TimeclockAppOnly != dto.IsTimeclockAppOnly)
                    props.Include(x => x.TimeclockAppOnly);

                if (props.Any())
                {
                    _session.UnitOfWork.RegisterModified(userEntity, props);
                }

                #endregion

                #region Auth User 

                if (user.AuthUserId > 0)
                {
                    var changeUser = new UserChangeDto
                    {
                        AuthUserId = user.AuthUserId.Value,
                        DsUserId1 = user.UserId,
                        ModifiedBy = _session.LoggedInUserInformation.UserId,
                        UserTypeId = (AuthUserTypeId)dto.UserType
                    };

                    _loginService.SetUserProperties(changeUser).MergeInto(result);
                }

                #endregion

                #region User Client

                // 1st condition - User is changing the user type from CA to something else 
                // 2nd condition - Changing from something else to a CA which requires UserClient records
                if (user.UserTypeId != dto.UserType 
                    && (user.UserTypeId == UserType.CompanyAdmin || dto.UserType == UserType.CompanyAdmin))
                {
                    if (user.UserTypeId == UserType.CompanyAdmin)
                    {
                        var remove = new UserClient
                        {
                            UserId = user.UserId,
                            ClientId = clientId
                        };

                        _session.UnitOfWork.RegisterDeleted(remove);
                    }
                    else
                    {
                        var uc = new UserClient
                        {
                            UserId = user.UserId,
                            ClientId = clientId
                        };

                        _session.UnitOfWork.RegisterNew(uc);
                    }
                }

                #endregion
            });
        }

        IOpResult IUserProvider.CreateUserProfile(UserProfileDto dto)
        {
            var result = new OpResult();

            return result.TryCatch(() =>
            {
                var user = MapDtoToUserProfileEntity(dto);
                var clientId = _session.LoggedInUserInformation.ClientId.GetValueOrDefault(0);

                #region User / UserSession / UserPermissions / UserPin / UserClient (if applicable)

                user.Session = new UserSession
                {
                    LastClientId = _session.LoggedInUserInformation.ClientId,
                    LastEmployeeId = dto.EmployeeId,
                    IpAddress = HttpContext.Current.Request.UserHostAddress,
                    LastLogin = DateTime.Now
                };

                user.Permissions = new UserPermissions
                {
                    IsEmployeeNavigatorAdmin = dto.IsEmployeeNavigatorAdmin
                };

                user.UserPin = new UserPin
                {
                    Pin = dto.UserPin,
                    ClientId = clientId
                };

                if (user.UserTypeId == UserType.CompanyAdmin)
                {
                    user.UserClientSettings.Add(new UserClient
                    {
                        ClientId = clientId,
                        IsClientAdmin = true
                    });
                }

                _session.UnitOfWork.RegisterNew(user);

                #endregion

                #region AuthUser

                _session.UnitOfWork.RegisterPostCommitAction(() =>
                {
                    dto.UserId = user.UserId;
                    var au = MapDtoToInsertUpdateUserDto(dto);

                    au.AuthUserId = _dsUser.InsertUser(au);

                    user.AuthUserId = au.AuthUserId;

                    _session.UnitOfWork.RegisterModified(user, new PropertyList<User>().Include(x => x.AuthUserId));
                    _session.UnitOfWork.Commit();
                });

                #endregion
            });
        }

        IOpResult<IEnumerable<UserClientAccessDto>> IUserProvider.SaveCompanyAdminAccessForUser(int userId, IEnumerable<UserClientAccessDto> access)
        {
            var result = new OpResult<IEnumerable<UserClientAccessDto>>();
            _session.CanPerformAction(SystemActionType.SystemAdministrator).MergeInto(result);

            if (result.HasError)
                return result;

            var userInfo = result.TryGetData(() =>
                _session.UnitOfWork.UserRepository.QueryUsers()
                .ByUserId(userId)
                .FirstOrDefaultAs(x => new
                {
                    x.UserId,
                    x.AuthUserId,
                    x.UserTypeId,
                    clientId = x.Employee == null ? x.Employee.ClientId : default(int?)
                }));

            if (result.CheckForNotFound(userInfo).HasError)
                return result;

            var isClientAdmin = userInfo.UserTypeId == UserType.CompanyAdmin;

            var currentAccess = Self.GetCurrentUserClientAccessSettings(userId, isClientAdmin).MergeInto(result).Data.NullCheckToList();

            if (result.HasError)
                return result;

            var updatedUserClients = new List<UserClient>();
            foreach (var change in access)
            {
                if (change.UserId != userId)
                    continue;

                var current = currentAccess.FirstOrDefault(c => c.ClientId == change.ClientId);

                var entity = new UserClient
                {
                    UserId = change.UserId,
                    ClientId = change.ClientId,
                    IsClientAdmin = false, //always false in the database
                    IsBenefitAdmin = change.IsBenefitAdmin
                };

                if (current == null)
                {
                    if (change.HasAccess)
                    {
                        //new
                        _session.UnitOfWork.RegisterNew(entity);
                        updatedUserClients.Add(entity);
                    }
                }
                else
                {
                    if (!change.HasAccess)
                    {
                        //delete
                        _session.UnitOfWork.RegisterDeleted(entity);
                    }
                    else
                    {
                        if (current.IsBenefitAdmin != change.IsBenefitAdmin)
                        {
                            //modified
                            _session.UnitOfWork.RegisterModified(entity, new PropertyList<UserClient>().Include(x => x.IsBenefitAdmin));
                        }

                        updatedUserClients.Add(entity);
                    }
                }
            }

            foreach (var toDelete in currentAccess.Where(cur => !access.Any(a => a.ClientId == cur.ClientId && a.UserId == cur.UserId)))
            {
                //delete
                var entity = new UserClient { ClientId = toDelete.ClientId, UserId = toDelete.UserId };
                _session.UnitOfWork.RegisterDeleted(entity);

            }

            #region Update AuthClientId (-1 for multi-client or 1+ for single-client)
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
                authClientID = userInfo.clientId ?? -1;
            }

            var dto = new UserSecurityDto()
            {
                AuthUserId = userInfo?.AuthUserId ?? -1,
                UserTypeId = (byte)userInfo.UserTypeId,
                DsUserId1 = userInfo.UserId,
                AuthClientId = authClientID,
                ModifiedBy = _session.LoggedInUserInformation.AuthUserId
            };

            if (dto.AuthUserId > 0)
                _loginProvider.SetUserSecurityInformation(dto).MergeInto(result);

            #endregion

            result.SetDataOnSuccess(access);

            return result;
        }

        #region User Profile - mapping entity methods

        private InsertUpdateUserDto MapDtoToInsertUpdateUserDto(UserProfileDto dto)
        {
            return new InsertUpdateUserDto(_session.LoggedInUserInformation.AuthUserId, _session.LoggedInUserInformation.UserId)
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.Username,
                Password1 = dto.Password1,
                Password2 = dto.Password2,
                Email = dto.Email,
                DsUserType = (int)dto.UserType,
                EmployeeID = dto.EmployeeId.GetValueOrDefault(),
                ClientId = _session.LoggedInUserInformation.ClientId.GetValueOrDefault(),
                ViewEmployees = (int)dto.ViewEmployeesType,
                ViewRates = (int)dto.ViewRatesType,
                EmployeeSelfServiceOnly = dto.HasEssSelfService,
                ViewOnly = dto.IsEssViewOnly,
                ReportingOnly = dto.IsReportingAccessOnly,
                BlockPayrollAccess = dto.BlockPayrollAccess,
                Timeclock = dto.IsTimeclockAppOnly,
                BlockHR = dto.BlockHr,
                ApplicantAdmin = dto.isApplicantTrackingAdmin,
                EditGL = dto.HasGLAccess,
                Timeout = dto.SessionTimeout,
                ViewTaxPackets = dto.HasTaxPacketsAccess,
                MustChangePWD = dto.ForceUserPasswordReset.HasValue 
                    ? dto.ForceUserPasswordReset.Value : default,
                DsUserId = dto.UserId,
                IsLocked = dto.IsAccountEnabled
            };
        }

        private User MapDtoToUserProfileEntity(UserProfileDto dto)
        {
            return new User
            {
                UserId = dto.UserId,
                UserTypeId = dto.UserType,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmployeeId = dto.EmployeeId,
                EmailAddress = dto.Email,
                MustChangePassword = dto.ForceUserPasswordReset.HasValue
                    ? dto.ForceUserPasswordReset.Value
                    : default,
                IsPasswordEnabled = dto.IsAccountEnabled,
                IsUserDisabled = dto.IsUserDisabled,
                TimeoutMinutes = dto.SessionTimeout,
                ViewEmployeePayTypes = dto.ViewEmployeesType,
                ViewEmployeeRateTypes = dto.ViewRatesType,
                IsEmployeeSelfServiceViewOnly = dto.IsEssViewOnly,
                IsHrBlocked = dto.BlockHr,
                IsEmployeeSelfServiceOnly = dto.HasEssSelfService,
                IsEmployeeAccessOnly = dto.HasEmployeeAccess,
                IsReportingOnly = dto.IsReportingAccessOnly,
                IsEditGlEnabled = dto.HasGLAccess,
                IsPayrollAccessBlocked = dto.BlockPayrollAccess,
                CanViewTaxPackets = dto.HasTaxPacketsAccess,
                IsApplicantTrackingAdmin = dto.isApplicantTrackingAdmin,
                IsTimeclockEnabled = dto.HasTimeAndAttAccess,
                TimeclockAppOnly = dto.IsTimeclockAppOnly
            };
        }

        #endregion

        IOpResult<UserTermsAndConditionsDto> IUserProvider.ProcessUsersTermsAndConditionsAcceptance(int userId, bool isAccepted)
        {
            var result = new OpResult<UserTermsAndConditionsDto>();

            var termsAndConditionsId = _session.UnitOfWork.CoreRepository
                .QueryTermsAndConditionsVersions()
                .ExecuteQueryAs(x => new { x.TermsAndConditionsVersionID, x.EffectiveDate })
                .OrderByDescending(x => x.EffectiveDate)
                .Select(x => x.TermsAndConditionsVersionID)
                .FirstOrDefault();

            var current = _session.UnitOfWork.UserRepository.QueryUserTermsAndConditions().ByUserId(userId).ExecuteQueryAs(x => new UserTermsAndConditionsDto
            {
                AcceptDate = x.AcceptDate,
                TermsAndConditionsVersionId = x.TermsAndConditionsVersionId,
                UserAccepted = x.UserAccepted,
                UserId = x.UserId,
                UserTermsAndConditionsID = x.UserTermsAndConditionsID
            })
            .FirstOrDefault();

            if(current == null)
            {
                current = new UserTermsAndConditionsDto
                {
                    TermsAndConditionsVersionId = termsAndConditionsId,
                    UserId = _session.LoggedInUserInformation.UserId
                };
            }

            var userTermsToSave = new UserTermsAndConditions
            {
                AcceptDate = current.AcceptDate,
                UserAccepted = current.UserAccepted,
                TermsAndConditionsVersionId = current.TermsAndConditionsVersionId,
                UserId = current.UserId,
                UserTermsAndConditionsID = current.UserTermsAndConditionsID
            };

            if (userTermsToSave.UserTermsAndConditionsID == 0)
            {
                _session.UnitOfWork.RegisterNew(userTermsToSave);
                _session.UnitOfWork.RegisterPostCommitAction(() =>
                {
                    current.UserTermsAndConditionsID = userTermsToSave.UserTermsAndConditionsID;
                });
            } else
            {
                var propsToSave = new PropertyList<UserTermsAndConditions>();

                if(userTermsToSave.UserAccepted != isAccepted)
                {
                    userTermsToSave.UserAccepted = isAccepted;
                    propsToSave.Include(x => x.UserAccepted);
                }

                if(isAccepted && userTermsToSave.AcceptDate == null)
                {
                    userTermsToSave.AcceptDate = DateTime.Now;
                    propsToSave.Include(x => x.AcceptDate);
                }

                if (!isAccepted && userTermsToSave.AcceptDate != null)
                {
                    userTermsToSave.AcceptDate = null;
                    propsToSave.Include(x => x.AcceptDate);
                }

                _session.UnitOfWork.RegisterModified(userTermsToSave, propsToSave);
                _session.UnitOfWork.RegisterPostCommitAction(() =>
                {
                    current.UserAccepted = userTermsToSave.UserAccepted;
                    current.AcceptDate = userTermsToSave.AcceptDate;
                });
            }

            _session.UnitOfWork.Commit().MergeInto(result);
            result.SetDataOnSuccess(current);

            return result;
        }

        IOpResult IUserProvider.DeleteCompanyAdminAccess(int userId)
        {
            var result = new OpResult();

            _session.CanPerformAction(SystemActionType.SystemAdministrator).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(_session.LoggedInUserInformation.ClientId.GetValueOrDefault(0)).MergeInto(result);

            if (result.HasError) return result;

            var currentAccess = Self.GetCurrentUserClientAccessSettings(userId, true)
                .MergeInto(result)
                .Data;

            if (currentAccess == null) return result;

            var entities = currentAccess.Select(x => new UserClient
            {
                ClientId = x.ClientId,
                UserId = x.UserId
            }).ToList();

            entities.ForEach((e) =>
            {
                _session.UnitOfWork.RegisterDeleted(e);
            });

            return result;
        }

        IOpResult IUserProvider.DeleteCompanyAdminUserPin(int userId)
        {
            var result = new OpResult();

            _session.CanPerformAction(UserManagerActionType.UserReadWrite).MergeInto(result);

            if (result.HasError) return result;

            return result.TryCatch(() =>
            {
                var userPinId = _session.UnitOfWork.UserRepository
                    .QueryUserPins()
                    .ByUserId(userId)
                    .ExecuteQueryAs(x => new
                    {
                        x.UserPinId
                    })
                    .FirstOrDefault()
                    ?.UserPinId;

                if (userPinId.HasValue)
                {
                    var e = new UserPin
                    {
                        UserPinId = userPinId.Value
                    };

                    _session.UnitOfWork.RegisterDeleted(e);
                }
            });
        }
    }
}