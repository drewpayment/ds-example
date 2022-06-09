using System.Collections.Generic;
using Dominion.Domain.Entities.Core;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Core
{
    public interface IAlertQuery : IQuery<Alert, IAlertQuery>
    {
        IAlertQuery ByClientId(int clientId);
        IAlertQuery ByAlertCategory(int alertCategoryId);
        IAlertQuery ByAlertType(int alertTypeId);
        IAlertQuery BySecurityLevel(int userTypeId);
        IAlertQuery ByExcludeTimeOff();
        IAlertQuery ByAlertIds(IEnumerable<int> alertIds);
        IAlertQuery ByAlertId(int alertId);
        IAlertQuery ByNotExpired();
    }
}
