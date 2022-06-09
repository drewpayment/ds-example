using System.Collections.Generic;

using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients
{
    /// <summary>
    /// Entity representation of the dbo.ClientRelation table.
    /// </summary>
    public class ClientOrganization : Entity<ClientOrganization>
    {
        public ClientOrganization()
        {            
        }

        public virtual int    ClientRelationId { get; set; }
        public virtual string Name             { get; set; }

        public virtual ICollection<Client> Clients { get; set; } 
    }
}
