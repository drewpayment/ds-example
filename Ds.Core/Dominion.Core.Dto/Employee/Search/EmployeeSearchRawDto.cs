using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Performance;
using Dominion.Core.Dto.Security;

namespace Dominion.Core.Dto.Employee.Search
{
    public class EmployeeSearchRawDto : IUserAccessEmployeeInfo
    {
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public string EmployeeNumber { get; set; }
        public string ClockBadgeNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public int? CostCenterId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public string ClientDepartmentCode { get; set; }
        public int? DivisionId { get; set; }
        public int? GroupId { get; set; }
        public int? ShiftId { get; set; }
        public int? JobProfileId { get; set; }
        public EmployeeStatusType? EmployeeStatus { get; set; }
        public PayType? PayType { get; set; }
        public bool IsTemp { get; set; }
        public bool IsActive { get; set; }
        public string Ssn { get; set; }
        public string HomePhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? ReHireDate { get; set; }
        public DateTime? SeparationDate { get; set; }
        public string EmailAddress { get; set; }
        public int? TimePolicyId { get; set; }
        public string DirectSupervisor { get; set; }
        public int? DirectSupervisorId { get; set; }
        public int? CompetencyModelId { get; set; }
        public int? UserId { get; set; }
        public IEnumerable<ReviewTemplateSearchDto> ReviewCycleReviews { get; set; }

        public EmployeeProfileImageDto ProfileImage { get; set; }
    }
}