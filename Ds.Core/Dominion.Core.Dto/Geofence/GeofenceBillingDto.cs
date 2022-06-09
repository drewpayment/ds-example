using System;

namespace Dominion.Core.Dto.Geofence
{
    public partial class GeofenceBillingDto
    {
        public int GeofenceBillingID { get; set; }
        public int ClientID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime RunDate { get; set; }
        public DateTime ActiveDate { get; set; }
    }
}
