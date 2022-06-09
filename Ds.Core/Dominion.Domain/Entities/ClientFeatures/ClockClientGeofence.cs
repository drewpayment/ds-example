using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;

namespace Dominion.Domain.Entities.ClientFeatures
{
    public partial class ClockClientGeofence : Entity<ClockClientGeofence>, IHasModifiedOptionalData
    {
        public virtual int ClockClientGeofenceID { get; set; }
        public virtual int ClientID { get; set; }
        public virtual Decimal Lat { get; set; }
        public virtual Decimal Lng { get; set; }
        public virtual Decimal Radius { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual bool? IsArchived { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual int? ModifiedBy { get; set; }
    }
}
