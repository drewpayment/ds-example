using Dominion.Domain.Entities.Reporting;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Reporting
{
    public interface IScheduledReportVisibleQuery : IQuery<ScheduledReportVisible, IScheduledReportVisibleQuery>
    {
        IScheduledReportVisibleQuery ByClientId(int clientId);
        IScheduledReportVisibleQuery ByIsVisible(bool visible);
    }
}