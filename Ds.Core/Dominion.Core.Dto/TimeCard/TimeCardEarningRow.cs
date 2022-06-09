using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class TimeCardEarningRow : IHasCostCenterDivAndDep
    {
        public string Description { get; set; }
        public string EarningDate { get; set; }
        public string Hours { get; set; }
        public int BenefitId { get; set; }
        public int CostCenterId { get; set; }
        public int PunchOption { get; set; }
        public string Notes { get; set; }
        public string Exceptions { get; set; }
        public bool IsBenefit { get; set; }
        public bool CanBeApproved { get; set; }
        public bool IsApproved { get; set; }
        public int ApprovedBy { get; set; }
        public string PeriodEnded { get; set; }
        public string CostCenter { get; set; }
        public string Division { get; set; }
        public string Department { get; set; }
        public IEnumerable<int> JobCostingAssignIDs { get; set; }
    }
}
