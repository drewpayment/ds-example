
using System;

namespace Dominion.Core.Dto.Geofence
{
    public partial class RealTimePunchLocationDto
    {
        public int PunchLocationID { get; set; }
        public Decimal Accuracy { get; set; }
        public Decimal Altitude { get; set; }
        public Decimal AltitudeAccuracy { get; set; }
        public int Floor { get; set; }
        public Decimal Heading { get; set; }
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
        public Decimal Speed { get; set; }
        public bool IsValid { get; set; }
    }
}
