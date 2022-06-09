using System;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    public class W4StateOHFormDto
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

        public string SchoolDistrictCode { get; set; }
        public string SchoolDistrictName { get; set; }
        public int? PersonalExemption { get; set; }
        public int? PersonalExemptionForSpouse { get; set; }
        public int? DependentCount { get; set; }
        public int? Allowances { get; set; }
        public int? AdditionalWithHolding { get; set; }
        public int? PersonalExemptionForSpouse_1 { get; set; }
        public string SignatureName { get; set; }
        public DateTime? SignatureDate { get; set; }
        public TaxExemptReason? TaxExemptionReasonId { get; set; }
    }
}
