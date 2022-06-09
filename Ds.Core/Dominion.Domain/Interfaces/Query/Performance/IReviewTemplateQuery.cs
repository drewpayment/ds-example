using System;
using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IReviewTemplateQuery : IQuery<ReviewTemplate, IReviewTemplateQuery>
    {
        IReviewTemplateQuery ByReviewTemplateId(int reviewTemplateId);
        IReviewTemplateQuery ByClientId(int clientId);
        IReviewTemplateQuery ByReviewProfileId(int reviewProfileId);
        IReviewTemplateQuery ByEvaluationPeriodEnd(DateTime evaluationEnd);
        IReviewTemplateQuery ByEvaluationPeriodStart(DateTime evaluationStart);
        IReviewTemplateQuery ByProcessStartDates(DateTime processStartDate);
        IReviewTemplateQuery ByPayrollRequestEffectiveDate(DateTime effectiveDate);
        IReviewTemplateQuery ByReferenceDate(ReferenceDate referenceDate);
        IReviewTemplateQuery ExcludeArchived();
    }
}
