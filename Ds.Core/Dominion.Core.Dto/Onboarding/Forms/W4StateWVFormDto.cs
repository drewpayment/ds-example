using System;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    public class W4StateWVFormDto
    {
        public int EmployeeId { get; set; }
        public string Employee_FirstName { get; set; }
        public string Employee_LastName { get; set; }
        public string Employee_MiddleInitial { get; set; }
        public string Employee_SocialSecurityNumber { get; set; }
        public string Employee_AddressLine1 { get; set; }
        public string Employee_AddressLine2 { get; set; }
        public string Employee_City { get; set; }
        public string Employee_StateAbbreviation { get; set; }
        public string Employee_Zipcode { get; set; }

        public bool? IsTaxExempt { get; set; }
        public int? TaxExemptReasonId { get; set; }
        public string TaxExemptReason { get; set; }

        public int? ClaimingSingle { get; set; }
        public int? ExemptionForMarried { get; set; }
        public int? AdditionalExemptions { get; set; }
        public int? TotalExemptions { get; set; }

        public int? AdditionalWithholdingAmt { get; set; }
        public bool? WithheldTaxAtLowerRate { get; set; }
        public string SignatureName { get; set; }
        public DateTime? SignatureDate { get; set; }

        public string NonRes_Employee_FirstName { get; set; }
        public string NonRes_Employee_LastName { get; set; }
        public string NonRes_Employee_MiddleInitial { get; set; }
        public string NonRes_Employee_SocialSecurityNumber { get; set; }
        public string NonRes_Employee_AddressLine1 { get; set; }
        public string NonRes_Employee_AddressLine2 { get; set; }
        public string NonRes_Employee_City { get; set; }
        public string NonRes_Employee_StateAbbreviation { get; set; }
        public string NonRes_Employee_Zipcode { get; set; }
        public string NonRes_SignatureName { get; set; }
        public DateTime? NonRes_SignatureDate { get; set; }
        public byte? DependentCount { get; set; }

    }
}