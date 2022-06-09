using Dominion.Domain.Entities.Reporting;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Reporting
{
    public interface ISavedReportQuery : IQuery<SavedReport, ISavedReportQuery>
    {
        ISavedReportQuery ByClientId(int clientId);
    }
}