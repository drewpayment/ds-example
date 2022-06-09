using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Location
{
    /// <summary>
    /// http://www.bitboost.com/ref/international-address-formats.html
    /// </summary>
    public class AddressBasicDto
    {
        public int StateId { get; set; }
        public int CountryId { get; set; }
        
        /// <summary>
        /// If needed.
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// If needed, not always used.
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Name of the city, town, etc.
        /// This can be a village, neighborhood, quarter, etc.
        /// Some non-us addresses require this.
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Either a state, province or locality name.
        /// </summary>
        public string StateProvinceName { get; set; }

        /// <summary>
        /// Either a US or Foreign postal code.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// If a zip extension was specified.
        /// </summary>
        public string PostalCodeExt { get; set; }

        /// <summary>
        /// The country name.
        /// </summary>
        public string CountryName { get; set; }
    }
}
