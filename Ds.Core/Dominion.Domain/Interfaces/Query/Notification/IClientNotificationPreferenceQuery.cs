using Dominion.Core.Dto.Notification;
using Dominion.Domain.Entities.Notification;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Notification
{
    public interface IClientNotificationPreferenceQuery : IQuery<ClientNotificationPreference, IClientNotificationPreferenceQuery>
    {
        /// <summary>
        /// Filters Client Notification Preferences by client notification preference ID.
        /// </summary>
        /// <param name="clientNotificationPreferenceId">The ID of the client notification preference to return.</param>
        /// <returns></returns>
        IClientNotificationPreferenceQuery ByClientNotificationPreferenceId(int clientNotificationPreferenceId);

        /// <summary>
        /// Filters Client Notification Preferences by client ID.
        /// </summary>
        /// <param name="clientId">The ID of the client to get notificaiton preferences for.</param>
        /// <returns></returns>
        IClientNotificationPreferenceQuery ByClientId(int clientId);

        /// <summary>
        /// Filters Client Notification Preferences by notification type.
        /// </summary>
        /// <param name="notificationType">The type of notification to get client preferences for.</param>
        /// <returns></returns>
        IClientNotificationPreferenceQuery ByNotificationType(NotificationType notificationType);

        /// <summary>
        /// Filters Client Notification Preferences by whether they are enabled or not. 
        /// By default, we will filter for enabled.
        /// </summary>
        /// <param name="isEnabled">The flag to filter for enabled or disabled notification preferences.</param>
        /// <returns></returns>
        IClientNotificationPreferenceQuery ByIsEnabled(bool isEnabled = true);
    }
}
