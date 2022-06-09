using System;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Misc
{
    public class DominionContact : Entity<DominionContact>
    {
        public virtual int      DominionContactId { get; set; }
        
        public virtual string   FirstName                  { get; set; }
        public virtual string   MiddleInitial              { get; set; }
        public virtual string   LastName                   { get; set; }
        
        public virtual string   CompanyName                { get; set; }
        public virtual string   TaxpayerName { get; set; }
        
        public virtual string   FederalIdNumber { get; set; }
        
        public virtual string   AddressLine1 { get; set; }
        public virtual string   AddressLine2 { get; set; }
        public virtual string   City { get; set; }
        public virtual int      StateId { get; set; }
        public virtual string   PostalCode                 { get; set; }
        public virtual int      CountryId { get; set; }
        
        public virtual string   Email { get; set; }
        public virtual string   PhoneNumber { get; set; }
        public virtual string   PhoneExtension { get; set; }
        public virtual string   Fax { get; set; }

        public virtual DateTime Modified { get; set; }
        public virtual int      ModifiedBy { get; set; }

        public virtual State State { get; set; }

    }
}
