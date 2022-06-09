using System;
using Dominion.Core.Dto.AR;
using Dominion.Domain.Entities.AR;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.AR
{
    public interface IArDepositQuery : IQuery<ArDeposit, IArDepositQuery>
    {
        IArDepositQuery ByArDepositId(int arDepositId);

        IArDepositQuery ByPostedDateRange(DateTime startDate, DateTime endDate);

        IArDepositQuery ByType(string type);
        IArDepositQuery WithNoPostedDate();
    }
}
