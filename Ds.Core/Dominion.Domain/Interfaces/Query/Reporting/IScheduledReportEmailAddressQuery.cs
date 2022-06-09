
using Dominion.Domain.Entities.Reporting;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IScheduledReportEmailAddressQuery : IQuery<ScheduledReportEmailAddress, IScheduledReportEmailAddressQuery>
    {
        /// <summary>
        /// Filters saved report configurations by client.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IScheduledReportEmailAddressQuery ByClientId(int clientId);
        IScheduledReportEmailAddressQuery ByScheduledReportEmailAddressId(int scheduledReportEmailAddressId);
        IScheduledReportEmailAddressQuery ByScheduledReportEmailAddress(string scheduledReportEmailAddress_);
        IScheduledReportEmailAddressQuery OrderByEmailAddress();
        IScheduledReportEmailAddressQuery ByUserType(bool isCompanyAdmin);
        IScheduledReportEmailAddressQuery ByDominionLive(bool byDominionLive);
    }
}
