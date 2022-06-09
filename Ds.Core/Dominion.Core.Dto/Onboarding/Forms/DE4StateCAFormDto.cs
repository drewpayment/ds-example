using System;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    public class DE4StateCAFormDto
    {
        public int EmployeeId { get; set; }
        public string Employee_FirstName { get; set; }
        public string Employee_LastName { get; set; }
        public string Employee_MiddleInitial { get; set; }
        public string Employee_SocialSecurityNumber { get; set; }

        public DateTime? Employee_BirthDate { get; set; }
        public DateTime? Employee_HireDate { get; set; }

        public string Employee_AddressLine1 { get; set; }
        public string Employee_AddressLine2 { get; set; }
        public string Employee_City { get; set; }
        public string Employee_StateAbbreviation { get; set; }
        public string Employee_Zipcode { get; set; }

        public bool? IsTaxExempt { get; set; }
        public int? TaxExemptReasonId { get; set; }
        public string TaxExemptReason { get; set; }

        public int? AdditionalExemptions { get; set; }
        public int? AdditionalWithholdingAmt { get; set; }

        public bool IsSingle { get; set; }
        public bool IsMarried { get; set; }
        public bool IsHeadofHousehold { get; set; }
        public W4MaritalStatus? MaritalStatus { get; set; }


        public int? Age { get; set; }
        public bool? AgeOver65
        {
            get { return this.Age >= 65; }
            private set { }
        }
        public bool? IsSpouseEmployed { get; set; }
        public bool? IsSpouseOver65 { get; set; }
        public bool? IsEmployeeBlind { get; set; }
        public bool? IsSpouseBlind { get; set; }
        public int? WS65You { get; set; }
        public int? WS65Spouse { get; set; }
        public int? WSBlindYou { get; set; }
        public int? WSBlindSpouse { get; set; }
        public int? WSSubtotalAgeBlind { get; set; }

        public bool? IsDependentCareClaim { get; set; }
        public bool? IsClaimSpouse { get; set; }
        public int? WSYourself { get; set; }
        public int? WSSpouse { get; set; }
        public int? WSDependents { get; set; }
        public int? WSSubtotalPersonal { get; set; }
        public int? AdditionalDeduction { get; set; }
        public bool? CompleteExempt { get; set; }
        public string SignatureName { get; set; }
        public DateTime? SignatureDate { get; set; }
        public string Employer_CompanyName { get; set; }
        public string Employer_AddressLine1 { get; set; }
        public string Employer_AddressLine2 { get; set; }
        public string Employer_City { get; set; }
        public string Employer_StateAbbreviation { get; set; }
        public string Employer_Zipcode { get; set; }
        public string Employer_Phone { get; set; }
        public string Employer_ContactName { get; set; }
        public string Employer_IdentificationNumber { get; set; }
        public int? CAEmployerAccountNumber { get; set; }
        public int? Allowances { get; set; }
        public int? EstimatedDeductionAllowances { get; set; }
        public bool? isSingleOrMarriedWith2OrMoreIncome { get; set; }
        public bool? isMarriedWithSingleIncome { get; set; }
    }
}