using System.Collections.Generic;

using Dominion.Benefits.Dto.Plans;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    public partial class CostFactor : Entity<CostFactor>
    {
        public virtual int                   CostFactorId       { get; set; } 
        public virtual CostFactorValueType   ValueTypeId        { get; set; } 
        public virtual SystemCostFactorType? SystemFactorTypeId { get; set; } 
        public virtual string                Name               { get; set; } 

        //REVERSE NAVIGATION
        public virtual ICollection<CostFactorOption> CostFactorOptions { get; set; } // many-to-one;
    }
}
