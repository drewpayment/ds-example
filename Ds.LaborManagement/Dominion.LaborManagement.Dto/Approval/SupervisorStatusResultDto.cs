using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Approval
{
    public class SupervisorStatusResultDto
    {
        public IEnumerable<SupervisorApprovalStatus> Supervisors { get; set; }
        public IEnumerable<EmployeeClockApprovalStatusDto> UnassignedEmployeeStatuses { get; set; }
    }
}
