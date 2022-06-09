using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Taxes.Dto.TaxTypes;

namespace Dominion.Domain.Entities.Tax
{
    /// <summary>
    /// Detail info for a particular tax calculation method.
    /// </summary>
    public partial class TaxCalculationTypeInfo : Entity<TaxCalculationTypeInfo>
    {
        public virtual TaxCalculationType TaxCalculationTypeId { get; set; }
        public virtual string             Name                 { get; set; }

        /// <summary>
        /// Set of tax entities using this tax calculation method.
        /// </summary>
        public virtual ICollection<Tax> Taxes { get; set; } 
    }
}
