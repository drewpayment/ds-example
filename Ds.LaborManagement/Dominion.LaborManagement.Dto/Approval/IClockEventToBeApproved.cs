using System;

namespace Dominion.LaborManagement.Dto.Approval
{
    public interface IClockEventToBeApproved
    {
        int EmployeeId { get; }
        int? CostCenterId { get; set; }
        ClockEventToBeApprovedType EventType { get; }
        DateTime EventDate { get; }
    }
}