using Dominion.Domain.Entities.AR;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.AR
{
    public interface IArReportQuery : IQuery<ArReport, IArReportQuery>
    {
        IArReportQuery ByArReportId(int arReportId);
    }
}
