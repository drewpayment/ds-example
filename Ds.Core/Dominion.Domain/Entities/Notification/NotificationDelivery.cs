using System;
using Dominion.Core.Dto.Notification;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.PerformanceReviews;

namespace Dominion.Domain.Entities.Notification
{
    public class NotificationDelivery : Entity<NotificationDelivery>
    {
        public virtual int                        UserNotificationId { get; set; }
        public virtual NotificationDeliveryMethod DeliveryMethodId   { get; set; }
        public virtual string                     Recipient          { get; set; }
        public virtual DateTime                   DateSent           { get; set; }
        public virtual int? EvaluationReminderId { get; set; }

        public virtual UserNotificationArchive    UserNotification { get; set; }
        public virtual EvaluationReminder EvaluationReminderSettings { get; set; }
    }
}
