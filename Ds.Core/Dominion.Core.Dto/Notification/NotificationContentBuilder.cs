using System;

namespace Dominion.Core.Dto.Notification
{
    public class NotificationContentBuilder
    {
        //public INotificationData Data         { get; set; }
        public Func<EmailDto>    EmailBuilder { get; set; }
        public Func<SmsDto>      SmsBuilder   { get; set; }
    }
}
