using System;
using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.Approval
{
    public class ClockApprovalDayDto
    {
        public int      EmployeeId                 { get; set; }
        public int?     ClockEmployeeApproveDateId { get; set; }
        public DateTime EventDate                  { get; set; }
        public int?     CostCenterId               { get; set; }
        public bool     IsApproved                 { get; set; }
        public List<IClockEventToBeApproved> Events     { get; set; }
    }
}