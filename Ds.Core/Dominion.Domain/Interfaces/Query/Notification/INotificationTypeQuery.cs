using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Notification;
using Dominion.Domain.Entities.Notification;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Notification
{
    public interface INotificationTypeQuery : IQuery<NotificationTypeInfo, INotificationTypeQuery>
    {
        /// <summary>
        /// Filters Notification Types by notification type ID.
        /// </summary>
        /// <param name="notificationTypeId">The ID of the notification type to filter by.</param>
        /// <returns></returns>
        INotificationTypeQuery ByNotificationTypeId(NotificationType notificationTypeId);

        /// <summary>
        /// Filters Notification Types by product ID.
        /// </summary>
        /// <param name="productId">The ID of the product to filter notification types for.</param>
        /// <returns></returns>
        INotificationTypeQuery ByProductId(Product productId);
    }
}
