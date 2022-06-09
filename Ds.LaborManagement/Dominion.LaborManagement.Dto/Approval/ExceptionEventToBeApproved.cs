using System;
using Dominion.Core.Dto.Labor;
using Dominion.LaborManagement.Dto.Clock;

namespace Dominion.LaborManagement.Dto.Approval
{
    public class ExceptionEventToBeApproved : IClockEventToBeApproved
    {
        public int                   EmployeeId                      { get; set; }
        public int?                  CostCenterId                    { get; set; }
        public ClockEventToBeApprovedType EventType                       => ClockEventToBeApprovedType.Exception;
        public DateTime              EventDate                       { get; set; }
        public int                   ClockEmployeeExceptionHistoryId { get; set; }
        public ClockExceptionType?   ClockExceptionTypeId            { get; set; }
        public int?                  ClockEmployeePunchId            { get; set; }
    }
}