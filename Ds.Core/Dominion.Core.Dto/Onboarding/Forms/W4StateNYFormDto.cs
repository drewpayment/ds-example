using System;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    public class W4StateNYFormDto
    {
        public int EmployeeId { get; set; }
        public string Employee_FirstName { get; set; }
        public string Employee_LastName { get; set; }
        public string Employee_MiddleInitial { get; set; }
        public string Employee_SocialSecurityNumber { get; set; }

        public DateTime? Employee_BirthDate { get; set; }
        public DateTime? Employee_HireDate { get; set; }

        public DateTime? Employee_RehireDate { get; set; }

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

        public double AllowableDeductionsToFedAdjGrossIncome { get; set; }
        public double IncomeNotSubjectToWitholding { get; set; }
        public double EstimatedItemizedDeductions { get; set; }
        public bool IsMarriedFilingSeparately { get; set; }
        public int? TotalExemptions { get; set; }
        public int? InstructionAOptionFromChart { get; set; }

        //StateW4 NY Changes - ON-841
        public bool? IsResidentOfNewYorkCity { get; set; }
        public bool? IsAdditionalNYCAmountWithheld { get; set; }
        public int? NYCAdditionalWithholdingAmt { get; set; }
        public int? NYCAllowances { get; set; }

        public bool? IsResidentOfYonkers { get; set; }
        public bool? IsAdditionalYonkersAmountWithheld { get; set; }
        public int? YonkersAdditionalWithholdingAmt { get; set; }

        public TaxExemptReason? TaxExemptionReasonId { get; set; }


    }
}