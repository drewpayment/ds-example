using System.Collections.Generic;
using Dominion.Domain.Entities.Banks;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Banks
{
    /// <summary>
    /// Query on <see cref="Bank"/>
    /// </summary>
    public interface IBankQuery : IQuery<Bank, IBankQuery>
    {
        IBankQuery ByBankId(int bankId);
        IBankQuery ByRoutingNumber(string routing);
        IBankQuery ByBankIds(IEnumerable<int> bankIds);
    }
}
