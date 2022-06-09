using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Clients
{
     /// <summary>
    /// Query on <see cref="ClientBankInfo"/>.
    /// </summary>
    
    public interface IClientBankInfoQuery  : IQuery<ClientBankInfo, IClientBankInfoQuery>
    {
        IClientBankInfoQuery ByClientId(int clientId);
    }
}