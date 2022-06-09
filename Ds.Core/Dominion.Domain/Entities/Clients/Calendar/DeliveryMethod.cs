using System.Collections.Generic;

using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients.Calendar
{
    /// <summary>
    /// Entity representation of the dbo.CalendarDelivery table.
    /// </summary>
    public partial class DeliveryMethod : Entity<DeliveryMethod>
    {
        public virtual byte   DeliveryMethodId   { get; set; } 
        public virtual string Name               { get; set; } 
        public virtual string Code               { get; set; } 
        public virtual int    TimesToPrint       { get; set; } 

        public virtual ICollection<ClientCalendar> ClientCalendars { get; set; } // many-to-one;
    }
}
