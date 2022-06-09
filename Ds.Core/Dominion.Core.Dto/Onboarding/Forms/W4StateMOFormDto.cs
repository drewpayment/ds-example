using System;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    public class W4StateMOFormDto
    {
        public int       EmployeeId                    { get; set; }
        public string    Employee_FirstName            { get; set; }
        public string    Employee_LastName             { get; set; }
        public string    Employee_MiddleInitial        { get; set; }
        public string    Employee_SocialSecurityNumber { get; set; }
        //public DateTime? Employee_BirthDate            { get; set; }
        //public string    Employee_DriversLicense       { get; set; }
        public string    Employee_AddressLine1         { get; set; }
        public string    Employee_AddressLine2         { get; set; }
        public string    Employee_City                 { get; set; }
        public string    Employee_StateAbbreviation    { get; set; }
        public string    Employee_Zipcode              { get; set; }
        public bool      IsSingle                      { get; set; }
        public bool      IsMarried                     { get; set; }
        public bool      IsHeadofHousehold             { get; set; }
        public DateTime? HireDate                      { get; set; }

        public int          Line1                      { get; set; }
        public bool         Line2Checkbox1             { get; set; }
        public bool         Line2Checkbox2             { get; set; }
        public int          Line2                      { get; set; }
        public int?         Line3                      { get; set; }
        public int?         Line4                      { get; set; }
        public int?         Line5                      { get; set; }
        public int?         Line6                      { get; set; }
        public string       Line7                      { get; set; }
        public string       Line8                      { get; set; }
        public string       Line9                       { get; set; }
        public string    SignatureName                 { get; set; }
        public DateTime? SignatureDate                 { get; set; }
        public string    Employer_CompanyName          { get; set; }
        public string    Employer_AddressLine1         { get; set; }
        public string    Employer_AddressLine2         { get; set; }
        public string    Employer_City                 { get; set; }
        public string    Employer_StateAbbreviation    { get; set; }
        public string    Employer_Zipcode              { get; set; }
        public string    Employer_Phone                { get; set; }
        public string    Employer_ContactName          { get; set; }
        public string    Employer_IdentificationNumber { get; set; }
        public string   MOTaxIdentificationNumber      { get; set; }
}
}
