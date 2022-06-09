using System.Collections.Generic;
using Dominion.Domain.Entities.Dashboard;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IDashboardConfigQuery : IQuery<DashboardConfig, IDashboardConfigQuery>
    {
        /// <summary>
        /// Filters dashboard configs by dashboardconfig id
        /// </summary>
        /// <param name="dashboardId"></param>
        /// <returns></returns>
        IDashboardConfigQuery ByDashboardId(int dashboardId);
        /// <summary>
        /// No Filter, return all
        /// </summary>
        /// <returns></returns>
        IDashboardConfigQuery GetAll();
    }
}
