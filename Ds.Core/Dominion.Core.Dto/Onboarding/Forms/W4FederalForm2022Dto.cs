using System;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    /// <summary>
    /// DTO containing a 1-to-1 mapping to the Federal 2022 W4 form fields.
    /// </summary>
    public class W4FederalForm2022Dto
    {
        public int FormYear { get; set; }
        public int EmployeeId { get; set; }

        public string W4_FirstName { get; set; }
        public string W4_MiddleInitial { get; set; }
        public string W4_LastName { get; set; }
        public string W4_SocialSecurityNumber { get; set; }
        public string W4_HomeAddressLine1 { get; set; }
        public string W4_HomeAddressLine2 { get; set; }
        public string W4_City { get; set; }
        public string W4_StateAbbreviation { get; set; }
        public string W4_Zipcode { get; set; }
        public bool W4_IsExempt { get; set; }
        public bool W4_Withholding_IsSingleOrMarriedFilingSeperately { get; set; }
        public bool W4_Withholding_IsMarriedFilingJointly { get; set; }
        public bool W4_Withholding_IsHeadOfHousehold { get; set; }
        public bool? W4_HasTwoSimilarJobs { get; set; }
        public decimal? W4_QualifyingChildrenAmount { get; set; }
        public decimal? W4_OtherDependentsAmount { get; set; }
        public decimal? W4_TaxCredit { get; set; }
        public decimal? W4_OtherTaxableIncome { get; set; }
        public decimal? W4_WageDeduction { get; set; }
        public decimal? W4_AdditionalAmountToWitholdPerPay { get; set; }
        public DateTime? W4_FirstDateOfEmployment { get; set; }
        public string W4_EmployerNameAndAddress { get; set; }
        public string W4_EmployerIdentificationNumber { get; set; }
        public string SignatureName { get; set; }
        public DateTime? SignatureDate { get; set; }

    }
}
