using Dominion.Core.Dto.Notification;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Notification
{
    public interface INotificationContactPreferenceQuery : IQuery<Domain.Entities.Notification.NotificationContactPreference, INotificationContactPreferenceQuery>
    {
        /// <summary>
        /// Filters contact preferences by notification contact preference ID.
        /// </summary>
        /// <param name="contactPreferenceId">The ID of the notification contact preference to get info for.</param>
        /// <returns></returns>
        INotificationContactPreferenceQuery ByNotificationContactPreferenceId(int contactPreferenceId);

        /// <summary>
        /// Filters preferences by notification contact ID.
        /// </summary>
        /// <param name="contactId">The ID of the Notification Contact to get preferences for.</param>
        /// <returns></returns>
        INotificationContactPreferenceQuery ByNotificationContactId(int contactId);

        /// <summary>
        /// Filters preferences by notification contact ID and notification type.
        /// </summary>
        /// <param name="contactId">The ID of the Notification Contact to get preferences for.</param>
        /// <param name="type">The type of notification to get perferences for.</param>
        /// <returns></returns>
        INotificationContactPreferenceQuery ByNotificationContactAndType(int contactId, NotificationType type);
    }
}
