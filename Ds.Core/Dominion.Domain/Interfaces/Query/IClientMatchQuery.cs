using System.Collections.Generic;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientMatchQuery : IQuery<ClientMatch,IClientMatchQuery>
    {
        IClientMatchQuery ByClientMatchId(int clientMatchId);
        IClientMatchQuery ByClientId(int clientId);
        IClientMatchQuery ByClientMatchIds(IEnumerable<int> clientMatchIds);
    }
}