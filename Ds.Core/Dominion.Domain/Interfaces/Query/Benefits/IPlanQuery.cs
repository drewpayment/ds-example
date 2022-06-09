using System;
using Dominion.Benefits.Dto.Plans;
using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    /// <summary>
    /// Query on <see cref="Plan"/> data.
    /// </summary>
    public interface IPlanQuery : IQuery<Plan, IPlanQuery>
    {
        /// <summary>
        /// Filters to a single benefit plan.
        /// </summary>
        /// <param name="planId">ID of the plan to get.</param>
        /// <returns></returns>
        IPlanQuery ByPlanId(int planId);

        /// <summary>
        /// Filters plans belonging to the specified client.
        /// </summary>
        /// <param name="clientId">ID of client to filter plans by.</param>
        /// <returns></returns>
        IPlanQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by plans containing the specified resource file/URL.
        /// </summary>
        /// <param name="resourceId">ID of the resource the plan should contain.</param>
        /// <returns></returns>
        IPlanQuery ByResourceId(int resourceId);

        /// <summary>
        /// Filters by plans with the specified active state.
        /// </summary>
        /// <param name="isActive">If true (default), query will only return active plans.</param>
        /// <returns></returns>
        IPlanQuery ByIsActive(bool isActive = true);

        /// <summary>
        /// Filters by plans belonging to a specific open enrollment.
        /// </summary>
        /// <param name="openEnrollmentId">Open enrollment the plan(s) should be associated with.</param>
        /// <returns></returns>
        IPlanQuery ByOpenEnrollmentId(int openEnrollmentId);

        /// <summary>
        /// Filters by plans that are active after a given date (i.e. plan end-date is after the given date).
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        IPlanQuery ByActiveAfterDate(DateTime date);

        /// <summary>
        /// Filters by plan start and end date (used to filter plans by filter control)
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IPlanQuery ByDateRange(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Filters by plans with the specified <see cref="PlanEligibilityType"/>(s).
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        IPlanQuery ByEligibilityType(params PlanEligibilityType[] types);
    }
}
