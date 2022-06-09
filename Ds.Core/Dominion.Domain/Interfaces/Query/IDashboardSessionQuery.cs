using System.Collections.Generic;
using Dominion.Domain.Entities.Dashboard;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IDashboardSessionQuery : IQuery<DashboardSession, IDashboardSessionQuery>
    {
        /// <summary>
        /// Filters dashboard widgets by dashboardconfig id
        /// </summary>
        /// <param name="dashboardId"></param>
        /// <returns></returns>
        IDashboardSessionQuery ByUserId(int userId);
    }
}
