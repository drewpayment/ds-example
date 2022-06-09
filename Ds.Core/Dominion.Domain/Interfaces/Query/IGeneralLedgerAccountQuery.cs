using Dominion.Domain.Entities.Accounting;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IGeneralLedgerAccountQuery : IQuery<GeneralLedgerAccount, IGeneralLedgerAccountQuery>
    {
        IGeneralLedgerAccountQuery ByClientId(int clientId);
        IGeneralLedgerAccountQuery ByLedgerId(int ledgerId);
    }
}