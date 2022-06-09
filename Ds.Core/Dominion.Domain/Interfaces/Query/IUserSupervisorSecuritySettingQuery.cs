using Dominion.Domain.Entities.Security;
using Dominion.Utility.Query;
using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.User;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Queries <see cref="UserSupervisorSecuritySetting"/> entities.
    /// </summary>
    public interface IUserSupervisorSecuritySettingQuery : IQuery<UserSupervisorSecuritySetting, IUserSupervisorSecuritySettingQuery>
    {
        /// <summary>
        /// Filters by user ID.
        /// </summary>
        /// <param name="userId">ID of user to filter by.</param>
        /// <returns></returns>
        IUserSupervisorSecuritySettingQuery ByUserId(int userId);

        /// <summary>
        /// Filters by users of the specified user type. Users with supervisor
        /// security settings *should* be <see cref="UserType.Supervisor"/> but
        /// could possibly have been changed.
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        IUserSupervisorSecuritySettingQuery ByUserType(UserType userType);

        /// <summary>
        /// Filters by users who have access to the specified client
        /// (per <see cref="UserClient"/> relationship).
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IUserSupervisorSecuritySettingQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by users who are allowed to edit the Group Scheduling Planner.
        /// </summary>
        /// <returns></returns>
        IUserSupervisorSecuritySettingQuery ByIsGroupPlannerEditingAllowed();

        IUserSupervisorSecuritySettingQuery ByViewManagerLinks();

        IUserSupervisorSecuritySettingQuery ByAllowAssignCompetencyModel();
    }
}
