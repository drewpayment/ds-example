using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Interfaces.Query.Clients
{
    public interface IClientGLClassGroupQuery : IQuery<ClientGLClassGroup, IClientGLClassGroupQuery>
    {
        IClientGLClassGroupQuery ByClientId(int clientId);
        IClientGLClassGroupQuery ByClientGLClassGroupId(int clientGLClassGroupId);
    }
}
