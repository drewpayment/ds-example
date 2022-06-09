using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientDivisionLogoQuery: IQuery<ClientDivisionLogo, IClientDivisionLogoQuery>
    {
        IClientDivisionLogoQuery ByClientDivisionLogoId(int clientDivisionLogoId);
        IClientDivisionLogoQuery ByClientId(int clientId);
    }
}
