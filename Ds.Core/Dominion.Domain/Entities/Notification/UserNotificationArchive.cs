using System;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Notification
{
    public class UserNotificationArchive : Entity<UserNotificationArchive>
    {
        public virtual int       UserNotificationId { get; set; }
        public virtual int       NotificationId     { get; set; }
        public virtual int       UserId             { get; set; }
        public virtual bool      IsRead             { get; set; }
        public virtual DateTime? DateRead           { get; set; }
        public virtual DateTime? DateDeleted { get; set; }

        public virtual Notification Notification { get; set; }
        public virtual User.User    User         { get; set; }
    }
}
