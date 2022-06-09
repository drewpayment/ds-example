using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface ICompetencyOptionsQuery : IQuery<CompetencyOptions, ICompetencyOptionsQuery>
    {
        ICompetencyOptionsQuery ByOptionId(int optionId);
        ICompetencyOptionsQuery ByIsDisabled(bool isDisabled);
    }
}
