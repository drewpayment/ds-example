using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Notification
{
    public interface INotificationContactQuery : IQuery<Domain.Entities.Notification.NotificationContact, INotificationContactQuery>
    {
        /// <summary>
        /// Filters Notification Contacts by notification contact ID.
        /// </summary>
        /// <param name="notificationContactId">The ID of the notification contact to get info for.</param>
        /// <returns></returns>
        INotificationContactQuery ByNotificationContactId(int notificationContactId);

        /// <summary>
        /// Filters Notification Contacts by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user to get notification contact info for</param>
        /// <returns></returns>
        INotificationContactQuery ByUserId(int userId);

        /// <summary>
        /// Filters Notification Contacts by employee ID.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to get notification contact info for.</param>
        /// <returns></returns>
        INotificationContactQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters Notification Contacts by user ID or employee ID
        /// </summary>
        /// <param name="userId">The ID of the user to get notification contact info for.</param>
        /// <param name="employeeId">The ID of the employee to get notification contact info for.</param>
        /// <returns></returns>
        INotificationContactQuery ByUserIdOrEmployeeId(int? userId, int? employeeId);
    }
}
