using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Notification
{
    public class NotificationContact : Entity<NotificationContact>, IHasModifiedData
    {
        public virtual int       NotificationContactId { get; set; }
        public virtual int?      UserId                { get; set; }
        public virtual int?      EmployeeId            { get; set; }
        public virtual string    Email                 { get; set; }
        public virtual string    PhoneNumber           { get; set; }
        public virtual DateTime  Modified              { get; set; }
        public virtual int       ModifiedBy            { get; set; }

        public virtual User.User                                  User                           { get; set; }
        public virtual Employee.Employee                          Employee                       { get; set; }
        public virtual ICollection<NotificationContactPreference> NotificationContactPreferences { get; set; }
    }
}
