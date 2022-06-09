using Dominion.Domain.Entities.Reporting;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Reporting
{
    public interface IReportHoldingQuery : IQuery<ReportHolding, IReportHoldingQuery>
    {
        IReportHoldingQuery ByUniqueId(string uniqueId);
    }
}
