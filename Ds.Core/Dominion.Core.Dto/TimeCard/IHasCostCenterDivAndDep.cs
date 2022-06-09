using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public interface IHasCostCenterDivAndDep
    {
        string CostCenter { get; set; }
        string Division { get; set; }
        string Department { get; set; }
        IEnumerable<int> JobCostingAssignIDs { get; set; }
    }
}
