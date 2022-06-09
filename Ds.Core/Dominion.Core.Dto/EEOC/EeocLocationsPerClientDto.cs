using System.Collections.Generic;
using Dominion.Core.Dto.Contact;

namespace Dominion.Core.Dto.EEOC
{
    public class EeocLocationsPerClientDto
    {
        public IEnumerable<EEOCLocationDto> EeocLocations { get; set; }
    }
}
