using Dominion.Core.Dto.Notification;
using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.User;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query on <see cref="User"/> data.
    /// </summary>
    public interface IUserQuery : IQuery<User, IUserQuery>
    {
        /// <summary>
        /// Filters by a specific user (by userId).
        /// </summary>
        /// <param name="userId">ID of user to filter by.</param>
        /// <returns></returns>
        IUserQuery ByUserId(int userId);

        IUserQuery ByUserIds(params int[] userIds);

        /// <summary>
        /// Filters by a specific user (by username).
        /// </summary>
        /// <param name="username">Username of user to filter by. Must be exact match (case-insensitive).</param>
        /// <returns></returns>
        IUserQuery ByUsername(string username);

        /// <summary>
        /// Filters by users associated with the specified client (per UserClient).
        /// </summary>
        /// <param name="clientId">ID of client the user is associated with.</param>
        /// <returns></returns>
        IUserQuery ByClientId(int clientId);

        /// <summary>
        /// Filters according to <see cref="UserClient"/> records for each <see cref="User"/>.
        /// 
        /// This is different than filtering for all users for a given client, 
        /// as not all users for a given client are guaranteed to have a 
        /// <see cref="UserClient"/> record indicating that relation.
        /// </summary>
        /// <param name="clientIds"></param>
        /// <returns></returns>
        IUserQuery ByClients(int[] clientIds);

        /// <summary>
        /// Filters by users associated with an employee belonging to the specified client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IUserQuery ByEmployeeClientId(int clientId);

        /// <summary>
        /// Filters by users associated with an applicant belonging to the specified client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IUserQuery ByApplicantClientId(int clientId);

        IUserQuery ByUserTypeId(Dominion.Core.Dto.User.UserType userTypeId);

        IUserQuery ByUserTypeAllButSystemAdmin();

        IUserQuery ByApplicantAdmin();
        IUserQuery ByLastClientId(int clientId);

        /// <summary>
        /// Filters by users who are benefit administrators for the given client.
        /// </summary>
        /// <returns></returns>
        IUserQuery ByIsBenefitAdministratorForClient(int clientId);

        /// <summary>
        /// Filters by users with the specified 'disabled' setting.
        /// </summary>
        /// <param name="isDisabled">If true, will return only users who are disabled. If false, will return active users.</param>
        /// <returns></returns>
        IUserQuery ByIsDisabled(bool isDisabled);

        IUserQuery ByEmployeeId(int employeeId);

        IUserQuery ByCanViewTaxPackets(bool canView = true);

        /// <summary>
        /// Filters by users for a client by checking the UserClient table and
        /// Employee.ClientId associated with the user.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IUserQuery ByUserClientOrEmployeeClientId(int clientId);

        /// <summary>
        /// Filters users by the CanAddSystemAdmins setting/flag
        /// </summary>
        /// <param name="isSuperSystemAdmin">Whether or not the user is a system admin. Defaults to true.</param>
        /// <returns></returns>
        IUserQuery ByIsSuperSystemAdmin(bool isSuperSystemAdmin = true);

        IUserQuery ByHasActiveEmployeeRecord();
        IUserQuery ByHasNoUserSecurityGroups();

        IUserQuery ExcludeTimeClockOnlyUsers();
        IUserQuery ByIsBillingAdmin();

        /// <summary>
        /// Filter users by a list of user types. 
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        IUserQuery ByUserTypes(ICollection<UserType> types);
        IUserQuery ByLockedUser();
        IUserQuery ByEmployees(IEnumerable<int> eeIds);
        IUserQuery ByNotificationType(NotificationType notificationType);
        IUserQuery ByCanSendEmail(bool canSendEmail = true);
        IUserQuery ByCanSendSms(bool canSendEmail = true);

    }
}
