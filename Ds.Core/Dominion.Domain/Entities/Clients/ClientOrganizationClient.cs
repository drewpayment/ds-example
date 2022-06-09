using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientOrganizationClient : Entity<ClientOrganizationClient>
    {
        public virtual int ClientRelationId { get; set; }
        public virtual int ClientId { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
        public virtual ClientOrganization ClientRelation { get; set; }

    }
}
