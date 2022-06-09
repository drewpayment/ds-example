using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Core
{
    public partial class ClientAzure : Entity<ClientAzure>
    {
        public virtual int ClientId { get; set; }
        public virtual string ClientGuId { get; set; }

        public virtual Client Client { get; set; }
    }
}
