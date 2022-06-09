using Dominion.Core.Dto.LeaveManagement;
using System;

namespace Dominion.LaborManagement.Dto.Approval
{
    public class TimeOffEventToBeApproved : IClockEventToBeApproved
    {
        public int                   EmployeeId             { get; set; }
        public int?                  CostCenterId           { get; set; }
        public ClockEventToBeApprovedType EventType         => ClockEventToBeApprovedType.RequestTimeOff;
        public DateTime              EventDate              => RequestDate;
        public int                   TimeOffRequestDetailId { get; set; }
        public int                   TimeOffRequestId       { get; set; }
        public DateTime              RequestDate            { get; set; }
        public TimeOffStatusType     Status                 { get; set; }
    }
}