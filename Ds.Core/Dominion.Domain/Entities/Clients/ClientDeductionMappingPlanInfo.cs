using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Payroll;

namespace Dominion.Domain.Entities.Clients
{
    public partial class ClientDeductionMappingPlanInfo : Entity<ClientDeductionMappingPlanInfo>
    {
        public virtual int ClientPlanId { get; set; }
        public virtual int ClientDeductionId { get; set; }
        public virtual ClientPlan ClientPlan { get; set; }
        public virtual ClientDeduction ClientDeduction { get; set; }
    }
}
