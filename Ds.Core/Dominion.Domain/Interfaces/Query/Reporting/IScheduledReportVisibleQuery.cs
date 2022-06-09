using Dominion.Domain.Entities.Reporting;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Reporting
{
    public interface IScheduledReportVisibleQuery : IQuery<ScheduledReportVisible, IScheduledReportVisibleQuery>
    {
        /// <summary>
        /// Filters saved report configurations by client.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IScheduledReportVisibleQuery ByClientId(int clientId);

        IScheduledReportVisibleQuery ByVisibility(bool isSystemAdmin);

        IScheduledReportVisibleQuery ByIsVisible(bool visible);
    }
}
