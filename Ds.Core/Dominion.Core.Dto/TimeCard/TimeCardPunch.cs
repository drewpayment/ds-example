using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class TimeCardPunch : IHasCostCenterDivAndDep
    {
        public string Description { get; set; }
        public int PunchId { get; set; }
        public DateTime DateTime { get; set; }
        public int ClockClientLunchId { get; set; }
        public string CostCenter { get; set; }
        public string Division { get; set; }
        public string Department { get; set; }
        public IEnumerable<int> JobCostingAssignIDs { get; set; }
    }
}
