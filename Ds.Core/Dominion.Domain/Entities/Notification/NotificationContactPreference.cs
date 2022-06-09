using System;
using Dominion.Core.Dto.Notification;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Notification
{
    public class NotificationContactPreference : Entity<NotificationContactPreference>, IHasModifiedData
    {
        public virtual int               NotificationContactPreferenceId { get; set; }
        public virtual int               NotificationContactId           { get; set; }
        public virtual NotificationType  NotificationTypeId              { get; set; }
        public virtual bool              SendEmail                       { get; set; }
        public virtual bool              SendSms                         { get; set; }
        public virtual DateTime         Modified                        { get; set; }
        public virtual int              ModifiedBy                      { get; set; }

        public virtual NotificationContact  NotificationContact { get; set; }
        public virtual NotificationTypeInfo NotificationType    { get; set; }

    }
}
