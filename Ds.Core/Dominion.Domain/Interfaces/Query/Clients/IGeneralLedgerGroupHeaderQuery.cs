using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Interfaces.Query.Clients
{
    public interface IGeneralLedgerGroupHeaderQuery : IQuery<GeneralLedgerGroupHeader, IGeneralLedgerGroupHeaderQuery>
    {
        IGeneralLedgerGroupHeaderQuery ByGeneralLedgerGroupId(int generalLedgerGroupId);
    }
}
