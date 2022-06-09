using System;

namespace Dominion.LaborManagement.Dto.Clock
{
    public class ClockEmployeeScheduleListDto
    {
        public int? ClientEmployeeScheduleId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public int? EmployeeId { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ClockClientTimePolicyId { get; set; }
        public int? ClockClientSchedule_ChangeHistory_ChangeId { get; set; }
        public int? ClientId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int? ClientCostCenterId { get; set; }
        public DateTime? Schedule2StartTime { get; set; }
        public DateTime? Schedule2StopTime { get; set; }
        public int? Schedule2ClientCostCenterId { get; set; }
        public int? Schedule2ClientDepartmentId { get; set; }
        public DateTime? Schedule3StartTime { get; set; }
        public DateTime? Schedule3StopTime { get; set; }
        public int? Schedule3ClientCostCenterId { get; set; }
        public int? Schedule3ClientDepartmentId { get; set; }
        public int? GroupScheduleShiftId { get; set; }
        public int? Schedule2GroupScheduleShiftId { get; set; }
        public int? Schedule3GroupScheduleShiftId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }
        public string ZipCode { get; set; }
        public int? CountryId { get; set; }
        public string HomePhone { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string EmployeeNumber { get; set; }
        public string JobTitle { get; set; }
        public string JobClass { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientGroupId { get; set; }
        public int? ClientWorkersCompId { get; set; }
        public bool? IsW2Pension { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? SeparationDate { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public DateTime? RehireDate { get; set; }
        public DateTime? EligibilityDate { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public int? PayStubOption { get; set; }
        public string Notes { get; set; }
        public byte? CostCenterType { get; set; }
        public bool? NewHireDataSent { get; set; }
        public byte? MaritalStatusId { get; set; }
        public int? EEOCRaceId { get; set; }
        public int? EEOCJobCategoryId { get; set; }
        public int? EEOCLocationId { get; set; }
        public string CellPhone { get; set; }
        public int? JobProfileId { get; set; }
        public string PsdCode { get; set; } //This code is not mapped into entity framework
        public bool IsInOnboarding { get; set; }
        public int? CountyId { get; set; }
        public string DepartmentName { get; set; }
        public string Schedule2DepartmentName { get; set; }
        public string Schedule3DepartmentName { get; set; }
        public string Schedule2CostCenterName { get; set; }
        public string Schedule3CostCenterName { get; set; }
    }
}