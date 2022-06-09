using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    /// <summary>
    /// Query on <see cref="PlanOption"/> data.
    /// </summary>
    public interface IPlanOptionQuery : IQuery<PlanOption, IPlanOptionQuery>
    {
        /// <summary>
        /// Filters plan options by a specific benefit plan.
        /// </summary>
        /// <param name="planId">ID of the benefit plan to filter by.</param>
        /// <returns></returns>
        IPlanOptionQuery ByPlanId(int? planId);

        /// <summary>
        /// Filters by one or more specific benefit plan options.
        /// </summary>
        /// <param name="planOptionIds">ID of one or more plan options to filter by.</param>
        /// <returns></returns>
        IPlanOptionQuery ByPlanOptionIds(params int[] planOptionIds);

        /// <summary>
        /// Excludes the specified plan options from the results.
        /// </summary>
        /// <param name="planOptionIds">One or more plan options to exclude.</param>
        /// <returns></returns>
        IPlanOptionQuery ExcludePlanOption(params int[] planOptionIds);

        /// <summary>
        /// Filters plan options by name.
        /// </summary>
        /// <param name="name">Name to filter by.</param>
        /// <param name="matchFullName">If true, will match the full text of the name (case-insensitive). Default
        /// is false, indicating the name shoud contain the specified text.</param>
        /// <returns></returns>
        IPlanOptionQuery ByName(string name, bool matchFullName = false);
    }
}
