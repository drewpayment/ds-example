using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Api;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;


namespace Dominion.Domain.Entities.Clients
{
    public class ClientDivisionAddress : Entity<ClientDivisionAddress>
    {
        public virtual int ClientDivisionAddressId { get; set; }
        public virtual int ClientDivisionId { get; set; }
        public virtual int ClientContactId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string City { get; set; }
        public virtual int StateId { get; set; }
        public virtual string Zip { get; set; }
        public virtual int CountryId { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }

        //FK
        public virtual ClientDivision ClientDivision { get; set; }
        public virtual ClientContact ClientContact { get; set; }
        public virtual State State { get; set; }

    }
}
