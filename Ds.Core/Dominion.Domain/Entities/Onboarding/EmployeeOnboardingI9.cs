using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Onboarding;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Contact;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Onboarding
{
    public class EmployeeOnboardingI9 : Entity<EmployeeOnboardingI9>, IHasModifiedData
    {
        public virtual int                       EmployeeId                 { get; set; } 
        public string                            OtherName                  { get; set; } 
        public virtual I9EligibilityStatus       I9EligibilityStatusId      { get; set; } 
        public virtual bool                      UsedATranslator            { get; set; } 
        public virtual int?                      TranslatorId               { get; set; } 
        public virtual int?                      TranslatorAddressId        { get; set; } 
        public virtual DateTime?                 TranslatedDate             { get; set; } 
        public virtual string                    PermResidentUscisNumber    { get; set; } 
        public virtual DateTime?                 AuthorizedWorkDate         { get; set; } 
        public virtual bool?                     DoesAuthorizationNotExpire { get; set; } 
        public virtual AlienAdmissionNumberType? AlienAdmissionNumberType   { get; set; } 
        public virtual string                    AlienAdmissionNumber       { get; set; } 
        public virtual string                    ForeignPassportNumber      { get; set; }
        public virtual int?                      PassportCountryId          { get; set; }
        public virtual bool?                     IsAdmissionNumberFromCBP   { get; set; }
        public virtual DateTime?                 CreatedDate                { get; set; } 
        public virtual DateTime?                 SignedDate                 { get; set; }
        public virtual DateTime                  Modified                   { get; set; } 
        public virtual int                       ModifiedBy                 { get; set; } 
        public virtual bool?                     IsTranslatorSigned         { get; set; }

        //FOREIGN KEYS
        public virtual Person            Translator        { get; set; } 
        public virtual Address           TranslatorAddress { get; set; } 
        public virtual Employee.Employee Employee          { get; set; }
        public virtual Country           PassportCountry   { get; set; }

        public virtual ICollection<EmployeeOnboardingI9Document> I9Documents { get; set; }
    }
}