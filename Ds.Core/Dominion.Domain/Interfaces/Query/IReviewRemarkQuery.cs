using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IReviewRemarkQuery : IQuery<ReviewRemark, IReviewRemarkQuery>
    {
        //IReviewRemarkQuery ByReviewId(int id);
        //IReviewRemarkQuery ByRemarkId(int remarkId);

    }
}
