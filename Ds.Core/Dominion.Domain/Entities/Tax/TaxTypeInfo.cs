using System;
using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Taxes.Dto.TaxTypes;

namespace Dominion.Domain.Entities.Tax
{
    /// <summary>
    /// Provides details regarding a particular <see cref="TaxType"/>.
    /// </summary>
    [Serializable]
    public class TaxTypeInfo : Entity<TaxTypeInfo>
    {
        public virtual TaxType          TaxType             { get; set; }
        public virtual string           Name                { get; set; }
        public virtual TaxCategory    TaxCategory         { get; set; }
        public virtual TaxCategoryInfo  TaxCategoryInfo     { get; set; }
        public virtual bool             IsEmployeeTax       { get; set; }
        public virtual bool             IsEmployerTax       { get; set; }

        public virtual ICollection<Tax> Taxes { get; set; }
    }
}
