using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    /// <summary>
    /// Queries <see cref="BenefitPackage"/> information.
    /// </summary>
    public interface IBenefitPackageQuery : IQuery<BenefitPackage, IBenefitPackageQuery>
    {
        /// <summary>
        /// Filters by packages belonging to the specified client.
        /// </summary>
        /// <param name="clientId">ID of client to get packages for.</param>
        /// <returns></returns>
        IBenefitPackageQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by a specific benefit package.
        /// </summary>
        /// <param name="benefitPackageId">ID of the package to get.</param>
        /// <returns></returns>
        IBenefitPackageQuery ByPackageId(int benefitPackageId);

        /// <summary>
        /// Query benefit packages by name.
        /// </summary>
        /// <param name="name">Name of package to filter by.</param>
        /// <param name="matchFullName">If true, returns only packages that match the full name specified; otherwise 
        /// (default), will match any package containing the specified name.</param>
        /// <returns></returns>
        IBenefitPackageQuery ByName(string name, bool matchFullName = false);

        /// <summary>
        /// Queries all packages that do NOT have the specified ID.
        /// </summary>
        /// <param name="packageId">ID to exclude from the query results.</param>
        /// <returns></returns>
        IBenefitPackageQuery ExcludePackage(int packageId);
    }
}
