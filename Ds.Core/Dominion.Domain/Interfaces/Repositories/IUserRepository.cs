using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Security;
using Dominion.Domain.Entities.User;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Dto;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IDisposable
    {
        User GetUser(int id);
        User GetUserByUserName(string userName);
        User GetUserByEmployeeId(int employeeId);
        IEnumerable<TResult> GetUsers<TResult>(QueryBuilder<User, TResult> queryBuilder) where TResult : class;

        IEnumerable<UserClientDto> GetCurrentUserInfoWithEmployeeForBenefitAdminEmails(int clientId);


        /// <summary>
        /// Gets the specified information for the given user.
        /// </summary>
        /// <typeparam name="TResult">The object type to which the resulting user info will be mapped.</typeparam>
        /// <param name="id">ID of the user to retrieve.</param>
        /// <param name="selector">Mapping expression describing how to translate the user entity to the object type to be returned.</param>
        /// <returns>User entity info in the specified mapped object form.</returns>
        TResult GetUser<TResult>(int id, Expression<Func<User, TResult>> selector) where TResult : class;

        #region USER CHANGE HISTORY

        /// <summary>
        /// Gets the user change history that satisfies the specified query.
        /// </summary>
        /// <typeparam name="TResult">Type the result set will be projected onto.</typeparam>
        /// <param name="query">Querybuilder containing the query criteria.</param>
        /// <returns>User change history satisfying the given query.</returns>
        IEnumerable<TResult> GetUserChangeHistory<TResult>(QueryBuilder<UserChangeHistory, TResult> query)
            where TResult : class;

        #endregion

        #region USER_ACTION_TYPE

        UserActionType GetUserActionType(int id);

        IEnumerable<TResult> GetUserActionTypes<TResult>(QueryBuilder<UserActionType, TResult> queryBuilder)
            where TResult : class;

        /// <summary>
        /// Get the actions that the given user is allowed to perform for the specified client
        /// </summary>
        /// <param name="userId">ID of the user for whom allowed actions are retrieved.</param>
        /// <param name="clientId">ID of the client to whom the allowed actions apply.</param>
        /// <returns>The actions that the given user is allowed to perform for the specified client.</returns>
        IEnumerable<UserActionType> GetUserActionTypesForUser(int userId, int? clientId);

        #endregion // USER_ACTION_TYPE

        #region USER_ACTION_TYPE_LEGACY_USER_TYPE

        /// <summary>
        /// Get the UserActionTypeLegacyUserType with the specified ID.
        /// </summary>
        /// <param name="id">ID of the entity to be found.</param>
        /// <returns>Entity with the specified ID or null if the id was not found.</returns>
        UserActionTypeLegacyUserType GetUserActionTypeLegacyUserType(int id);

        /// <summary>
        /// Get the UserActionType entities that are associated with the given legacy user id.
        /// </summary>
        /// <param name="legacyUserTypeId">The legacy user id for which to retrieve associated UserActionTypes</param>
        /// <returns>The UserActionType entities that are associated with the given legacy user id.</returns>
        IEnumerable<UserActionType> GetUserActionTypesForLegacyUserTypeId(UserType legacyUserTypeId);

        #endregion // USER_ACTION_TYPE_LEGACY_USER_TYPE

        // UserGroupMembership methods
        UserGroupMembership GetUserGroupMembership(int id);

        IEnumerable<TResult> GetUserGroupMemberships<TResult>(QueryBuilder<UserGroupMembership, TResult> queryBuilder)
            where TResult : class;

        // UserType methods
        UserTypeInfo GetUserType(int id);
        UserTypeInfo GetUserTypeForUserId(int userId);
        IEnumerable<UserTypeInfo> GetAllUserTypes();

        /// <summary>
        /// Create a new query on <see cref="User"/> data.
        /// </summary>
        /// <returns></returns>
        IUserQuery QueryUsers();

        IUserTermsAndConditionsQuery QueryUserTermsAndConditions();

        /// <summary>
        /// Creates a new query on <see cref="UserClient"/> data.
        /// </summary>
        /// <returns></returns>
        IUserClientQuery QueryUserClientSettings();

        /// <summary>
        /// Queries <see cref="UserSupervisorSecuritySetting"/> data (i.e. 
        /// user-level supervisor settings).
        /// </summary>
        /// <returns></returns>
        IUserSupervisorSecuritySettingQuery QuerySupervisorSecuritySettings();
        
        /// <summary>
        /// Queries <see cref="UserSupervisorSecurityGroupAccess"/> data (i.e. what
        /// groups, employees a supervisor has access to).
        /// </summary>
        /// <returns></returns>
        IUserSupervisorSecurityGroupAccessQuery QuerySupervisorSecurityGroupAccess();

        /// <summary>
        /// Queries <see cref="UserActionType"/> data.
        /// </summary>
        /// <returns></returns>
        IUserActionTypeQuery QueryUserActionTypes();
		
		/// <summary>
        /// Create a new query on <see cref="UserPin"/> data.
        /// </summary>
        /// <returns></returns>
        IUserPinQuery QueryUserPins();

        /// <summary>
        /// Create a new query on <see cref="AccessRule"/> data.
        /// </summary>
        /// <returns></returns>
        IAccessRuleQuery QueryAccessRules();
        IUserBetaFeatureQuery UserBetaFeatureQuery();

        /// <summary>
        /// Create a new query on <see cref="UserPermissions"/> data.
        /// </summary>
        /// <returns></returns>
        IUserPermissionsQuery UserPermissionsQuery();
    }
}