using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.Clock
{
    public class EmployeeTimePolicyConfiguration
    {
        public int ClockClientTimePolicyId { get; set; }
        public string Name { get; set; }

        public EmployeeTimePolicyRuleConfiguration Rules { get; set; }
        public IEnumerable<EmployeeTimePolicyLunchConfiguration> Lunches { get; set; }
    }
}