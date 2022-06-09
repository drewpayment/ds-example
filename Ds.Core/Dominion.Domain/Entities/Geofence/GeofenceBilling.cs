using Dominion.Domain.Entities.Base;
using System;

namespace Dominion.Domain.Entities.Geofence
{
    public partial class GeofenceBilling : Entity<GeofenceBilling>
    {
        public virtual int GeofenceBillingID { get; set; }
        public virtual int ClientID { get; set; }
        public virtual int ClockEmployeeID { get; set; }
        public virtual DateTime RunDate { get; set; }
        public virtual DateTime ActiveDate { get; set; }
    }
}
