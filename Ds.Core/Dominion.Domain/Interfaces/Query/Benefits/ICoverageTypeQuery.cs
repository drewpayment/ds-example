using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    public interface ICoverageTypeQuery : IQuery<CoverageType, ICoverageTypeQuery>
    {
        /// <summary>
        /// Filters by one or more specific coverage types.
        /// </summary>
        /// <param name="coverageTypeIds">ID(s) of the coverage type(s) to filter by.</param>
        /// <returns></returns>
        ICoverageTypeQuery ByCoverageTypes(params int[] coverageTypeIds);

        /// <summary>
        /// Filters by coverage types associated with the specified plan type.
        /// </summary>
        /// <param name="planTypeId"></param>
        /// <returns></returns>
        ICoverageTypeQuery ByPlanType(int planTypeId);

        /// <summary>
        /// Filters by coverage types with the specified employee-grouping settings.
        /// </summary>
        /// <param name="isEmployeeGrouping">If true (default), will only include coverage types 
        /// used to group plans in ESS (i.e. are visible to the employee).</param>
        /// <returns></returns>
        ICoverageTypeQuery ByIsEmployeeGrouping(bool isEmployeeGrouping = true);
    }
}
