using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.LeaveManagement
{
    public class TimeOffRequestDetailDto
    {
        public int TimeOffRequestDetailId { get; set; }
        public int TimeOffRequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public double Hours { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }

        // Relationships
        public Employee.EmployeeAccrualDto Employee { get; set; }
        public ClientAccrualDto Client { get; set; }
        public TimeOffRequestDto TimeOffRequest { get; set; }
    }
}
