using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Entities.Onboarding;
using System;
using Dominion.Core.Dto.Tax;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominion.Domain.Entities.Tax.Legacy
{
    /// <summary>
    /// Entity representation of a Legacy Local Tax.
    /// </summary>
    public class LegacyLocalTax : Entity<LegacyLocalTax>, ILegacyTaxIdAndType
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
        public bool           BlockOverrides     { get; set; }

        public virtual State          State              { get; set; }
        public virtual County         County             { get; set; }
        public virtual SchoolDistrict SchoolDistrict     { get; set; }

        // ILegacyTaxIdAndType Properties
        [NotMapped]
        public virtual int TaxId => LocalTaxId;
    }
}
