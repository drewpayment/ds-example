using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Taxes.Dto.TaxOptions;

namespace Dominion.Domain.Entities.Tax
{
    [Serializable]
    public class TaxOptionTypeInfo : Entity<TaxOptionTypeInfo>
    {
        public virtual TaxOptionTypes TaxOptionType { get; set; }
        public virtual string Name { get; set; } 
    }
}
