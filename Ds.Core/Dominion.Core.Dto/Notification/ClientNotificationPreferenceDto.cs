using System;

namespace Dominion.Core.Dto.Notification
{
    public class ClientNotificationPreferenceDto : IHasNotificationType
    {
        public int              ClientNotificationPreferenceId { get; set; }
        public int              ClientId                       { get; set; }
        public NotificationType NotificationTypeId             { get; set; }
        public string           ClientLabel                    { get; set; }
        public string           ClientDescription              { get; set; }
        public bool             CanClientControl               { get; set; }
        public bool             IsEnabled                      { get; set; }
        public bool             UserMustAcknowledge            { get; set; }
        public bool             AllowCustomFrequency           { get; set; }
        public DateTime         Modified                       { get; set; }
        public int              ModifiedBy                     { get; set; }
    }
}
