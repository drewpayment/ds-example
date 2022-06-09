using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Api;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;


namespace Dominion.Domain.Entities.Clients
{
    public class ClientDivisionLogo : Entity<ClientDivisionLogo>
    {
        public virtual int ClientDivisionLogoId { get; set; }
        public virtual int ClientDivisionId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual byte[] DivisionLogo { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }

    }
}

