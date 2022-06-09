using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Location
{
    /// <summary>
    /// This is an attempt to add a DTO that will capture data
    /// Some information on different formats: http://www.bitboost.com/ref/international-address-formats.html
    /// </summary>
    public class AddressExtendedDto : AddressBasicDto
    {
        /// <summary>
        /// If this is a United States Address then true.
        /// </summary>
        public bool IsUsa { get; set; }

        /// <summary>
        /// State abbreviation.
        /// </summary>
        public string StateProvinceAbbreviation { get; set; }

        /// <summary>
        /// State abbreviation.
        /// </summary>
        public string CountryAbbreviation { get; set; }

        /// <summary>
        /// Added with the ACA stuff.
        /// See enum description for details.
        /// The XSD to Objects creates an enum exactly like this, but only uses the name of the variables.
        /// This is a standard so I don't see it changing (hopefully) and I didn't want to put the ACA code in core.
        /// </summary>
        public StateCodeUSPS? StateCodeUSPS { get; set; }

        /// <summary>
        /// Added with the ACA stuff.
        /// See enum description for details.
        /// The XSD to Objects creates an enum exactly like this, but only uses the name of the variables.
        /// This is a standard so I don't see it changing (hopefully) and I didn't want to put the ACA code in core.
        /// </summary>
        public CountryCodeFIPS? CountryCodeFIPS { get; set; }
    }
}
