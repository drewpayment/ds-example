using Dominion.Core.Dto.Tax;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Tax.Legacy
{
    /// <summary>
    /// Entity representation of info relating to Legacy Tax Type.
    /// </summary>
    public class LegacyTaxTypeInfo : Entity<LegacyTaxTypeInfo>
    {
        public virtual LegacyTaxType LegacyTaxType { get; set; }
        public virtual string        Description   { get; set; }
        public virtual bool          IsEmployerTax { get; set; }
        public virtual string        TaxGroupCode  { get; set; }
    }
}
