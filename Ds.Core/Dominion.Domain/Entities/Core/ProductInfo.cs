using System.Collections.Generic;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Notification;

namespace Dominion.Domain.Entities.Core
{
    public class ProductInfo : Entity<ProductInfo>
    {
        public virtual Product ProductId { get; set; }
        public virtual string  Name      { get; set; }
        public virtual string  IconName  { get; set; }

        public virtual ICollection<NotificationTypeInfo> NotificationTypes { get; set; }
    }
}
