using Dominion.Core.Dto.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Tax
{
    public class LegacyLocalTaxDto : ILegacyTaxIdAndType
    {
        public int            LocalTaxId         { get; set; } 
        public string         Description        { get; set; } 
        public string         Code               { get; set; } 
        public int            StateId            { get; set; } 
        public string         W2Description      { get; set; } 
        public DateTime       Modified           { get; set; } 
        public string         Modifiedby         { get; set; } 
        public bool           IsW2ElectronicFile { get; set; } 
        public string         W2Code             { get; set; } 
        public string         MasterTaxCode      { get; set; } 
        public LegacyTaxType? LegacyTaxType      { get; set; } 
        public int?           ResidentId         { get; set; }
        public bool?          IsObligationTax    { get; set; }
        public int?           CountyId           { get; set; }
        public int?           SchoolDistrictId   { get; set; }
        public StateDto State                    { get; set; }
        //public CountyDto County { get; set; }
        //public SchoolDistrictDto SchoolDistrict { get; set; }

        // ILegacyTaxIdAndType Mappings
        public int TaxId => LocalTaxId;
    }
}
