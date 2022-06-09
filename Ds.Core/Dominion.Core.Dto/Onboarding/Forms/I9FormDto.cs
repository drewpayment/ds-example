using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    public class I9FormDto
    {
        public int       EmployeeId                 { get; set; }
        public string    Employee_FirstName         { get; set; }
        public string    Employee_LastName          { get; set; }
        public string    Employee_MiddleInitial     { get; set; }
        public string    Employee_OtherNames        { get; set; }
        public string    Employee_Address           { get; set; }
        public string    Employee_ApartmentNumber   { get; set; }
        public string    Employee_City              { get; set; }
        public string    Employee_StateAbbreviation { get; set; }
        public string    Employee_Zipcode           { get; set; }
        public DateTime? Employee_BirthDate         { get; set; }
        public string    Employee_SSN1              { get; set; }
        public string    Employee_SSN2              { get; set; }
        public string    Employee_SSN3              { get; set; }
        public string    Employee_EmailAddress      { get; set; }
        public string    Employee_Phone             { get; set; }

        public bool?     IsUsCitizen               { get; set; } 
        public bool?     IsNonCitizenNational      { get; set; }
        public bool?     IsLawfulPermanentResident { get; set; }
        public string    AlienUscisNumber          { get; set; }
        public bool?     IsAuthorizedAlien         { get; set; }
        public DateTime? AlienExpirationDate       { get; set; }
        public string    FormI94AdmissionNumber    { get; set; }
        public string    ForeignPassportNumber     { get; set; }
        public string    CountryOfIssuance         { get; set; }
        public string    CitizenshipStatus        { get; set; }

        //Preparer
        public bool      HasPreparer                { get; set; }
        public bool?      Preparer_Signed            { get; set; }
        public string    Preparer_LastName          { get; set; }
        public string    Preparer_FirstName         { get; set; }
        public string    Preparer_Address           { get; set; }
        public string    Preparer_City              { get; set; }
        public string    Preparer_StateAbbreviation { get; set; }
        public string    Preparer_Zipcode           { get; set; }

        //Section 2 Documents
        public IEnumerable<I9FormDocumentDto> ListADocuments { get; set; }
        public I9FormDocumentDto              ListBDocument  { get; set; }
        public I9FormDocumentDto              ListCDocument  { get; set; }

        //Employer
        public DateTime? Employee_HireDate          { get; set; } 
        public string    Employer_CompanyName       { get; set; }
        public string    Employer_Address           { get; set; }
        public string    Employer_City              { get; set; }
        public string    Employer_StateAbbreviation { get; set; }
        public string    Employer_Zipcode           { get; set; }

        //Reverify
        public string            Reverify_FirstName     { get; set; }
        public string            Reverify_LastName      { get; set; }
        public string            Reverify_MiddleInitial { get; set; }
        public DateTime?         Reverify_RehireDate    { get; set; }
        public I9FormDocumentDto Reverify_Document      { get; set; }

        //Signatures
        public string    Employee_SignatureName          { get; set; }
        public DateTime? Employee_SignatureDate          { get; set; }
        public string    Preparer_SignatureName          { get; set; }
        public DateTime? Preparer_SignatureDate          { get; set; }
        public string    Employer_SignatureName          { get; set; }
        public DateTime? Employer_SignatureDate          { get; set; }
        public string    Employer_SignatureTitle         { get; set; }
        public string    Employer_SignatureFirstName     { get; set; }
        public string    Employer_SignatureLastName      { get; set; }
        public string    Employer_SignatureMiddleInitial { get; set; }
        public string    Reverify_SignatureName          { get; set; }
        public DateTime? Reverify_SignatureDate          { get; set; }
    }
}
