using System;

namespace Dominion.LaborManagement.Dto.Clock.Misc
{
    public class RealTimePunchLocation
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
    }
}
