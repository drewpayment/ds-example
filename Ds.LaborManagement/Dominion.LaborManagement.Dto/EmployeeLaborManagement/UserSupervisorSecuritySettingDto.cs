using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.EmployeeLaborManagement
{
    public class UserSupervisorSecuritySettingDto
    {
        public bool IsAllowEditEmployeeSetup { get; set; }
        public bool IsLimitCostCenters { get; set; }
    }
}
