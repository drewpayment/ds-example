using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Notification;
using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.User;

namespace Dominion.Domain.Entities.Notification
{
    public class NotificationTypeUserType : Entity<NotificationTypeUserType>
    {
        public NotificationType NotificationTypeId { get; set; }
        public UserType         UserTypeId         { get; set; }

        public virtual NotificationTypeInfo NotificationType { get; set; }

        public virtual UserTypeInfo UserType { get; set; }
    }
}
