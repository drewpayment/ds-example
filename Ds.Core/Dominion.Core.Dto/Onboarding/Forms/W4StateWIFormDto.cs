using System;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    public class W4StateWIFormDto
    {
        public int EmployeeId { get; set; }
        public string Employee_FirstName { get; set; }
        public string Employee_LastName { get; set; }
        public string Employee_MiddleInitial { get; set; }
        public string Employee_SocialSecurityNumber { get; set; }
        public DateTime? Employee_BirthDate { get; set; }
        public DateTime? Employee_HireDate { get; set; }
        public int? Age { get; set; }
        public bool IsSingle { get; set; }
        public bool IsMarried { get; set; }
        public bool IsHeadofHousehold { get; set; }
        //public W4MaritalStatus? MaritalStatus { get; set; }
        public string Employee_AddressLine1 { get; set; }
        public string Employee_AddressLine2 { get; set; }
        public string Employee_City { get; set; }
        public string Employee_StateAbbreviation { get; set; }
        public string Employee_Zipcode { get; set; }
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
    }
}