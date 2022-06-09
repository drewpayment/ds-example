using Dominion.Domain.Entities.Base;
using System;

namespace Dominion.Domain.Entities.Employee.ClockEmployeeInfo
{
    public partial class ClockEmployeePunchLocation : Entity<ClockEmployeePunchLocation>
    {
        public virtual int ClockEmployeePunchLocationID { get; set; }
        public virtual Decimal Accuracy { get; set; }
        public virtual Decimal Altitude { get; set; }
        public virtual Decimal AltitudeAccuracy { get; set; }
        public virtual int Floor { get; set; }
        public virtual Decimal Heading { get; set; }
        public virtual Decimal Latitude { get; set; }
        public virtual Decimal Longitude { get; set; }
        public virtual Decimal Speed { get; set; }
        public virtual int? ClockClientGeofenceID { get; set; }
        public virtual Decimal ClockClientGeofenceLat { get; set; }
        public virtual Decimal ClockClientGeofenceLng { get; set; }
}
}
