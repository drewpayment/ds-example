using System.Collections.Generic;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientPlanQuery : IQuery<ClientPlan, IClientPlanQuery>
    {
        IClientPlanQuery ByClientId(int clientId);
        IClientPlanQuery ByPlanId(int planId);
        IClientPlanQuery ByPlanIds(IEnumerable<int> planIds);
    }
}