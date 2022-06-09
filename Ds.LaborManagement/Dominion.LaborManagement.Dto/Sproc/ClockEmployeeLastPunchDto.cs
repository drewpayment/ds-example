using System;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class ClockEmployeeLastPunchDto
    {
        public bool IsTransferPunch { get; set; }
        public int? ClockEmployeePunchId { get; set; }
        public int? ClientCostCenterId { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientDepartmentId { get;set; }
        public int? JobCostingAssignment1 { get;set; }
        public int? JobCostingAssignment2 { get;set; }
        public int? JobCostingAssignment3 { get;set; }
        public int? JobCostingAssignment4 { get;set; }
        public int? JobCostingAssignment5 { get;set; }
        public int? JobCostingAssignment6 { get;set; }
        public DateTime ModifiedPunch { get; set; }
        public int? ClockClientLunchId { get; set; }
    }
}