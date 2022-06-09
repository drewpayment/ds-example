using System;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    public class W4StateALFormDto
    {
        public int EmployeeId { get; set; }
        public string Employee_FirstName { get; set; }
        public string Employee_LastName { get; set; }
        public string Employee_MiddleInitial { get; set; }
        public string Employee_SocialSecurityNumber { get; set; }
        public DateTime? Employee_BirthDate { get; set; }
        public string Employee_DriversLicense { get; set; }
        public string Employee_AddressLine1 { get; set; }
        public string Employee_AddressLine2 { get; set; }
        public string Employee_City { get; set; }
        public string Employee_StateAbbreviation { get; set; }
        public string Employee_Zipcode { get; set; }
        public bool? IsNewEmployee { get; set; }
        public DateTime? NewHireDate { get; set; }
        public int? NumberOfExemptions { get; set; }
        public decimal? AdditionalAmount { get; set; }
        public bool? ReasonA_IsExempt { get; set; }
        public bool? ReasonB_IsExempt { get; set; }
        public bool? ReasonC_IsExempt { get; set; }
        public string ReasonB_Explination { get; set; }
        public string ReasonC_RenaissanceZone { get; set; }
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
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Line5 { get; set; }
        public string Line6 { get; set; }
    }
}
