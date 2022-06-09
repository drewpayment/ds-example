using Dominion.Core.Dto.Labor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.EmployeeLaborManagement
{
    public class EmployeeTimePolicyInfoDto
    {
        public int ClientId { get; set; }
        public int TimePolicyId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual ClockClientRulesDto PunchRules { get; set; }
    }
}
