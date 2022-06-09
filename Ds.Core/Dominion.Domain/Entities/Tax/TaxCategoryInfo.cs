using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Taxes.Dto.TaxTypes;

namespace Dominion.Domain.Entities.Tax
{
    [Serializable]
    public class TaxCategoryInfo : Entity<TaxCategoryInfo>
    {
        public virtual TaxCategory TaxCategory { get; set; } 
        public virtual string Name { get; set; } 

        //public virtual ICollection<TaxTypeInfo> TaxTypes { get; set; }
    }
}
