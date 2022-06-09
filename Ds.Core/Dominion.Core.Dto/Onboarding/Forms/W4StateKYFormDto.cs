using System;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    public class W4StateKYFormDto
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

        public int? AdditionalExemptions { get; set; }
        public int? AdditionalWithholdingAmt { get; set; }

        public string SignatureName { get; set; }
        public DateTime? SignatureDate { get; set; }

        public int? ReciprocalStateId { get; set; }
        public string ReciprocalStateCode { get; set; }
    }
}