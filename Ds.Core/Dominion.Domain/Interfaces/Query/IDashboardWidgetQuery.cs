using System.Collections.Generic;
using Dominion.Domain.Entities.Dashboard;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IDashboardWidgetQuery : IQuery<DashboardWidget, IDashboardWidgetQuery>
    {
        /// <summary>
        /// Filters dashboard widgets by dashboardconfig id
        /// </summary>
        /// <param name="dashboardId"></param>
        /// <returns></returns>
        IDashboardWidgetQuery ByDashboardId(int dashboardId);
        IDashboardWidgetQuery ByDashboardWidgetId(int dashboardWidgetId);
    }
}
