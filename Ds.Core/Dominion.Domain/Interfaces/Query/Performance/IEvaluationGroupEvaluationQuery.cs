using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IEvaluationGroupEvaluationQuery : IQuery<EvaluationGroupEvaluation, IEvaluationGroupEvaluationQuery>
    {
        IEvaluationGroupEvaluationQuery ByEvaluationAndEvalGroup(int evaluationId, int evaluationGroupId);
    }
}
