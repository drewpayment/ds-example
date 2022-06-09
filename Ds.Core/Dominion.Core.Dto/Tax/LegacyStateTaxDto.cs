using Dominion.Core.Dto.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Tax
{
    public class LegacyStateTaxDto : ILegacyTaxIdAndType
    {
		public int    StateTaxId                { get; set; } 
        public string Description               { get; set; } 
        public int    StateId                   { get; set; } 
        public bool   IsW2ElectronicFile        { get; set; } 
        public string MasterTaxCode             { get; set; }
        public string MasterTaxCodeStateFuta { get; set; }
        public int DefaultTaxFrequencyId { get; set; }
        public byte DefaultCalendarDebitId { get; set; }
        public bool BlockOverrides { get; set; }

        public StateDto State                   { get; set; }

        // ILegacyTaxIdAndType Mappings
        public int TaxId => StateTaxId;
        public LegacyTaxType? LegacyTaxType => Dominion.Core.Dto.Tax.LegacyTaxType.StateWitholding;
    }
}
