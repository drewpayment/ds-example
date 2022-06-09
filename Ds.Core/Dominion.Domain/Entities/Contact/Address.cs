using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.Contact
{


    /// <summary>
    /// Entity for a person in the contact schema
    /// </summary>
    public class Address : Entity<Address>
    {
        public virtual int    AddressId { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string City { get; set; }
        public virtual int    StateId { get; set; }
        public virtual int    CountryId { get; set; }
        public virtual int?   CountyId { get; set; }
        public virtual string ZipCode { get; set; }

        //FOREIGN KEYS
        public virtual State   State { get; set; }
        public virtual Country Country { get; set; }
        public virtual County  County { get; set; }

    }
}