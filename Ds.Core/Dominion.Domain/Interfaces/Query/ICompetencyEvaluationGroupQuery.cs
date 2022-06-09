using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query
{
    public interface ICompetencyEvaluationGroupQuery : IQuery<CompetencyEvaluationGroup, ICompetencyEvaluationGroupQuery>
    {
        ICompetencyEvaluationGroupQuery ByCompetencyGroupId(int compGroupId);
        ICompetencyEvaluationGroupQuery ByEvalGroupId(int evalGroupId);
        ICompetencyEvaluationGroupQuery ByRootId(int rootId);

        ICompetencyEvaluationGroupQuery ByParent(int parentId);
    }
}
