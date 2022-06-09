using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;

namespace Dominion.Domain.Entities.Employee
{
    public partial class ClockClientGeofence : Entity<ClockClientGeofence>, IHasModifiedOptionalData
    {
        public virtual int ClockClientGeofenceID { get; set; }
        public virtual int ClientID { get; set; }
        public virtual double Lat { get; set; }
        public virtual double Lng { get; set; }
        public virtual double Radius { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual bool? IsArchived { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual int? ModifiedBy { get; set; }
    }
}
