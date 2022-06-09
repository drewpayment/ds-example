using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Payroll
{
    public class DeductionDeferredComp : Entity<DeductionDeferredComp>
    {
        public virtual int DeductionDeferredCompId { get; set; } 
        public virtual string Description { get; set; } 
        public virtual string Code { get; set; } 
        public virtual bool IsCalcAfterTax { get; set; } 
    }
}
