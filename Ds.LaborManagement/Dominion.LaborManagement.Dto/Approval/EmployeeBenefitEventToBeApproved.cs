using System;

namespace Dominion.LaborManagement.Dto.Approval
{
    public class EmployeeBenefitEventToBeApproved : IClockEventToBeApproved
    {
        public int EmployeeBenefitId { get; set; }
        public int                   EmployeeId   { get; set; }
        public int?                  CostCenterId { get; set; }
        public ClockEventToBeApprovedType EventType    => ClockEventToBeApprovedType.EmployeeBenefit;
        public DateTime              EventDate    { get; set; }
    }
}