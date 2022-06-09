using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Clients
{
    public interface IClientBankQuery : IQuery<ClientBank, IClientBankQuery>
    {
        IClientBankQuery ByClientId(int clientId);
        IClientBankQuery ByBankId(int bankId);
        IClientBankQuery OrderByName();
    }
}
