using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Dominion.Authentication.Api.Internal.AttributeRules;
using Dominion.Authentication.Dto;
using Dominion.Authentication.Dto.Enums;
using Dominion.Authentication.Interface.Api;
using Dominion.Authentication.zTest.Api.Internal;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Services.Api.Auth;
using Dominion.Core.Services.Api.DataServicesInjectors;
using Dominion.Core.Services.Internal.Providers;
using Dominion.Utility.Configs;
using Dominion.Utility.Constants;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Security;

namespace Dominion.Core.Services.Api
{
    using System.IO;
    using System.Transactions;
    using System.Web;

    using Dominion.Core.Dto.Client;
    using Dominion.Core.Dto.Common;
    using Dominion.Core.Dto.TimeCard;
    using Dominion.Core.Dto.Core;
    using Dominion.Core.Dto.User;
    using Dominion.Core.EF.Interfaces;
    using Dominion.Core.Services.Interfaces;
    using Dominion.Core.Services.Mapping;
    using Dominion.Domain.Entities.Core;
    using Dominion.Domain.Entities.User;
    using Dominion.Utility.Containers;
    using Dominion.Utility.Mapping;
    using Dominion.Utility.Msg.Identifiers;
    using Dominion.Utility.Msg.Specific;
    using Dominion.Utility.OpResult;
    using Dominion.Utility.Security.Msg;
    using Dominion.Utility.Web;
    using Dominion.Utility.Query;
    using Dominion.Core.Services.Api.Auth.DB.Qry.Sprocs;
    using FastMapper;
    using Dominion.Authentication.Interface.Api.Providers;

    /// <summary>
    /// Defines a service that is/will be able to handle actions related to users.
    /// Currently, most of the service-related logic exists in the <see cref="UserManager"/> class.
    /// The goal will be to migrate that logic into this class to be consistent with all of the new logic that is being written.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IBusinessApiSession        _session;
        private readonly ILoginService              _loginService;
        private readonly ISecurityService           _securityService;
        private readonly IUserProvider              _userProvider;
        private readonly IContactProvider           _contactProvider;
        private readonly IDsDataServicesUserService _objUserService;
        private readonly IDsSuperAdminEmailer       _objSuperAdminEmailer;
        private readonly IUserManager               _userManager;
        private readonly IAccountFeatureService     _accountFeatureService;
        private readonly ILoginProvider _loginProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="loginService"></param>
        /// <param name="userProvider"></param>
        public UserService(IBusinessApiSession session, ILoginService loginService, ISecurityService securityService, IUserProvider userProvider,
            IContactProvider contactProvider, IDsDataServicesUserService objUserService, IDsSuperAdminEmailer objSuperAdminEmailer, IUserManager userManager,
            IAccountFeatureService accountFeatureService, ILoginProvider loginProvider)
        {
            _session              = session;
            _loginService         = loginService;
            _securityService      = securityService;
            _userProvider         = userProvider;
            _contactProvider      = contactProvider;
            _loginService.IP      = CommonConstants.NO_IP_AVAILABLE;
            _objUserService       = objUserService;
            _objSuperAdminEmailer = objSuperAdminEmailer;
            _userManager          = userManager;
            _accountFeatureService= accountFeatureService;
            _loginProvider = loginProvider;
            Self                  = this;
        }

        internal IUserService Self { set; get; }

        /// <summary>
        /// Updates the last client ID for the current user.
        /// </summary>
        /// <param name="dto">The request object that contains the new client ID for the user. Effectively updates the client that the user is viewing.</param>
        /// <returns></returns>
        IOpResult<UpdateUserLastClientIdDto> IUserService.UpdateLastClientId(UpdateUserLastClientIdDto dto)
        {
            var result = new OpResult<UpdateUserLastClientIdDto>();

            _session.CanPerformAction(UserManagerActionType.UpdateLastClientId).MergeInto(result);

            if (result.Success)
            {
                if (dto == null)
                {
                    result.AddMessage(new NullReferenceMsg<UpdateUserLastClientIdDto>("Body"));
                    result.SetToFail();
                }
                if (result.Success)
                {
                    result.TryCatch(
                        () =>
                        {
                            var userId = this._session.LoggedInUserInformation.UserId;
                            this._session.UnitOfWork.NoChangeTracking();
                            var user = this._session.UnitOfWork.UserRepository.QueryUsers().ByUserId(userId)
                                .ExecuteQueryAs(u => new UserDto()
                                {
                                    UserId = u.UserId,
                                    UserTypeId = u.UserTypeId,
                                    LastClientId = u.Session != null && u.Session.LastClientId.HasValue ? u.Session.LastClientId.Value : 0
                                })
                                .Single();

                            //if user already set to this client don't have to update
                            if(dto.ClientId == user.LastClientId)
                            {
                                result.SetDataOnSuccess(dto);
                                return;
                            }

                            var isSystemAdmin = user.UserTypeId == UserType.SystemAdmin;
                            var query = this._session.UnitOfWork.ClientRepository.QueryClients()
                                    .ByClientId(dto.ClientId);
                            if (!isSystemAdmin)
                            {
                                query.ByUserId(userId);
                            }

                            var client = query
                                    .ExecuteQueryAs(c => new ClientDto() { ClientId = c.ClientId })
                                    .SingleOrDefault();

                            if (client != null)
                            {
                                var newUser = new User()
                                {
                                    UserId = user.UserId,
                                    LastModifiedByUserId = user.UserId
                                };

                                var newUserSession = new UserSession
                                {
                                    UserId = user.UserId,
                                    LastClientId = client.ClientId,
                                    LastEmployeeId = null
                                };

                                _session.UnitOfWork.RegisterModified(
                                    newUser,
                                    new PropertyList<User>()
                                        .Include(u => u.LastModifiedByUserId));

                                _session.UnitOfWork.RegisterModified(newUserSession, new PropertyList<UserSession>().Include(x => x.LastClientId).Include(x => x.LastEmployeeId));

                                _session.UnitOfWork.Commit().MergeInto(result);

                                if (result.Success)
                                {
                                    result.Data = new UpdateUserLastClientIdDto()
                                    {
                                        ClientId = newUserSession.LastClientId.Value
                                    };
                                }
                            }
                        });
                }
            }

            return result;
        }

        IOpResult<UserInfoDto> IUserService.ChangeSelectedClient(UpdateUserLastClientIdDto dto)
        {
            var result = new OpResult<UserInfoDto>();

            _session.CanPerformAction(UserManagerActionType.UpdateLastClientId).MergeInto(result);

            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(dto.ClientId).MergeInto(result);

            if (result.HasError) return result;

            if (dto == null)
            {
                result.AddMessage(new NullReferenceMsg<UpdateUserLastClientIdDto>("Body"));
                return result.SetToFail();
            }

            var userId = this._session.LoggedInUserInformation.UserId;

            _session.UnitOfWork.NoChangeTracking();

            var user = this._session.UnitOfWork.UserRepository
                .QueryUsers()
                .ByUserId(userId)
                .ExecuteQueryAs(u => new UserInfoDto()
                {
                    UserId = u.UserId,
                    UserTypeId = u.UserTypeId,
                    LastClientId = u.Session != null ? u.Session.LastClientId ?? 0 : 0,
                    LastClientCode = u.Session != null ? u.Session.LastClient.ClientCode : default,
                    LastClientName = u.Session != null ? u.Session.LastClient.ClientName : default
                })
                .FirstOrDefault();

            if (user == null) return result.SetToFail();

            //if user already set to this client don't have to update
            if (dto.ClientId == user.LastClientId)
            {
                return result.SetDataOnSuccess(new UserInfoDto
                {
                    LastClientId = user.LastClientId,
                    LastClientCode = user.LastClientCode,
                    LastClientName = user.LastClientName,
                });
            }

            var isSystemAdmin = user.UserTypeId == UserType.SystemAdmin;
            var query = this._session.UnitOfWork.ClientRepository
                .QueryClients()
                .ByClientId(dto.ClientId);

            if (!isSystemAdmin)
            {
                query.ByUserId(userId);
            }

            var client = query
                .ExecuteQueryAs(c => new ClientDto()
                {
                    ClientId = c.ClientId,
                    ClientName = c.ClientName,
                    ClientCode = c.ClientCode
                })
                .SingleOrDefault();

            if (client == null) return result.SetToFail();

            var newUser = new User()
            {
                UserId = user.UserId,
                LastModifiedByUserId = user.UserId
            };

            var newUserSession = new UserSession
            {
                UserId = user.UserId,
                LastClientId = client.ClientId,
                LastEmployeeId = null
            };

            _session.UnitOfWork.RegisterModified(
                newUser,
                new PropertyList<User>()
                    .Include(u => u.LastModifiedByUserId));

            _session.UnitOfWork.RegisterModified(
                newUserSession,
                new PropertyList<UserSession>()
                    .Include(x => x.LastClientId)
                    .Include(x => x.LastEmployeeId));

            _session.UnitOfWork.Commit().MergeInto(result);

            if (result.HasError) return result.SetToFail();

            return result.SetDataOnSuccess(new UserInfoDto
            {
                LastClientId = client.ClientId,
                LastClientName = client.ClientName,
                LastClientCode = client.ClientCode
            });
        }

        IOpResult<SiteConfigurationDto> IUserService.GetSiteConfiguration(byte id)
        {
            var r = new OpResult<SiteConfigurationDto>();

            r.Data = _loginService.GetSiteConfigurations((byte)id).Data?.FirstOrDefault() ??
                     new SiteConfigurationDto()
                     {
                         CoAdminRootUrl = ConfigValues.MainRedirectRootUrl,
                         DsRootUrl = ConfigValues.MainRedirectRootUrl,
                         EssRootUrl = ConfigValues.MainRedirectRootUrl,
                         RootUrl = ConfigValues.MainRedirectRootUrl,
                         SiteConfigurationId = id,
                     };

            return r;
        }

        IOpResult<LoginResultsDto> IUserService.GetMfaRequirements(int authUserId)
        {
            var r = new OpResult<LoginResultsDto>();

            r.Data = _loginService.GetMfaRequirements(authUserId).MergeInto(r).Data;

            return r;
        }

        IOpResult<ProfileDataDto> IUserService.GetMfaProfileData(int authUserId)
        {
            var r = new OpResult<ProfileDataDto>();
            r.Data = new ProfileDataDto();

            var data = _loginService
                .GetUserProfileAttribute(authUserId, UserProfileAttributeTypeId.RecoveryEmail)
                .MergeInto(r).Data;

            if (r.Success)
                r.Data.AuthRecoveryEmail = data?.Data;

            return r;
        }

        IOpResult IUserService.AuthAppEmailCheck(ProfileDataDto dto)
        {
            var r = new OpResult();

            //check to see if logged in user is the user we're modifying
            if (_session.LoggedInUserInformation.AuthUserId == dto.AuthUserId)
            {
                var attr = _loginService
                    .GetUserProfileAttribute(dto.AuthUserId, UserProfileAttributeTypeId.RecoveryEmail)
                    .MergeInto(r)
                    .Data;

                //if the emails are different update the app email
                if (r.Success && attr?.Data != dto.AppRecoveryEmail)
                {
                    var newUser = new User()
                    {
                        UserId = dto.DsUserId,
                        EmailAddress = attr?.Data,
                        LastModifiedByUserId = _session.LoggedInUserInformation.UserId,
                    };

                    _session.UnitOfWork.RegisterModified(
                        newUser,
                        new PropertyList<User>()
                            .Include(u => u.EmailAddress)
                            .Include(u => u.LastModifiedByUserId));

                    _session.UnitOfWork.Commit().MergeInto(r);
                }

            }

            return r;
        }
        
        IOpResult<IEnumerable<UserClientAccessDto>> IUserService.GetCompanyAdminAccessForUser(int userId)
        {
            var result = new OpResult<IEnumerable<UserClientAccessDto>>();

            _session.CanPerformAction(SystemActionType.SystemAdministrator).MergeInto(result);

            if(result.HasError)
                return result;

            var userInfo = result.TryGetData(() => _session.UnitOfWork.UserRepository.QueryUsers()
                .ByUserId(userId)
                .ExecuteQueryAs(x => new
                {
                    x.UserId,
                    x.UserTypeId
                })
                .FirstOrDefault());

            ;
            if(result.CheckForNotFound(userInfo).HasError)
                return result;

            var isClientAdmin = userInfo.UserTypeId == UserType.CompanyAdmin;

            var userClients = _userProvider.GetCurrentUserClientAccessSettings(userId, isClientAdmin).MergeInto(result).Data.NullCheckToList();

            if(result.HasError)
                return result;

            //only if a company admin, include other clients in the organization, but mark them as no access
            if(isClientAdmin)
            {
                var accessibleOrganizations = result.TryGetData(() => _session.UnitOfWork.ClientRepository.ClientRelationQuery()
                    .ByUserHasAccessToAtLeaseOneClientInRelation(userId)
                    .ExecuteQueryAs(x => new
                    {
                        Clients = x.Clients.Select(c => new
                        {
                            c.ClientId,
                            c.ClientName
                        })
                    })
                    .ToList());

                if(result.HasError)
                    return result;

                foreach(var client in accessibleOrganizations.SelectMany(x => x.Clients))
                {
                    var currentAccess = userClients.FirstOrDefault(x => x.ClientId == client.ClientId);
                    if (currentAccess == null)
                    {
                        currentAccess = new UserClientAccessDto
                        {
                            UserId         = userId,
                            ClientId       = client.ClientId,
                            ClientName     = client.ClientName,
                            HasAccess      = false,
                            IsBenefitAdmin = false,
                            IsClientAdmin  = false
                        };

                        userClients.Add(currentAccess);
                    }
                }
            }

            result.SetDataOnSuccess(userClients);

            return result;
        }

        IOpResult IUserService.DeleteCompanyAdminAccess(int userId)
        {
            var result = new OpResult();

            _session.CanPerformAction(SystemActionType.SystemAdministrator).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(_session.LoggedInUserInformation.ClientId.GetValueOrDefault(0)).MergeInto(result);

            if (result.HasError) return result;

            var currentAccess = _userProvider.GetCurrentUserClientAccessSettings(userId, true)
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

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult<IEnumerable<UserClientAccessDto>> IUserService.SaveCompanyAdminAccessForUser(int userId, IEnumerable<UserClientAccessDto> access)
        {
            var result = new OpResult<IEnumerable<UserClientAccessDto>>();
            _session.CanPerformAction(SystemActionType.SystemAdministrator).MergeInto(result);

            if(result.HasError)
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

            if(result.CheckForNotFound(userInfo).HasError)
                return result;

            var isClientAdmin = userInfo.UserTypeId == UserType.CompanyAdmin;

            var currentAccess = _userProvider.GetCurrentUserClientAccessSettings(userId, isClientAdmin).MergeInto(result).Data.NullCheckToList();

            if(result.HasError)
                return result;

            var updatedUserClients = new List<UserClient>();
            foreach(var change in access)
            {
                if(change.UserId != userId)
                    continue;

                var current = currentAccess.FirstOrDefault(c => c.ClientId == change.ClientId);

                var entity = new UserClient
                {
                    UserId         = change.UserId,
                    ClientId       = change.ClientId,
                    IsClientAdmin  = false, //always false in the database
                    IsBenefitAdmin = change.IsBenefitAdmin
                };

                if(current == null)
                {
                    if(change.HasAccess)
                    {
                        //new
                        _session.UnitOfWork.RegisterNew(entity);
                        updatedUserClients.Add(entity);
                    }
                }
                else
                {
                    if(!change.HasAccess)
                    {
                        //delete
                        _session.UnitOfWork.RegisterDeleted(entity);
                    }
                    else
                    {
                        if(current.IsBenefitAdmin != change.IsBenefitAdmin)
                        {
                            //modified
                            _session.UnitOfWork.RegisterModified(entity, new PropertyList<UserClient>().Include(x => x.IsBenefitAdmin));
                        }

                        updatedUserClients.Add(entity);
                    }
                }
            }

            foreach(var toDelete in currentAccess.Where(cur => !access.Any(a => a.ClientId == cur.ClientId && a.UserId == cur.UserId)))
            {
                //delete
                var entity = new UserClient { ClientId = toDelete.ClientId, UserId = toDelete.UserId };
                _session.UnitOfWork.RegisterDeleted(entity);

            }

            _session.UnitOfWork.Commit().MergeInto(result);

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
                //update the clientId
                _loginService.SetUserSecurityInformation(dto);
            #endregion

            result.SetDataOnSuccess(access);

            return result;
        }

        IOpResult<bool> IUserService.CheckUsername(string username)
        {
            var result =  _loginService.CheckUserName(username);
            return new OpResult<bool>(result.Success);
        }

        IOpResult<UserSettingsDto> IUserService.GetUserProfileSettings(int clientId, int userId)
        {
            var result = new OpResult<UserSettingsDto>();

            var changeRequestRequired = _accountFeatureService.GetClientAccountFeature(Core.Dto.Misc.AccountFeatureEnum.EmployeeChangeRequests, clientId).Data?.IsEnabled == true;

            return result.TrySetData(() => _session.UnitOfWork.UserRepository
                .QueryUsers()
                .ByLastClientId(clientId)
                .ByUserId(userId)
                .ExecuteQueryAs(x => new UserSettingsDto
                {
                    UserId = x.UserId,
                    EmployeeId = x.EmployeeId ?? 0,
                    LastClientId = x.Session != null ? x.Session.LastClientId ?? 0 : 0,
                    EmailAddress = x.EmailAddress,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    ChangeRequestRequired = changeRequestRequired
                })
                .FirstOrDefault());
        }

        UserSettingsInputDto IUserService.UpdateUserProfileSettingsEmailWithEmail(string email, int employeeId)
        {

            var user = _session.UnitOfWork.UserRepository.GetUserByEmployeeId(employeeId);

            var userSettingsInputDto = new UserSettingsInputDto
            {
                ChangeRequestRequired = false,
                Email = email
            };

            return Self.UpdateUserProfileSettingsEmail(userSettingsInputDto, (user.Session?.LastClientId ?? 0), user.UserId).Data;
        }

        IOpResult<UserSettingsInputDto> IUserService.UpdateUserProfileSettingsEmail(UserSettingsInputDto dto, int clientId, int userId)
        {
            var opResult = new OpResult<UserSettingsInputDto>();
            opResult.Data = dto;
            _session.CanPerformAction(UserManagerActionType.BasicInfoUpdate).MergeInto(opResult);
            if (!opResult.Success)
            {
                opResult.SetToFail();
                opResult.AddMessage(new ActionNotAllowedMsg(UserManagerActionType.BasicInfoUpdate));
            }
            else
            {
                var user = _session.UnitOfWork.UserRepository.QueryUsers().ByLastClientId(clientId).ByUserId(userId).FirstOrDefault();
                if (dto.ChangeRequestRequired)
                {
                    opResult.Data.Email = user.EmailAddress;
                    return opResult;
                }
                user.EmailAddress = dto.Email;
                user.LastModifiedByUserId = _session.LoggedInUserInformation.UserId;
                _session.UnitOfWork.RegisterModified(user);

                var employee = _session.UnitOfWork.EmployeeRepository.QueryEmployees().ByEmployeeId((int)user.EmployeeId).FirstOrDefault();
                employee.EmailAddress = dto.Email;
                _session.SetModifiedProperties(employee);
                _session.UnitOfWork.RegisterModified(employee);

                var authUserId = (int) user.AuthUserId;
                _loginService.UpdateRecoveryEmail(authUserId, dto.Email).MergeInto(opResult);

                _session.UnitOfWork.Commit().MergeInto(opResult);                            
            }
            return opResult;
        }

        
        IOpResult<UserDto> IUserService.GetUser(int? userId)
        {
            var result = new OpResult<UserDto>();

            var user = _session.UnitOfWork.UserRepository.QueryUsers()
                    .ByUserId(userId ?? _session.LoggedInUserInformation.UserId)
                    .ExecuteQueryAs(x => new UserDto
                    {
                        ViewEmployeeRateTypes = x.ViewEmployeeRateTypes,
                        EmployeeId = x.EmployeeId ?? 0,
                        UserTypeId = x.UserTypeId
                    })
                    .FirstOrDefault();
            // new FastMapperDs<User, UserDto>()
            result.Data = user;

            return result;
        }

        IOpResult<IEnumerable<UserProfileBaseDto>> IUserService.GetUserProfilesByClientId(int clientId, bool? includeTerminated = null)
        {
            var result = new OpResult<IEnumerable<UserProfileBaseDto>>();

            _session.CanPerformAction(UserManagerActionType.GetUsers).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() =>
            {
                // spGetUserList sproc in SQL takes a parameter called "isActive" which indicates whether it should return just 
                // active users. This value is equal to the inverse of includeTerminated and is why you see us invert the value here.
                var isActive = includeTerminated.HasValue ? !includeTerminated.Value : true;
                return _objUserService.GetUserList(_session.LoggedInUserInformation.UserId, clientId, isActive);
            });
        }

        #region User Profile Actions

        IOpResult<UserProfileDto> IUserService.SaveUserProfile(int clientId, int userId, UserProfileDto profile)
        {
            var result = new OpResult<UserProfileDto>();

            _session.CanPerformAction(UserManagerActionType.UserReadWrite).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);

            if (result.HasError) return result;

            _userProvider.SaveUserProfile(profile).MergeInto(result);

            return result;
        }

        IOpResult IUserService.SetAccountEnabledState(int clientId, int userId, bool isAccountEnabled)
        {
            var result = new OpResult();

            _session.CanPerformAction(UserManagerActionType.UserReadWrite).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);

            if (result.HasError) return result;

            return result.TryCatch(() =>
            {
                var req = new User
                {
                    UserId = userId,
                    IsPasswordEnabled = isAccountEnabled
                };

                var props = new PropertyList<User>().Include(x => x.IsPasswordEnabled);

                _session.UnitOfWork.RegisterModified(req, props);
                _session.UnitOfWork.Commit().MergeInto(result);
            });
        }

        IOpResult IUserService.SetUserDisabledState(int clientId, int userId, bool isUserDisabled)
        {
            var result = new OpResult();

            _session.CanPerformAction(UserManagerActionType.UserReadWrite).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);

            if (result.HasError) return result;

            return result.TryCatch(() =>
            {
                var curr = _session.UnitOfWork.UserRepository
                    .QueryUsers()
                    .ByUserId(userId)
                    .ExecuteQueryAs(x => new
                    {
                        x.UserId,
                        x.AuthUserId
                    })
                    .FirstOrDefault();

                var req = new User
                {
                    UserId = userId,
                    IsUserDisabled = isUserDisabled
                };

                var props = new PropertyList<User>().Include(x => x.IsUserDisabled);

                _session.UnitOfWork.RegisterModified(req, props);

                if (curr != null && curr.AuthUserId.HasValue)
                {
                    var authUser = new UserSecurityDto
                    {
                        AuthUserId = curr.AuthUserId.Value,
                        IsLocked = isUserDisabled,
                        AuthClientId = clientId
                    };

                    _loginService.SetUserSecurityInformation(authUser).MergeInto(result);
                } 

                _session.UnitOfWork.Commit().MergeInto(result);
            });
        }

        #endregion

        IOpResult<UserProfileDto> IUserService.GetUserSecurityInformation(int userId)
        {
            var result = new OpResult<UserProfileDto>();

            _session.CanPerformAction(UserManagerActionType.GetUsers).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(_session.LoggedInUserInformation.ClientId.GetValueOrDefault(0))
                .MergeInto(result);

            if (_session.LoggedInUserInformation.IsSupervisor && _session.LoggedInUserInformation.UserId != userId)
            {
                var ee = _session.UnitOfWork.UserRepository
                    .QueryUsers()
                    .ByUserId(userId)
                    .ExecuteQueryAs(x => new
                    {
                        x.EmployeeId
                    })
                    .FirstOrDefault();

                if (ee == null)
                    return result.SetToFail(MessageConstants.RESOUCE_AUTHORIZATION_ERROR);

                var accessibleUsers = _objUserService.GetUserSupervisorSecurityListByUserIDClientIDInDataModel(
                    _session.LoggedInUserInformation.UserId, _session.LoggedInUserInformation.ClientId.GetValueOrDefault());

                var noAccess = !accessibleUsers.List.Any(x => x.ForeignKeyID == ee.EmployeeId && x.IsAllowed);

                if (noAccess)
                    result.SetToFail(MessageConstants.RESOUCE_AUTHORIZATION_ERROR);
            }
            else
            {
                _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.User, userId).MergeInto(result);
            }

            if (result.HasError) return result;

            result.TrySetData(() =>
            {
                var user = _session.UnitOfWork.UserRepository
                    .QueryUsers()
                    .ByUserId(userId)
                    .ExecuteQueryAs(x => new UserProfileDto
                    {
                        UserId = x.UserId,
                        AuthUserId = x.AuthUserId,
                        UserType = x.UserTypeId,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Username = x.UserName,
                        Email = x.EmailAddress,
                        EmployeeId = x.EmployeeId,
                        Employee = x.Employee != null ? new EmployeeBasicDto()
                        {
                            EmployeeId = x.Employee.EmployeeId,
                            ClientId = x.Employee.ClientId,
                            EmployeeNumber = x.Employee.EmployeeNumber,
                            FirstName = x.Employee.FirstName,
                            LastName = x.Employee.LastName,
                            EmployeeStatusType = x.Employee.EmployeePayInfo.Any()
                                ? x.Employee.EmployeePayInfo.FirstOrDefault().EmployeeStatusId
                                : default,
                        } : default,
                        EmployeeStatusType = (x.Employee != null) && x.Employee.EmployeePayInfo.Any()
                            ? x.Employee.EmployeePayInfo.FirstOrDefault().EmployeeStatusId
                            : default,
                        ForceUserPasswordReset = x.MustChangePassword,
                        BlockHr = x.IsHrBlocked,
                        BlockPayrollAccess = x.IsPayrollAccessBlocked,
                        HasEmployeeAccess = x.IsEmployeeAccessOnly.HasValue ? x.IsEmployeeAccessOnly.Value : false,
                        HasEssSelfService = x.IsEmployeeSelfServiceOnly,
                        HasGLAccess = x.IsEditGlEnabled,
                        HasTaxPacketsAccess = x.CanViewTaxPackets,
                        HasTimeAndAttAccess = x.IsTimeclockEnabled,
                        isApplicantTrackingAdmin = x.IsApplicantTrackingAdmin,
                        IsEssViewOnly = x.IsEmployeeSelfServiceViewOnly,
                        IsReportingAccessOnly = x.IsReportingOnly,
                        SessionTimeout = x.TimeoutMinutes,
                        ViewEmployeesType = x.ViewEmployeePayTypes,
                        ViewRatesType = x.ViewEmployeeRateTypes,
                        UserPin = x.UserPin != null ? x.UserPin.Pin : null,
                        IsEmployeeNavigatorAdmin =
                            x.Permissions != null && x.Permissions.IsEmployeeNavigatorAdmin,
                        IsTimeclockAppOnly = x.TimeclockAppOnly,
                        HasTempAccess = x.TempEnableFromDate.HasValue || x.TempEnableToDate.HasValue,
                        FromDate = x.TempEnableFromDate,
                        ToDate = x.TempEnableToDate
                    })
                    .FirstOrDefault();

                if (user == null)
                {
                    result.SetToFail("Did not find specified user.");
                    return null;
                }

                UserSecurityDto authUser;

                if (user.AuthUserId.HasValue)
                {
                    authUser = _loginService.GetUserSecurityInformation(user.AuthUserId.Value).MergeInto(result).Data;
                }
                else
                {
                    authUser = _loginService.GetUserSecurityInformationByDsUserId(user.UserId).MergeInto(result).Data;
                }

                user.IsUserDisabled = authUser.IsLocked;
                user.IsAccountEnabled = !authUser.StrikeOut;

                return user;
            });

            return result;
        }

        IOpResult<InsertUpdateUserDto> IUserService.AddNewUser(InsertUpdateUserDto dto)
        {
            var result = new OpResult<InsertUpdateUserDto>();

            _loginService.CheckUserName(dto.UserName).MergeInto(result);

            if (result.Success)
            {
                #region "INSERT AUTH USER DATA"

                //a record of the various user ids and types
                var newUserInfo = new AuthUserHelper();
                var authHelper  = new AuthHelper(_loginService, _securityService, null);
                //this is called upon after this method is finished
                dto.AddedUserInfo = newUserInfo;

                //record and set the auth user type id
                newUserInfo.DsToAuthUserType(dto.DsUserType);

                //dto we send to API to add new user
                var xxx = new CreateNewUserDto
                {
                    ClientId          = dto.ClientId,
                    AuthUserTypeId    = newUserInfo.AuthUserTypeId, //always employee for self registration
                    SiteId            = ConfigValues.SiteConfigurationID,
                    EmployeeId        = dto.EmployeeID,
                    UserName          = dto.UserName,
                    Password1         = dto.Password1,
                    Password2         = dto.Password2,
                    EmailAddress      = dto.Email,
                    AllUnSafePassword = dto.AllowUnSafePass,
                    AccessStatusId    = dto.AccessStatusId
                };

                //add the user to the AUTH server
                var authNewUserResult = authHelper.AddAuthUser(xxx).MergeInto(result);

                //update security attributes for the user 
                if (result.Success)
                {
                    //lowFix: auth: this could be added to the add new auth user API call since this is an entirely separate call; we can combine the two under one commit ?? 
                    //update selected attributes values 
                    dto.AuthUserId = authNewUserResult.Data;
                }

                #endregion

                if (result.Success && authNewUserResult.Data.GetValueOrDefault() > 0)
                {
                    if (newUserInfo.AuthUserTypeId == AuthUserTypeId.SysAdmin)
                    {
                        //var t_emailAdmins = new NewSystemAdminEmail();
                        var t = new System.Threading.Thread(_objSuperAdminEmailer.EmailSuperAdmins);

                        _objSuperAdminEmailer.nsa_CreatedByUserID = dto.AuthModifiedBy ?? CommonConstants.SYS_DEFAULT_AUTH_USER_ID;
                        _objSuperAdminEmailer.nsa_UserFirstName   = dto.FirstName;
                        _objSuperAdminEmailer.nsa_UserLastName    = dto.LastName;

                        //Start Thread
                        t.IsBackground = true;
                        t.Start();
                    }

                    //record new auth user id and the user's type
                    newUserInfo.AuthUserId = authNewUserResult.Data;

                    #region "INSERT THE USER DIRECTLY INTO CURRENT ENVIRONMENT"

                    //insert the user into the app data and get the ds user id
                    newUserInfo.DsUserId1 = _objUserService.InsertUser(dto);
                    dto.DsUserId = newUserInfo.DsUserId1;

                    //validate the user was added to the current environment's data(either live or 2live)
                    authHelper.WasUserProperlyAddedToAppEnvironment(newUserInfo.DsUserId1).MergeInto(result);

                    #endregion


                    var sysAdminCheck = (result.Success && (newUserInfo.AuthUserTypeId == AuthUserTypeId.SysAdmin) && (ConfigValues.ConnectionStringAH != null));
                    //ConfigValues.ActivateRedirection AndAlso 'review: auth: this was part of the evaluation; do we really need it ?


                    //only do this if the AH connection string is in the app config and the user is a SYS ADMIN
                    //If sysAdminCheck Then
                    if (sysAdminCheck) {

                        #region "ADD SYS ADMIN TO THE AH DOMSQL 2LIVE DATA"

                        //Insert the user into the AH environment (2LIVE & DOMSQL)
                        dto.ConnStr = ConfigValues.ConnectionStringAH;

                        //review: jay: this should be removed before release; or for sure set to false in the app settings.
                        //review: jay: this should be removed before release; or for sure set to false in the app settings.
                        //review: jay: this should be removed before release; or for sure set to false in the app settings.
                        dto.UserName = (ConfigValues.SimulateMultiServerEnvironment ? dto.UserName + "abc" : dto.UserName);

                        //insert the user
                        newUserInfo.DsUserId2 = _objUserService.InsertUser(dto);
                        //newUserInfo.DsUserId2 = (newUserInfo.DsUserId2 == null) ? newUserInfo.DsUserId2 : 0;

                        //validate the data was added to the AH data
                        authHelper.WasUserProperlyAddedToAppEnvironment(newUserInfo.DsUserId2.GetValueOrDefault(), true).MergeInto(result);

                        #endregion

                    }

                    //update the auth user's ds user id if new ds user record was created
                    if (result.Success && newUserInfo.HasAllIdsBeenSet()) {
                        #region "UPDATE AUTH USER RECORD WITH APP DATA"

                        authHelper.UpdateAuthUserWithDsUserId(newUserInfo.AuthUserId.GetValueOrDefault(),newUserInfo.DsUserId1,newUserInfo.DsUserId2).MergeInto(result);

                        #endregion

                        _securityService.UpdateAuthUserAttributes(dto).MergeInto(result);

                    }


                }

            }

            result.Data = dto;
            return result;
        }

        IOpResult<UserDto> IUserService.GetCurrentUserHRBlockedAndViewOnly()
        {
            var result = new OpResult<UserDto>();

            var user = _session.UnitOfWork.UserRepository.GetUser(_session.LoggedInUserInformation.UserId);

            result.Data = new UserDto
            {
                IsHrBlocked = user.IsHrBlocked,
                IsEmployeeSelfServiceViewOnly = user.IsEmployeeSelfServiceViewOnly,
                ViewEmployeePayTypes = UserViewEmployeePayType.None,
                ViewEmployeeRateTypes = UserViewEmployeePayType.None
            };

            return result;
        }


        #region Authorization
        //TODO: Refactor Authorization Logic into an Authorization Library
        IOpResult<UserChangeDto> IUserService.SetStatusForCompanyAdmin(int userId, int updatedClientId,
            AccessStatusId clientStatusId, DateTime? allowAccessUntil, int modifiedByAuthUserId)
        {
            var opResult = new OpResult<UserChangeDto>();

            opResult.TryCatch(() =>
            {
                //Get a list of clients for a given user
                var result = _userProvider.GetAssociatedClientsForUser(userId).MergeInto(opResult);
                var userClients = result.Data;

                //will need a list of statuses to compare against an ordered list of status.
                var statuses = new List<AccessStatusId>();
                var orderedStatuses = new List<AccessStatusId>()
                {
                    AccessStatusId.Regular,
                    AccessStatusId.ProvisionalAccessWithDate,
                    AccessStatusId.ProvisionalAccess,
                    AccessStatusId.None
                };

                //Initialize aggregate variables with updated client data
                var MaxAllowAccessUntilDate = allowAccessUntil ?? DateTime.MinValue;
                statuses.Add(clientStatusId);

                //build a list of auth statuses for each client
                foreach (var userClient in userClients.Where(uc => uc.ClientId != updatedClientId))
                {
                    //get client status
                    var client = _session.UnitOfWork.ClientRepository.GetClient(userClient.ClientId);
                    var clientStatus = client.ClientStatus ?? ClientStatusType.Terminated;
                    
                    //check for max allowaccessuntildate
                    if (client.AllowAccessUntilDate.HasValue && client.AllowAccessUntilDate.Value > MaxAllowAccessUntilDate)
                    {
                        MaxAllowAccessUntilDate = client.AllowAccessUntilDate.Value;
                    }
                    
                    //convert from clientstatus to authstatus, then add to list
                    var authStatus = ClientProvider.GetClientAuthStatus(clientStatus).MergeInto(opResult).Data;
                    statuses.Add(authStatus);
                }


                if (statuses.Any())
                {
                    //find the most accessible client
                    var mostPrivilegedClientStatus = orderedStatuses.First(status => statuses.Contains(status));

                    //If mostPrivilegedStatus is more restrictive than user's current status:
                    var authUser = _loginService.GetAuthUser(userId).Data;
                    if (authUser?.AuthUserId != null)
                    {
                        var authUserId = (int) authUser.AuthUserId;
                        var isMultiClient = statuses.Count() > 1;
                        //compare the client auth statuses with the user auth statuses. Set the most restrictive.
                        var newStatus = orderedStatuses.Last(status => status == mostPrivilegedClientStatus);

                        //(isMultiClient || newStatus != AccessStatusId.ProvisionalAccessWithDate) omitted
                        //to handle the scenario above we can create an interim solution but it will have additional issues
                        if (newStatus != AccessStatusId.Regular &&
                            newStatus != AccessStatusId.ProvisionalAccessWithDate)
                        {
                            _userProvider.DisableAuthUser(authUserId, modifiedByAuthUserId);
                        }

                        _userProvider.UpdateAllowAccessUntil(authUserId, MaxAllowAccessUntilDate, newStatus,
                            modifiedByAuthUserId);
                    }
                }
            });

            return opResult;
        }

        /// <summary>
        /// sets a User Mfa Settings record in Auth
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isMfaRequired"></param>
        /// <param name="isMfaQuestionsAllowed"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        IOpResult IUserService.SetUserMfaSettings(int userId, bool isMfaRequired, bool isMfaQuestionsAllowed, int modifiedBy)
        {
            var result = new OpResult();

            //add some security checks
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.User, userId).MergeInto(result);
            _session.CanPerformAction(UserManagerActionType.UpdateUserMfa).MergeInto(result);

            //if there is a security error, do not continue
            if (result.HasError)
                return result;

            return result.TryCatch(() =>
            {

                var canDisableMfa = false;
                //extra checks to see if we can turn off MFA in multi client scenario
                if (!isMfaRequired)
                {
                    var opResult = _userProvider.IsAnyClientMfaRequiredForUser(userId);
                    //If there are no associated clients that require Mfa, then you can disable Mfa
                    canDisableMfa = opResult.HasError; //success means it is required and we cannot disable; To disable, we need a failure
                }
 				//Get Auth User to write records to Auth
                var authUser = _loginService.GetAuthUser(userId).Data;
                var authUserId = authUser?.AuthUserId ?? -1;

                //need to populate the data in the record
                var userData = new UserMfaSettingsDto()
                {
                    MfaSettings = new List<UserMfaAttrValueDto>
                    {
                        //this is stored in the data field of the record and is not used by this type of record
                        //if does need to be null however, or the DB update will fail.
                        new UserMfaAttrValueDto()
                        {
                            Required = isMfaRequired || !canDisableMfa, //can only disable when can disable is true and Mfa is not Required
                            Questions = isMfaQuestionsAllowed //This is used to determine if the user is allowed to use Sec Questions
                        }
                    },
                    ModifiedBy = modifiedBy
                };

                //Set UserSecurityAttribute - MFA Override
                _loginService.SetUserMfaOverride(authUserId, userData);
            });
        }

        IOpResult<MfaAttrValueDto> IUserService.GetUserMfaSettings(int userId)
        {
            var result = new OpResult<MfaAttrValueDto>();

            //add some security checks
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.User, userId).MergeInto(result);
            _session.CanPerformAction(UserManagerActionType.UserRead).MergeInto(result);

            if (result.HasError)
                return result;

            return result.TryCatch(() =>
            {

                //Get Auth User to write records to Auth
                var authUser = _loginService.GetAuthUser(userId).Data;
                var authUserTypeId = authUser != null
                    ? (AuthUserTypeId) authUser?.UserTypeId
                    : AuthUserTypeId.UnknownOrNone;
                var authUserId = authUser?.AuthUserId ?? -1;
                var defaultMfaSettings = MfaAttrValueDto.GetDefaultInstance(
                    authUserTypeId.Equals(AuthUserTypeId.SysAdmin));


                var userSecuritySettingsResult = _loginService.GetUserSecuritySettings(authUserId);
                if (userSecuritySettingsResult.HasData)
                {
                    var userSecuritySettings = userSecuritySettingsResult.Data;
                    var userMfaSettings = userSecuritySettings?.FirstOrDefault(x =>
                                                  x.SecurityAttributeType == SecurityAttributeTypeId.UserMfaSettings)
                                              ?.Data
                                              ?.SecurityAttributeDataTo<MfaAttrValueDto>()
                                          ?? defaultMfaSettings;
                    ;

                    result.SetToSuccess();
                    result.SetDataOnSuccess(userMfaSettings);
                }
                else
                {
                    result.SetToSuccess();
                    result.SetDataOnSuccess(defaultMfaSettings);
                }
            });
        }

        IOpResult IUserService.UpdateAllowAccessUntil(int authUserId, AccessStatusId accessStatusId, DateTime? allowAccessUntil, int modifiedByAuthUserId)
        {
            var r = new OpResult();

            r.TryCatch(() =>
            {
                _userProvider.UpdateAllowAccessUntil(authUserId, allowAccessUntil, accessStatusId, modifiedByAuthUserId);
            });

            return r;
        }

        #endregion

        #region USER_INFO_ACTIONS

        /// <summary>
        /// Get the basic user info information for the given user id.
        /// </summary>
        /// <returns>The basic user profile information for the given user id.</returns>
        IOpResult<UserInfoDto> IUserService.GetCurrentUserInfo(int userId)
        {
            var result = new OpResult<UserInfoDto>();

            if (userId == 0)
                userId = _session.LoggedInUserInformation.UserId;//_principal.UserId;

            // check permissions
            _session.CanPerformAction(UserManagerActionType.BasicInfoView).MergeInto(result);

            // a user is only allowed to see their own user info.
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.User, userId).MergeInto(result);

            if (result.Success)
            {
                // get the user info from the repo and copy it to the dto.
                result.TrySetData(_session.UnitOfWork.UserRepository.QueryUsers()
                    .ByUserId(userId)
                    .ExecuteQueryAs(UserMaps.FromUser.ToUserInfoDto.Instance)
                    .FirstOrDefault);

                if (result.HasNoErrorAndHasData)
                {
                    result.Data.EditPermission = _session.CanPerformAction(EmployeeManagerActionType.EmployeeDependentUpdate).Success ? EditPermissions.RequestChange : EditPermissions.ViewOnly;
                }
                else
                {
                    //if here and successful that means we didn't get a user back...so add message accordingly
                    if (result.Success)
                        result.AddMessage(new DataNotFoundMsg<UserInfoDto>(MsgLevels.Error)).SetToFail();
                }

            }

            return result;
        }

        IOpResult<UserTermsAndConditionsDto> IUserService.GetUserTermsAndConditions(int userId)
        {
            var result = new OpResult<UserTermsAndConditionsDto>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.User, userId).MergeInto(result);

            if (result.HasError) return result;

            _userProvider.GetUserTermsAndConditions(userId).MergeAll(result);
            return result;
        }

        IOpResult<TermsAndConditionsVersionsDto> IUserService.GetTermsAndConditionVersions()
        {
            var result = new OpResult<TermsAndConditionsVersionsDto>();

            _userProvider.GetTermsAndConditionsVersions().MergeAll(result);

            return result;
            
        }

        IOpResult<IEnumerable<TermsAndConditionsVersionsDto>> IUserService.GetAllTermsAndConditionVersions()
        {
            var result = new OpResult<IEnumerable<TermsAndConditionsVersionsDto>>();

            _userProvider.GetAllTermsAndConditionsVersions().MergeAll(result);

            return result;
            
        }

        IOpResult<FileStreamDto> IUserService.GetFileStream(string path)
        {
            return new OpResult<FileStreamDto>().TrySetData(() =>
                {
                    var ext = Path.GetExtension(path);
                    return new FileStreamDto
                        {
                            FileName      = Path.GetFileName(path),
                            FileExtension = ext,
                            MimeType      = MimeTypeMap.GetMimeType(ext),
                            FileStream    = new FileStream(path, FileMode.Open, FileAccess.Read)
                        };
                });
        }

        IOpResult IUserService.UploadTermsAndConditions(int itemCount)
        {
            var result = new OpResult();

            _session.CanPerformAction(SystemActionType.SystemAdministrator).MergeInto(result);

            if(result.HasError)
                return result;

            string fileName   = null;
            string fileExt    = null;
            var httpRequest   = HttpContext.Current.Request;
            var postedFile    = httpRequest.Files["fileKey"];
            var serverPath    = System.Configuration.ConfigurationManager.AppSettings["AlertDirectory"];
            var oldServerPath = serverPath + @"\OldServiceAgreements";

            try
            {
                if (postedFile != null)
                {
                    if ( !Directory.Exists(serverPath) )    Directory.CreateDirectory(serverPath);
                    if ( !Directory.Exists(oldServerPath) ) Directory.CreateDirectory(oldServerPath);

                    fileName = postedFile.FileName;
                    fileExt  = Path.GetExtension(fileName);

                    if (fileExt.ToUpper().Contains("PDF"))
                    {
                        var filePath = serverPath + @"\" + fileName;
                        if (File.Exists(filePath))
                        {
                            var oldFileName = "PayrollServiceAgreement_" + itemCount + ".pdf";
                            File.Move(filePath, oldServerPath + @"\" + oldFileName);
                        }

                        _objUserService.InsertTermsAndConditionsVersion(serverPath);

                        postedFile.SaveAs(filePath);
                    } else { 
                        result.SetToFail();
                        result.AddMessage(new GenericMsg("File to upload must be a PDF."));
                    } // end of if pdf
                } // end of has file
                else
                {
                    result.SetToFail();
                    result.AddMessage(new GenericMsg("No file sent over request."));
                }
            }   // end of try
            catch(Exception ex)
            {
                result.SetToFail();
                result.AddMessage(new GenericMsg("Failed to upload file."));
            }
            

            return result;
        }

        IOpResult<UserTermsAndConditionsDto> IUserService.ProcessUsersTermsAndConditionsAcceptance(int userId, bool isAccepted)
        {
            var result = new OpResult<UserTermsAndConditionsDto>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.User, userId).MergeInto(result);

            if (result.HasError) return result;

            _userProvider.ProcessUsersTermsAndConditionsAcceptance(userId, isAccepted).MergeAll(result);

            return result;
        }

        IOpResult<IEnumerable<UserDto>> IUserService.GetCompanyAdminForClient(int clientId)
        {
            var result = new OpResult<IEnumerable<UserDto>>();

            if (result.Success)
            {
                result.TrySetData(() =>
                _session.UnitOfWork.UserRepository
                    .QueryUsers()
                    .ByClientId(clientId)
                    .ExecuteQueryAs(u => new UserDto()
                    {
                        UserId = u.UserId,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        UserTypeId = u.UserTypeId
                    }).OrderBy(n => n.LastName).ToList());
            }
            result.Data = result.Data.Where(t => t.UserTypeId == UserType.CompanyAdmin);
            return result;
        }

        #endregion


        IOpResult<UserPermissionsDto> IUserService.GetUserPermissions(int userId)
        {
            var result = new OpResult<UserPermissionsDto>();

            var userPermissions = _session.UnitOfWork.UserRepository.UserPermissionsQuery()
                .ByUserId(userId)
                .ExecuteQueryAs(x => new UserPermissionsDto
                {
                    UserId                   = x.UserId,
                    IsEmployeeNavigatorAdmin = x.IsEmployeeNavigatorAdmin
                }).FirstOrDefault();

            result.Data = userPermissions;

            return result;
        }

        IOpResult IUserService.UpdateUserPermissions(UserPermissionsDto dto, bool isNewRecord)
        {
            var result = new OpResult();
            //TODO: As this get expanded to more permissions or is used in different areas, check permissions before calling this method
            // or determine what is being updated and what requirements need to be met for that property to be updated.
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(_session.LoggedInUserInformation.ClientId ?? 0).MergeInto(result);

            if (result.HasError) return result;

            var permissionEntity = new UserPermissions 
            { 
                UserId                   = dto.UserId,
                IsEmployeeNavigatorAdmin = dto.IsEmployeeNavigatorAdmin
            };


            if (isNewRecord) _session.UnitOfWork.RegisterNew(permissionEntity);
            else _session.UnitOfWork.RegisterModified(permissionEntity);

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult IUserService.SyncUserRecoveryEmail()
        {
            var result = new OpResult();
            var userId = _session.LoggedInUserInformation.UserId;
            var authUserId = _session.LoggedInUserInformation.AuthUserId;

            if (authUserId <= 0)
                return result.SetToFail("Cannot find auth user id.");

            var user = _session.UnitOfWork.UserRepository.QueryUsers()
                .ByUserId(userId)
                .ExecuteQueryAs(x => new { x.EmailAddress, x.UserTypeId })
                .FirstOrDefault();

            var appEmail = user.EmailAddress ?? string.Empty;

            // select emailAddress from user where userid = 142811
            var emailResult = _loginService
                .GetUserProfileAttribute(authUserId, UserProfileAttributeTypeId.RecoveryEmail)
                .MergeInto(result);

            if (result.HasError)
                return result;

            var authEmail = emailResult.Data?.Data ?? string.Empty;

            if (authEmail.ToLower() != appEmail.ToLower())
            {
                var r = new SyncUsersEmailsSproc(userId, (int)user.UserTypeId, authEmail);
                r.Execute(ConfigValues.DominionContext).MergeInto(result);
            }

            return result;
        }


    }
}
