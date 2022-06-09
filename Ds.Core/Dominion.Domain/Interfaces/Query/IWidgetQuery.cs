using System.Collections.Generic;
using Dominion.Domain.Entities.Dashboard;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IWidgetQuery : IQuery<Widget, IWidgetQuery>
    {
        /// <summary>
        /// Filters dashboard configs by dashboardconfig id
        /// </summary>
        /// <param name="dashboardId"></param>
        /// <returns></returns>
        IWidgetQuery ByWidgetId(int widgetId);
        /// <summary>
        /// No Filter, return all
        /// </summary>
        /// <returns></returns>
        //IDashboardConfigQuery GetAll();
    }
}
