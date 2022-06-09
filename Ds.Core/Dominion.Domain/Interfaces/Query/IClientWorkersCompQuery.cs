using System.Collections.Generic;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientWorkersCompQuery : IQuery<ClientWorkersComp, IClientWorkersCompQuery>
    {
        IClientWorkersCompQuery ByClientId(int clientId);
        IClientWorkersCompQuery ByIsActive(bool isActive);
        IClientWorkersCompQuery ByWorkersCompId(int workersCompId);
        IClientWorkersCompQuery ByWorkersCompIds(IEnumerable<int> workersCompIds);
        IClientWorkersCompQuery OrderByClientWorkersCompId();
        IClientWorkersCompQuery OrderByDescription();
    }
}
