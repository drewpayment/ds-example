using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dominion.Core.Dto.Tax;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Tax.Legacy
{
    /// <summary>
    /// Entity representation of a Legacy State Tax.
    /// </summary>
    public class LegacyStateTax : Entity<LegacyStateTax>, ILegacyTaxIdAndType
    {
        public int    StateTaxId                { get; set; }
        public string Description               { get; set; }
        public int    StateId                   { get; set; }
        public bool   IsW2ElectronicFile        { get; set; }
        public string MasterTaxCode             { get; set; }
        public string MasterTaxCodeStateFuta    { get; set; }
        public int    DefaultTaxFrequencyId     { get; set; }
        public byte   DefaultCalendarDebitId    { get; set; }
        public bool   BlockOverrides            { get; set; }

        public virtual State State              { get; set; }

        // ILegacyTaxIdAndType Properties
        [NotMapped]
        public virtual int TaxId => StateTaxId;
        [NotMapped]
        public virtual LegacyTaxType? LegacyTaxType => Dominion.Core.Dto.Tax.LegacyTaxType.StateWitholding;
    }
}
