using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Contact;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Payroll;
using Dominion.Core.Dto.Security;
using Dominion.Taxes.Dto;

namespace Dominion.Core.Dto.Employee
{
    [Serializable]
    public class EmployeeDto2 : CommonContactPersonalAddressDto2
    {
        public DateTime? HireDate { get; set; }
        public virtual EmployeePcpDto PrimaryCarePhysician { get; set; }
    }

    /// <summary>
    /// Represents a DTO that contains basic information about an employee.
    /// </summary>
    /// <remarks>
    /// This DTO contains mostly foreign keys for the employee and was created for 
    /// use with the time clock app 
    /// </remarks>
    public class EmployeeBasicDto
    {
        public int? ClientCostCenterId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public EmployeeStatusType? EmployeeStatusType { get; set; }
    }

    public class EmployeeBasicUserAccessDto : IUserAccessEmployeeInfo
    {

        public virtual int? ClientId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public string NameAndNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }

        public PayType? PayType { get; set; }
        public EmployeeStatusType? EmployeeStatus { get; set; }
        public int? ClientDepartmentId { get; set; }
        public string ClientDepartmentCode { get; set; }
    }

    public class EmployeeBasicPayDto
    {
        public int? ClientTaxId { get; set; }
        public PayFrequencyType? PayFrequencyId { get; set; }
        public PayType? PayTypeId { get; set; }
        public EmployeeStatusType? EmployeeStatusId { get; set; }
        public double? Salary { get; set; }
    }

    public class EmployeeBasicClientRateDto
    {
        public int? ClientRateId { get; set; }
        public double? Rate { get; set; }
        public bool? IsDefaultRate { get; set; }
        public DateTime? RateEffectiveDate { get; set; }
    }

    public class EmployeeBasicStatusDto
    {
        public EmployeeStatusType? EmployeeStatusId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public int EmployeeId { get; set; }
        public int? EmployeeTerminationReasonId { get; set; }
        public RehireEligibleType? RehireEligibleId { get; set; }
    }

    public class EmployeePayDetailDto : EmployeeBasicDto
    {
        public int? UserId { get; set; }
        public bool? IsEmployeeOnboardingEnabled { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public int? JobProfileId { get; set; }
        public string JobTitle { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? RehireDate { get; set; }
        public DateTime? SeparationDate { get; set; }
        public bool? ShouldUpdateJobProfile { get; set; }
        public bool? CanAccessEmployeePay { get; set; }
        public TaxDto SutaState { get; set; }
        public int? DirectSupervisorId { get; set; }

        public EmployeeBasicPayDto EmployeeBasicPayData { get; set; }
        public EmployeeBasicClientRateDto EmployeeBasicClientRateData { get; set; }
    }

    public class EmployeeMultiplePayDetailsDto : EmployeeBasicDto
    {
        public IEnumerable<EmployeeBasicClientRateDto> EmployeeMultipleClientRatesData { get; set; }
        public string ClientCode { get; set; }
        public string CellPhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public EmployeeBasicPayDto PayInfo { get; set; }
    }

    public class EmployeeFullDto : EmployeeBasicDto
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public int? CountyId { get; set; }
        public string PostalCode { get; set; }
        public string HomePhoneNumber { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string JobTitle { get; set; }
        public string JobClass { get; set; }
        public int? ClientGroupId { get; set; }
        public int? ClientWorkersCompId { get; set; }
        public bool? IsW2Pension { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? SeparationDate { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public DateTime? RehireDate { get; set; }
        public DateTime? EligibilityDate { get; set; }
        public bool IsActive { get; set; }
        public string EmailAddress { get; set; }
        public int? PayStubOption { get; set; }
        public string Notes { get; set; }
        public byte CostCenterType { get; set; }
        public string PsdCode { get; set; }
        public bool? IsNewHireDataSent { get; set; }
        public bool IsInOnboarding { get; set; }
        public int? DirectSupervisorId { get; set; }
        public IEnumerable<EmployeeNotesDto> EmployeeNotes { get; set; }
        public EmployeePayDetailDto DirectSupervisor { get; set; }
        public CoreClientDepartmentDto Department { get; set; }
        public CoreClientCostCenterDto CostCenter { get; set; }
        public IEnumerable<EmployeeClientRateDto> Rates { get; set; }
        public EmployeePayDto PayInfo { get; set; }

        /**
         * Checks for valid social security number and determines if it means the extraneous criteria outlined by
         * the SSA for offline partial validation. Rules are:
         * - Cannot contain all zeros in any specific group (e.g. 000-xx-xxxx, xx-00-xxxx)
         * - Cannot begin with '666'
         * - Cannot begin with any value in the range: 900-999
         * - Cannot be 078-05-1120
         * - Cannot be 219-09-9999
         * - Cannot contain all matching values (e.g. 000-00-0000)
         * - Cannot contain incrementing values (e.g. 123-45-6789)
         */
        public bool HasValidSocialSecurityNumber()
        {
            if (SocialSecurityNumber == null || String.IsNullOrWhiteSpace(SocialSecurityNumber))
                return false;

            var rx = new Regex(@"^(?!\b(\d)\1+-(\d)\1+-(\d)\1+\b)(?!123-45-6789|219-09-9999|078-05-1120)(?!666|000|9\d{2})\d{3}-(?!00)\d{2}-(?!0{4})\d{4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return rx.IsMatch(SocialSecurityNumber);
        }
    }

    public class EmployeeNavigatorEmployeeRequiredFields
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? SeparationDate { get; set; }
        public List<string> MissingFields { get; set; } 
    }

    public class EmployeeHireDto
    {
        public int EmployeeId { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? ReHireDate { get; set; }
        public DateTime? SeparationDate { get; set; }
    }
}