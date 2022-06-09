using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;

namespace Dominion.Core.Dto.Payroll
{
    public class ClientPlanDto
    {
        public int    ClientPlanId          { get; set; }
        public int    ClientId              { get; set; }
        public string Description           { get; set; }
        public double Amount                { get; set; }
        public int    DeductionAmountTypeId { get; set; }
        public int    ModifiedBy            { get; set; }
        public bool   Inactive              { get; set; }
        public ICollection<ClientDeductionMappingPlanDto> ClientDeductionMappingPlan { get; set; }

    }
}
