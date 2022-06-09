using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.Clients;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query.Clients
{
    public interface IClientGLControlQuery : IQuery<ClientGLControl, IClientGLControlQuery>
    {
        IClientGLControlQuery ByClientId(int clientId);
    }
}
