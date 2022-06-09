using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Notification;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Notification
{
    public class Notification : Entity<Notification>
    {
        public virtual int              NotificationId { get; set; }
        public virtual NotificationType NotificationTypeId { get; set; }
        public virtual string           Details { get; set; }
        public virtual DateTime         DateGenerated { get; set; }

        public virtual NotificationTypeInfo NotificationType { get; set; }

        //public virtual ICollection<NotificationUser> NotificationUsers { get; set; }
        //public virtual ICollection<NotificationUserArchive> NotificationUserArchives { get; set; }
    }
}
