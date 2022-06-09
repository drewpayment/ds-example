using System.Collections.Generic;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Notification;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;

namespace Dominion.Domain.Entities.Notification
{
    public class NotificationTypeInfo : Entity<NotificationTypeInfo>
    {
        public virtual NotificationType     NotificationTypeId       { get; set; }
        public virtual Product              ProductId                { get; set; }
        public virtual string               ClientLabel              { get; set; }
        public virtual string               ClientDescription        { get; set; }
        public virtual string               ContactLabel             { get; set; }
        public virtual string               ContactDescription       { get; set; }
        public virtual bool                 CanClientControl         { get; set; }
        public virtual bool                 IsEnabledDefault_Client  { get; set; }
        public virtual bool                 IsEnabledDefault_User    { get; set; }

        public virtual ProductInfo Product { get; set; }
        public virtual ICollection<NotificationTypeUserType> NotificationTypeUserType { get; set; }
    }
}
