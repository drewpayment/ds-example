using System;
using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;
using Dominion.Taxes.Dto.TaxTypes;

namespace Dominion.Domain.Entities.Tax
{
    /// <summary>
    /// Entity providing generic information for a particular tax.
    /// </summary>
    [Serializable]
    public partial class Tax : Entity<Tax>
    {
        public virtual int                TaxId              { get; set; }
        public virtual TaxType            TaxType            { get; set; }
        public virtual TaxTypeInfo        TaxTypeInfo        { get; set; }
        public virtual int?               LegacyTaxId        { get; set; }
        public virtual string             Name               { get; set; }
        public virtual int?               StateId            { get; set; }
        public virtual State              State              { get; set; }
        public virtual string             MasterTaxCode      { get; set; }
        public virtual TaxCalculationType TaxCalculationType { get; set; }

        public virtual ICollection<TaxOptionRule>    TaxOptionRules         { get; set; }
        public virtual ICollection<TaxConfiguration> TaxConfigurations      { get; set; } 
        public virtual TaxCalculationTypeInfo        TaxCalculationTypeInfo { get; set; }
    }
}
