using System;

using Dominion.Core.Dto.Contact;

namespace Dominion.Core.Dto.Onboarding
{   
    [Serializable]
    public class EmployeeOnboardingI9Dto
    {
        public int                       EmployeeId                 { get; set; } 
        public string                    OtherName                  { get; set; } 
        public I9EligibilityStatus       I9EligibilityStatusId      { get; set; } 
        public bool                      UsedATranslator            { get; set; } 
        public int?                      TranslatorId               { get; set; } 
        public int?                      TranslatorAddressId        { get; set; } 
        public DateTime?                 TranslatedDate             { get; set; } 
        public string                    PermResidentUscisNumber    { get; set; } 
        public DateTime?                 AuthorizedWorkDate         { get; set; } 
        public bool?                     AuthorizationDoesNotExpire { get; set; } 
        public AlienAdmissionNumberType? AlienAdmissionNumberType   { get; set; } 
        public string                    AlienAdmissionNumber       { get; set; } 
        public DateTime?                 CreatedDate                { get; set; } 
        public DateTime?                 SignedDate                 { get; set; }
        public DateTime                  Modified                   { get; set; } 
        public int                       ModifiedBy                 { get; set; } 
        public int?                      PassportCountryId          { get; set; }
        public string                    ForeignPassportNumber      { get; set; }
        public bool?                     AdmissionNumberFromCBP     { get; set; }
        public bool?                     TranslatorSigned           { get; set; }
        
        public PersonDto Translator { get; set; }
        public AddressDto TranslatorAddress { get; set; }
        
    }
}
