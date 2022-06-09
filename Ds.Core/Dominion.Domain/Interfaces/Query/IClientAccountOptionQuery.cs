using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientAccountOptionQuery : IQuery<ClientAccountOption, IClientAccountOptionQuery>
    {
        IClientAccountOptionQuery ByClientId(int clientId);
        IClientAccountOptionQuery ByOption(AccountOption option);
        IClientAccountOptionQuery ByIsSecurity(bool shouldBeSecurityOption);
        IClientAccountOptionQuery ByOptions(List<AccountOption> options);
    }
}