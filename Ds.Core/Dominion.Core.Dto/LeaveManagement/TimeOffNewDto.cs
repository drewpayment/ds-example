using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.LeaveManagement
{
    public class TimeOffNewDto
    {
        public string PolicyName { get; set; }
        public double? StartingUnits { get; set; }
        public double? UnitsAvailable { get; set; }
        public double? PendingUnits { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? OriginalRequestDate { get; set; }
        public virtual TimeOffStatusType Status { get; set; }
        public DateTime? DateOfEvent { get; set; }
    }
}
