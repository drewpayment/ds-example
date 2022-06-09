using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Query.Reporting;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IReportRepository
    {
        ISavedReportQuery SavedReportQuery();
        IScheduledReportEmailAddressQuery ScheduledReportEmailAddressQuery();
        IScheduledReportQuery ScheduledReportQuery();
        IScheduledReportVisibleQuery ScheduledReportVisibleQuery();
        IStandardReportQuery StandardReportQuery();
        IReportQueueQuery ReportQueueQuery();
        IScheduledReportEmailAddressChangeHistoryQuery ScheduledReportEmailAddressChangeHistoryQuery();
        ISavedReportCustomFieldQuery SavedReportCustomFieldQuery();
        IFileFormatQuery FileFormatQuery();
        IDashboardWidgetQuery DashboardWidgetQuery();
        IDashboardConfigQuery DashboardConfigQuery();
        IWidgetQuery WidgetQuery();
        IDashboardSessionQuery DashboardSessionQuery();
        IReportHoldingQuery ReportHoldingQuery();
    }
}
