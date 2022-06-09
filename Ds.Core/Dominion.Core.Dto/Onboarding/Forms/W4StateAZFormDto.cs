using System;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    public class W4StateAZFormDto
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
        public bool? IsAdditionalAmountWithheld { get; set; }
        public int? AdditionalWithholdingAmt { get; set; }
        public int? Allowances { get; set; }
        public decimal? TaxableWagesPercentage { get; set; }

        public string SignatureName { get; set; }
        public DateTime? SignatureDate { get; set; }
        
    }
}
