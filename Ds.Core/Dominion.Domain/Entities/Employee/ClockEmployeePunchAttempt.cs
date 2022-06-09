using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;

namespace Dominion.Domain.Entities.Employee
{
    public partial class ClockEmployeePunchAttempt : Entity<ClockEmployeePunchAttempt>, IHasModifiedOptionalData
    {
        public int ClockEmployeePunchAttemptID { get; set; }
        public int ClientID { get; set; }
        public int EmployeeID { get; set; }
        public int? ClockEmployeePunchLocationID { get; set; }
        public int? ClientCostCenterID { get; set; }
        public int? ClientDepartmentID { get; set; }
        public int? ClientDivisionID { get; set; }
        public bool IsOutPunch { get; set; }
        public int? PunchTypeID { get; set; }
        public DateTime? OverridePunchTime { get; set; }
        public int? OverrideLunchBreak { get; set; }
        public decimal? InputHours { get; set; }
        public DateTime? InputHoursDate { get; set; }
        public string Comment { get; set; }
        public string EmployeeComment { get; set; }
        public string ClockName { get; set; }
        public int? ClockHardwareId { get; set; }
        public int? ClientJobCostingAssignmentID1 { get; set; }
        public int? ClientJobCostingAssignmentID2 { get; set; }
        public int? ClientJobCostingAssignmentID3 { get; set; }
        public int? ClientJobCostingAssignmentID4 { get; set; }
        public int? ClientJobCostingAssignmentID5 { get; set; }
        public int? ClientJobCostingAssignmentID6 { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
    }
}
