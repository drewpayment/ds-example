using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;
namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IApprovalProcessHistoryQuery : IQuery<ApprovalProcessHistory, IApprovalProcessHistoryQuery>
    {
        IApprovalProcessHistoryQuery ByEvaluationId(int evaluationId);
    }
}
