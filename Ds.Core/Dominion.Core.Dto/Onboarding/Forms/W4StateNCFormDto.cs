using System;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    public class W4StateNCFormDto
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
        public int? NumberOfExemptions { get; set; }
        public decimal? AdditionalAmount { get; set; }
        public byte? FilingStatus { get; set; }
        public string SignatureName { get; set; }
        public DateTime? SignatureDate { get; set; }
        public bool IsMarriedFilingSeparately { get; set; }
        public bool IsSpouseEmployed { get; set; }
        public string Employee_County { get; set; }
        public string Employee_Country { get; set; }
        public int Employee_CountryId { get; set; }
    }
}
