using System;

namespace Dominion.LaborManagement.Dto.Approval
{
    public class EmployeePunchEventToBeApproved : IClockEventToBeApproved
    {
        public int                   EmployeeId    { get; set; }
        public int?                  CostCenterId  { get; set; }
        public ClockEventToBeApprovedType EventType     => ClockEventToBeApprovedType.EmployeePunch;
        public DateTime              EventDate     => ShiftDate ?? ModifiedPunch;
        public DateTime?             ShiftDate     { get; set; }
        public DateTime              ModifiedPunch { get; set; }
        public int                   EmployeePunchId { get; set; }
    }
}