using Dominion.Domain.Entities.Reporting;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Queries <see cref="SavedReport"/> data.
    /// </summary>
    public interface ISavedReportQuery : IQuery<SavedReport, ISavedReportQuery>
    {
        /// <summary>
        /// Filters saved report configurations by client.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        ISavedReportQuery ByClientId(int clientId);

        ISavedReportQuery WithoutTimeAndAttendence();

    }
}
