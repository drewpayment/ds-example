using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;
using Dominion.Taxes.Dto.TaxTypes;

namespace Dominion.Domain.Entities.Tax
{
    /// <summary>
    /// Defines a <see cref="Tax"/> rule for a particular <see cref="TaxOption"/>.
    /// </summary>
    [Serializable]
    public class TaxOptionRule : Entity<TaxOptionRule>
    {
        public virtual int         TaxOptionRuleId { get; set; } 
        public virtual int         TaxOptionId     { get; set; } 
        public virtual TaxOption   TaxOption       { get; set; } 
        public virtual int?        TaxId           { get; set; } 
        public virtual Tax         Tax             { get; set; }
        public virtual TaxType?    TaxType         { get; set; } 
        public virtual TaxTypeInfo TaxTypeInfo     { get; set; }
        public virtual int?        StateId         { get; set; } 
        public virtual State       State           { get; set; } 
        public virtual bool        IsTaxable       { get; set; }
    }
}
