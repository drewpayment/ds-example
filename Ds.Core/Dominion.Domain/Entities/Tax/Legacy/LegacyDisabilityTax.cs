using Dominion.Core.Dto.Tax;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominion.Domain.Entities.Tax.Legacy
{
    /// <summary>
    /// Entity representation of a Legacy Disability Tax.
    /// </summary>
    public class LegacyDisabilityTax : Entity<LegacyDisabilityTax>, ILegacyTaxIdAndType
    {
        public int              DisabilityTaxId        { get; set; }
        public string           Description            { get; set; }
        public string           Code                   { get; set; }
        public int              StateId                { get; set; }
        public DateTime         Modified               { get; set; }
        public string           Modifiedby             { get; set; }
        public string           W2Description          { get; set; }
        public bool             IsW2ElectronicFile     { get; set; }
        public string           W2Code                 { get; set; }
        public bool             IsAddWithState         { get; set; }
        public string           MasterTaxCode          { get; set; }
        public bool             IsShowInBox14          { get; set; }
        public int              DefaultTaxFrequencyId  { get; set; }
        public byte             DefaultCalendarDebitId { get; set; }
        public bool             AddWithSuta            { get; set; }
        public LegacyTaxType    TaxTypeId              { get; set; }
        public bool             BlockOverrides         { get; set; }

        public virtual State    State                  { get; set; }

        // ILegacyTaxIdAndType Properties
        [NotMapped]
        public virtual int TaxId => DisabilityTaxId;
        [NotMapped]
        public virtual LegacyTaxType? LegacyTaxType => TaxTypeId;
    }
}
