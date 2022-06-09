using System.Collections.Generic;
using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Notification;

namespace Dominion.Domain.Entities.User
{
    /// <summary>
    /// Container class for a user type.
    /// </summary>
    public class UserTypeInfo : Entity<UserTypeInfo>
    {
        public virtual UserType UserTypeId { get; set; }
        public virtual string Label { get; set; }

        public virtual ICollection<NotificationTypeUserType> NotificationTypeUserType { get; set; }
    }
}