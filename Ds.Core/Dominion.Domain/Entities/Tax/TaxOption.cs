using System;
using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Payroll;
using Dominion.Taxes.Dto.TaxOptions;

namespace Dominion.Domain.Entities.Tax
{
    [Serializable]
    public class TaxOption : Entity<TaxOption>, IHasTaxOptionUniqueName
    {
        public virtual int TaxOptionId { get; set; } 
        public virtual string Name { get; set; } 
        public virtual string Description { get; set; } 
        
        public virtual TaxOptionTypes OptionType { get; set; }
        public virtual TaxOptionTypeInfo OptionTypeInfo { get; set; }

        public virtual bool IsIncludedInGrossWages { get; set; } 
        public virtual bool IsIncludedInNetWages { get; set; } 

        public virtual ICollection<TaxOptionRule> Rules { get; set; }

        public virtual ICollection<ClientEarning> ClientEarnings { get; set; }
        public virtual ICollection<ClientDeduction> ClientDeductions { get; set; }

    }
}
