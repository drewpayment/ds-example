using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Location;

//Future Consideration: Consolidate with Person & Address
namespace Dominion.Core.Dto.Contact
{
    [Serializable]
    public partial class EEOCLocationDto
    {
        public int    EeocLocationId { get; set; }
        public string EeocLocationDescription { get; set; }
        public int    ClientId { get; set; }
        public bool   IsActive { get; set; }
        public bool   IsHeadquarters { get; set; }
        public int?   UnitAddressId { get; set; }
        public string UnitNumber { get; set; }

        //FOREIGN KEYS
        public AddressDto UnitAddress { get; set; }
    }
}
