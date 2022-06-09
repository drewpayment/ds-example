using Dominion.Core.Dto.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Tax
{
    public class LegacyDisabilityTaxDto : ILegacyTaxIdAndType
    {
        public int           DisabilityTaxId    { get; set; } 
        public string        Description        { get; set; } 
        public string        Code               { get; set; } 
        public int           StateId            { get; set; } 
        public DateTime      Modified           { get; set; } 
        public string        Modifiedby         { get; set; } 
        public string        W2Description      { get; set; } 
        public bool          IsW2ElectronicFile { get; set; } 
        public string        W2Code             { get; set; } 
        public bool          IsAddWithState     { get; set; } 
        public string        MasterTaxCode      { get; set; } 
        public bool          IsShowInBox14      { get; set; }
        public LegacyTaxType TaxTypeId          { get; set; }

        public StateDto State { get; set; }

        // ILegacyTaxIdAndType Mappings
        public int TaxId => DisabilityTaxId;
        public LegacyTaxType? LegacyTaxType => TaxTypeId;
    }
}
