using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Client;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients
{
    public partial class ClientTurboTax : Entity<ClientTurboTax>
    {
        public virtual int ClientTurboTaxId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int TaxYear { get; set; }
        public virtual ClientTurboTaxFileStatus TurboTaxFileStatus { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }

        public virtual Client Client { get; set; }
    }
}
