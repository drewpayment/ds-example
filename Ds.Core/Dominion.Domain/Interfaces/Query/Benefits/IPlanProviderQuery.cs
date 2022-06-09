using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    /// <summary>
    /// Query on benefit plan providers (see <see cref="PlanProvider"/>).
    /// </summary>
    public interface IPlanProviderQuery : IQuery<PlanProvider, IPlanProviderQuery>
    {
        /// <summary>
        /// Query providers for a single client.
        /// </summary>
        /// <param name="clientId">ID of client to query by.</param>
        /// <returns></returns>
        IPlanProviderQuery ByClient(int clientId);

        /// <summary>
        /// Query a single provider.
        /// </summary>
        /// <param name="planProviderId">ID of provider.</param>
        /// <returns></returns>
        IPlanProviderQuery ByPlanProvider(int planProviderId);

        /// <summary>
        /// Queries all providers that do NOT have the specified ID.
        /// </summary>
        /// <param name="planProviderId">ID to exclude from the query results.</param>
        /// <returns></returns>
        IPlanProviderQuery ByNotPlanProvider(int planProviderId);

        /// <summary>
        /// Query providers by name.
        /// </summary>
        /// <param name="name">Name of provider to filter by.</param>
        /// <param name="matchFullName">If true, returns only provides that match the full name specified; othwise 
        /// (default), will match any provider containing the specified name.</param>
        /// <returns></returns>
        IPlanProviderQuery ByName(string name, bool matchFullName = false);

        /// <summary>
        /// Queries providers by activity-state.
        /// </summary>
        /// <param name="isActive">If true (default), will only return providers marked as active.</param>
        /// <returns></returns>
        IPlanProviderQuery ByIsActive(bool isActive = true);
    }
}
