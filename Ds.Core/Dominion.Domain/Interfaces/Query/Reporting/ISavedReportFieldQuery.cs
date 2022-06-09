using System.Collections.Generic;
using Dominion.Domain.Entities.Reporting;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Reporting
{
    public interface ISavedReportFieldQuery : IQuery<SavedReportField, ISavedReportFieldQuery>
    {
        ISavedReportFieldQuery ByReportId(int reportId);
        ISavedReportFieldQuery ByReportIds(IEnumerable<int> reportIds);
    }
}