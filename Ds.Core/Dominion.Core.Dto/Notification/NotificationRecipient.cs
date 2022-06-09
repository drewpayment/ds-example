using System.Collections.Generic;

namespace Dominion.Core.Dto.Notification
{
    public class NotificationRecipient : INotificationRecipient
    {
        public int? UserId { get; set; }
        public int? EmployeeId { get; set; }
        public int? ApplicantId { get; set; }
        public IEnumerable<NotificationContentBuilder> Notifications { get; set; }
    }
}
