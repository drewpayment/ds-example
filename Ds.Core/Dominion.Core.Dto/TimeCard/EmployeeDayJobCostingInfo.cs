using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class EmployeeDayJobCostingInfo
    {
        public virtual IEnumerable<int> AssignmentIds { get; set; } = Enumerable.Empty<int>();
        public string CostCenter { get; set; }
        public string Division { get; set; }
        public string Department { get; set; }
    }
}
