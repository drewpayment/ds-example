using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Tax
{
    public interface ILegacyTaxIdAndType
    {
        /// <summary>
        /// The ID of the tax.
        /// </summary>
        int TaxId { get; }

        /// <summary>
        /// The ID of the tax type.
        /// </summary>
        LegacyTaxType? LegacyTaxType { get; }
    }

    public class BasicLegacyTaxIdAndTypeDto : ILegacyTaxIdAndType
    {
        public int TaxId { get; set; }

        public LegacyTaxType? LegacyTaxType { get; set; }
    }
}
