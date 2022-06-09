using System;

namespace Dominion.Core.Dto.Geofence
{
    public partial class ClockClientGeofenceDto 
    {
        public int ClockClientGeofenceID { get; set; }
        public int ClientID { get; set; }
        public Decimal Lat { get; set; }
        public Decimal Lng { get; set; }
        public Decimal Radius { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool? IsArchived { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
