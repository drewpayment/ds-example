using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Notification;
using Dominion.Core.Dto.User;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Notification
{
    public interface INotificationTypeUserTypeQuery : IQuery<Domain.Entities.Notification.NotificationTypeUserType, INotificationTypeUserTypeQuery>
    {
        /// <summary>
        /// Filters NotificationTypeUserTypes by notification type ID
        /// </summary>
        /// <param name="notificationTypeId">The ID of the notification type to NotificationTypeUserType info for.</param>
        /// <returns></returns>
        INotificationTypeUserTypeQuery ByNotificationTypeId(NotificationType notificationTypeId);

        /// <summary>
        /// Filters NotificationTypeUserTypes by user type ID
        /// </summary>
        /// <param name="userTypeId">The ID of the user type to NotificationTypeUserType info for.</param>
        /// <returns></returns>
        INotificationTypeUserTypeQuery ByUserTypeId(UserType userTypeId);
    }
}
