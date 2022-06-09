using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Employee
{
    public partial class ClockEmployeePunchLocation : Entity<ClockEmployeePunchLocation>
    {
        public virtual int ClockEmployeePunchLocationID { get; set; }
        public virtual double Accuracy { get; set; }
        public virtual double Altitude { get; set; }
        public virtual double AltitudeAccuracy { get; set; }
        public virtual int Floor { get; set; }
        public virtual double Heading { get; set; }
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
        public virtual double Speed { get; set; }
        public virtual int? ClockClientGeofenceID { get; set; }
        public virtual double ClockClientGeofenceLat { get; set; }
        public virtual double ClockClientGeofenceLng { get; set; }

    }
}
