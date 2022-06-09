using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query.Performance
{
    public interface IGoalOptionsQuery : IQuery<GoalOptions, IGoalOptionsQuery>
    {
        IGoalOptionsQuery ByOptionId(int optionId);
        IGoalOptionsQuery ByIsDisabled(bool isDisabled);
    }
}
