using Dominion.Core.Dto.Client;

namespace Dominion.Core.Dto.Geofence
{
    public partial class GeofenceOptInDto
    {
        public GeofenceOptionType GeofenceOptionID { get; set; }
        public ClientNotesDto ClientNotes { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserPin { get; set; }
        public bool OptIn { get; set; }
    }
}
