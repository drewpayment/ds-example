using System;
using Dominion.Core.Dto.Notification;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Notification
{
    public class ClientNotificationPreference : Entity<ClientNotificationPreference>,  IHasModifiedData
    {
        public virtual int              ClientNotificationPreferenceId { get; set; }
        public virtual int              ClientId                       { get; set; }
        public virtual NotificationType NotificationTypeId             { get; set; }
        public virtual bool             IsEnabled                      { get; set; }
        public virtual bool             UserMustAcknowledge            { get; set; }
        public virtual bool             AllowCustomFrequency           { get; set; }
        public virtual DateTime         Modified                       { get; set; }
        public virtual int              ModifiedBy                     { get; set; }

        public virtual Client               Client           { get; set; }
        public virtual NotificationTypeInfo NotificationType { get; set; }
    }
}
