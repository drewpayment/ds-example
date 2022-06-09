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
    public partial class AddressDto
    {
        public int    AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int    StateId { get; set; }
        public int    CountryId { get; set; }
        public int?   CountyId { get; set; }
        public string ZipCode { get; set; }

    }
}
