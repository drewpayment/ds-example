using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Taxes.Dto.TaxOptions;
using Dominion.Taxes.Dto.TaxRates;

namespace Dominion.Domain.Entities.Tax
{
    [Serializable]
    public class TaxRateTypeInfo : Entity<TaxRateTypeInfo>
    {
        public virtual TaxRateType TaxRateType { get; set; } 
        public virtual string Name { get; set; } 
    }
}
