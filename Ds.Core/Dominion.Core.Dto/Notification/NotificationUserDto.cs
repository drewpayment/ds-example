using System;

namespace Dominion.Core.Dto.Notification
{
    public partial class NotificationUserDto
    {
        public int      NotificationUserId { get; set; }
        public int      NotificationId     { get; set; }
        public bool     IsRead             { get; set; }
        public DateTime DateRead           { get; set; }

        //FOREIGN KEYS
        public NotificationDto Notification { get; set; }
    }
}
