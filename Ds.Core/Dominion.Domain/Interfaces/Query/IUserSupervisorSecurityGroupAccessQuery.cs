using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Security;
using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Security;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Gets the supervisor security access information for specific types.
    /// See: UserSecurityGroupType enum (search for it)
    /// This is where we say 'supervisor a' has access to 'client cost center a'.
    /// The table is a lookup style table with no primary key or foreign keys
    /// </summary>
    public interface IUserSupervisorSecurityGroupAccessQuery : IQuery<UserSupervisorSecurityGroupAccess, IUserSupervisorSecurityGroupAccessQuery>
    {
        /// <summary>
        /// pass:1
        /// Filter by User ID.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns></returns>
        IUserSupervisorSecurityGroupAccessQuery ByUserId(int userId);

        /// <summary>
        /// pass:1
        /// Filter by the user security group type.
        /// </summary>
        /// <returns></returns>
        IUserSupervisorSecurityGroupAccessQuery ByUserSecurityGroupType(UserSecurityGroupType userSecurityGroupType);

        IUserSupervisorSecurityGroupAccessQuery ByForeignKey(int id);

        /// <summary>
        /// Filter by a particular user type.
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        IUserSupervisorSecurityGroupAccessQuery ByUserType(UserType userType);

        IUserSupervisorSecurityGroupAccessQuery ByCanApproveHours(bool canApprove);

        IUserSupervisorSecurityGroupAccessQuery ByClientId(int clientId);

        IUserSupervisorSecurityGroupAccessQuery ByActiveUserEmployee(bool isActive);

        IUserSupervisorSecurityGroupAccessQuery ByUserIsEnabled(bool isEnabled);
    }

}
