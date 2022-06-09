using System;

namespace Dominion.Core.Dto.Notification
{
    public class NotificationContactPreferenceDto : IHasNotificationType
    {
        public int              NotificationContactPreferenceId { get; set; }
        public int              NotificationContactId           { get; set; }
        public NotificationType NotificationTypeId              { get; set; }
        public string           ContactLabel                    { get; set; }
        public string           ContactDescription              { get; set; }
        public bool             CanClientControl                { get; set; }
        public bool             SendEmail                       { get; set; }
        public bool             SendSms                         { get; set; }
        public DateTime         Modified                        { get; set; }
        public int              ModifiedBy                      { get; set; }
    }
}
