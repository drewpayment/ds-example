using System.Collections.Generic;

namespace Dominion.Core.Dto.Notification
{
    public interface INotificationRecipient : IRecipient
    {
        IEnumerable<NotificationContentBuilder> Notifications { get; }
    }
}
