using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Interfaces.Query.Clients
{
    public interface IClientGLCustomClassQuery : IQuery<ClientGLCustomClass, IClientGLCustomClassQuery>
    {
        IClientGLCustomClassQuery ByClientId(int clientId);
        IClientGLCustomClassQuery ByClientDivisionId(int clientDivisionId);
        IClientGLCustomClassQuery ByClientGLCustomClassId(int clientGLCustomClassId);
        IClientGLCustomClassQuery ByClientGroupId(int clientGroupId);
        IClientGLCustomClassQuery ByClientCostCenterId(int clientCostCenterId);
    }
}
