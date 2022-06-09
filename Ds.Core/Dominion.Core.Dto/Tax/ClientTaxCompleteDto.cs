using Dominion.Core.Dto.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Tax
{
    public class ClientTaxCompleteDto : ClientTaxDto, IClientTaxWithLegacyTaxes<LegacyLocalTaxDto, LegacyStateTaxDto, LegacyDisabilityTaxDto>
    {
        public LegacyLocalTaxDto LegacyLocalTax { get; set; }
        public LegacyLocalTaxDto LegacyOtherTax { get; set; }
        public LegacyStateTaxDto LegacyStateTax { get; set; }
        public LegacyDisabilityTaxDto LegacyDisabilityTax { get; set; }
    }
}
