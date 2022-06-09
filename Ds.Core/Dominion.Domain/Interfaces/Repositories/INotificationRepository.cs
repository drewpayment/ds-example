using System;
using Dominion.Domain.Interfaces.Query.Notification;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface INotificationRepository : IRepository, IDisposable
    {
        /// <summary>
        /// Constructs a new query on Notification Contact data.
        /// </summary>
        /// <returns></returns>
        INotificationContactQuery QueryNotificationContacts();

        /// <summary>
        /// Constructs a new query on Notification Contact Preference data.
        /// </summary>
        /// <returns></returns>
        INotificationContactPreferenceQuery QueryNotificationContactPreferences();

        /// <summary>
        /// Constructs a new query on Client Notificaiton Preference data.
        /// </summary>
        /// <returns></returns>
        IClientNotificationPreferenceQuery QueryClientNotificationPreferences();

        /// <summary>
        /// Constructs a new query on Product data.
        /// </summary>
        /// <returns></returns>
        IProductQuery QueryProducts();

        /// <summary>
        /// Constructs a new query on Notification Type data.
        /// </summary>
        /// <returns></returns>
        INotificationTypeQuery QueryNotificationTypes();

        
    }
}
