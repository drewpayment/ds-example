using Dominion.Utility.Query;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Interfaces.Query.Clients
{
    public interface IGeneralLedgerTypeQuery : IQuery<GeneralLedgerType, IGeneralLedgerTypeQuery>
    {
        IGeneralLedgerTypeQuery ByGeneralLedgerTypeId(int generalLedgerTypeId);
    }
}
