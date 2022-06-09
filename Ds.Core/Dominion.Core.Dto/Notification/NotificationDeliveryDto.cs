using System;

namespace Dominion.Core.Dto.Notification
{
    public partial class NotificationDeliveryDto
    {
        public int                            NotificationUserId { get; set; }
        public NotificationDeliveryMethod     DeliveryMethodId   { get; set; }
        public string                         Recipient          { get; set; }
        public DateTime                       DateSent           { get; set; }

        //FOREIGN KEYS
        public NotificationUserDto NotificationUsers { get; set; }
        
    }
}
