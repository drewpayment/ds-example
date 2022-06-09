using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAccountOptionItemQuery : IQuery<AccountOptionItem,IAccountOptionItemQuery>
    {
        IAccountOptionItemQuery ByClientOptionItemId(int clientOptionItemId);
    }
}
