using System;

namespace Dominion.Core.Dto.Geofence
{
    public partial class ClockEmployeePunchAttemptDto
    {
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public int? ClockEmployeePunchLocationId { get; set; }
        public int? ClockEmployeeBeneiftId { get; set; }
        public int? CostCenterId { get; set; }
        public int? ClientEarningId { get; set; }
        public double? Hours { get; set; }
        public string IpAddress { get; set; }
        public DateTime? EventDate { get; set; }
        public int? ClientShiftId { get; set; }
        public bool? IsApproved { get; set; }
        public int? ClockClientHolidayDetailId { get; set; }
        public bool? IsWorkedHours { get; set; }
        public int? RequestTimeOffDetailId { get; set; }
        public string Subcheck { get; set; }
        public int? EmployeeClientRateId { get; set; }
        public decimal? EmployeeBenefitPay { get; set; }
        public int? ApprovedBy { get; set; }
        public int? DepartmentId { get; set; }
        public int? DivisionId { get; set; }
        public bool IsOutPunch { get; set; }
        public int? PunchTypeId { get; set; }
        public DateTime? OverridePunchTime { get; set; }
        public int? OverrideLunchBreak { get; set; }
        public decimal? InputHours { get; set; }
        public DateTime? InputHoursDate { get; set; }
        public string Comment { get; set; }
        public string EmployeeComment { get; set; }
        public string ClockName { get; set; }
        public int? ClockHardwareId { get; set; }
        public int? JobCostingAssignmentId1 { get; set; }
        public int? JobCostingAssignmentId2 { get; set; }
        public int? JobCostingAssignmentId3 { get; set; }
        public int? JobCostingAssignmentId4 { get; set; }
        public int? JobCostingAssignmentId5 { get; set; }
        public int? JobCostingAssignmentId6 { get; set; }
    }
}