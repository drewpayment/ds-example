using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClient401KSetupQuery : IQuery<Client401KSetup, IClient401KSetupQuery>
    {
        IClient401KSetupQuery ByClient(int clientId);
        IClient401KSetupQuery ByClientIds(params int[] clientIds);
        IClient401KSetupQuery ByClient401KProviderId(int Client401KProviderId);
    }
}
