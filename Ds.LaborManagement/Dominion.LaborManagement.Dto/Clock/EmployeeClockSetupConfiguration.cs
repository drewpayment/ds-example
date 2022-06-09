using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.Clock
{
    public class EmployeeClockSetupConfiguration
    {
        public string BadgeNumber        { get; set; }
        public EmployeeTimePolicyConfiguration TimePolicy { get; set; }
        public IEnumerable<EmployeeRecentPunchInfo> Punches { get; set; }

    }
}