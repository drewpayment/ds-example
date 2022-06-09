using System;
using Dominion.Taxes.Dto;
using System.Collections.Generic;
using Dominion.Core.Dto.Onboarding;

namespace Dominion.Core.Dto.Employee
{
    [Serializable]
    public class AddEmployeeDto
    {
        public AddEmployeeDto()
        {
            this.EmployeeClientRates = new List<EmployeeClientRateDto>();
            this.employeeWorkflow = new List<EmployeeOnboardingWorkflowDto>();
    }

        public int EmployeeId { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public EmployeeStatusType EmployeeStatusId { get; set; }
        public int PayFrequencyId { get; set; }
        public PayType PayTypeId { get; set; }

        public bool IsOtExempt { get; set; }

        public bool? IsTippedEmployee { get; set; }

        public int? ClockClientTimePolicyId { get; set; }

        //public int ClientRateId { get; set; }
        //public double Rate { get; set; }
        public List<EmployeeClientRateDto> EmployeeClientRates { get; set; }

        public double? SalaryAmount { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public TaxDto SutaState { get; set; }
        public int? JobProfileId { get; set; }
        public string JobTitle { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int? ClientCostCenterId { get; set; }
        public int? ClientWorkersCompId { get; set; }
        public int? ClientGroupId { get; set; }
        public int? ClientShiftId { get; set; }
        public int? EEOCJobCategoryId { get; set; }
        public int? EEOCLocationId { get; set; }
        public bool? isFromAddEmployee { get; set; } = false;
        public bool? IsBenefitsEligibility { get; set; }
        public int? BenefitPackageId { get; set; }
        public int? SalaryMethodTypeId { get; set; }
        public string JobClass { get; set; }
        public bool? IsBenefitPortalOn { get; set; }
        public int? CompetencyModelId { get; set; }
        public int? DirectSupervisorId { get; set; }
        public IEnumerable<EmployeeOnboardingWorkflowDto> employeeWorkflow { get; set; }
    }
}
