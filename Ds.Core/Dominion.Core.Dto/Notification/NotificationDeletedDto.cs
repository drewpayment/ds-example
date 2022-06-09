using System;
using Dominion.Core.Dto.User;

namespace Dominion.Core.Dto.Notification
{
    public partial class NotificationDeletedDto
    {
        public int       NotificationUserId { get; set; }
        public int       NotificationId     { get; set; }
        public int       UserId             { get; set; }
        public bool      IsRead             { get; set; }
        public DateTime? DateRead           { get; set; }
        public DateTime  DateDeleted        { get; set; }

        //FOREIGN KEYS
        public NotificationDto Notification { get; set; }
        public UserDto         User         { get; set; }
    }
}
