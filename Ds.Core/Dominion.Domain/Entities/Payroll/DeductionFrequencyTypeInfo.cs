using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Tax;
using Dominion.Pay.Dto.Deductions;

namespace Dominion.Domain.Entities.Payroll
{
    public class DeductionFrequencyTypeInfo : Entity<DeductionFrequencyTypeInfo>
    {
        public virtual DeductionFrequencyType DeductionFrequencyType { get; set; } 
        public virtual string Name { get; set; } 
    }
}
