using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Misc
{
// Country
    public class Country : Entity<Country>
    {
        public virtual int CountryId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Abbreviation { get; set; }

        //REVERSE NAVIGATION
        public virtual ICollection<ClientDivision> ClientDivisions { get; set; } // many-to-one;
    }
}