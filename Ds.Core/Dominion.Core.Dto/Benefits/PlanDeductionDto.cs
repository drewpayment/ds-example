using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Benefits
{
    public class PlanDeductionDto
    {
        public int ClientPlanID { get; set; }
        public int ClientID { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int DeductionAmountTypeID { get; set; }
        public int ModifiedBy { get; set; }
        public bool isActive { get; set; }
        public string DeductionAmountType { get; set; }
    }
}
