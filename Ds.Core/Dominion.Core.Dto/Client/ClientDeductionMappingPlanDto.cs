using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Client
{
    public partial class ClientDeductionMappingPlanDto
    {
        public int ClientPlanId { get; set; }
        public int ClientDeductionId { get; set; }

        public ClientPlanDto ClientPlanDto { get; set; }
    }
}
