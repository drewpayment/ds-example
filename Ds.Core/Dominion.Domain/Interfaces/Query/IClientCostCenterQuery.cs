using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientCostCenterQuery : IQuery<ClientCostCenter, IClientCostCenterQuery>
    {
        IClientCostCenterQuery ByClientId(int clientId);
        IClientCostCenterQuery ByIsActive(bool isActive);
        IClientCostCenterQuery ByScheduleGroupId(int scheduleGroupId);
        IClientCostCenterQuery ByUserSupervisorSecurity(int clientId, int userId);
        IClientCostCenterQuery OrderByDescription();

        /// <summary>
        /// Filters by the default GL cost centers.
        /// </summary>
        /// <returns></returns>
        IClientCostCenterQuery ByIsDefaultGlCostCenter(bool isDefault = true);

        /// <summary>
        /// Filters by one or more particular cost centers.
        /// </summary>
        /// <param name="costCenterIds"></param>
        /// <returns></returns>
        IClientCostCenterQuery ByCostCenter(params int[] costCenterIds);
        IClientCostCenterQuery ExcludeCostCenter(params int[] costCenterIds);

        /// <summary>
        /// Filters by cost center(s) with the same description as the specified ID.
        /// </summary>
        /// <param name="costCenterId">ID of the cost center to compare descriptions to.</param>
        /// <param name="description">Description to compare against.</param>
        /// <returns></returns>
        IClientCostCenterQuery ByHasSameDescriptionOf(int costCenterId, string description);

        /// <summary>
        /// Filters by cost center(s) with the same code as the specified ID.
        /// </summary>
        /// <param name="costCenterId">ID of the cost center to compare codes to.</param>
        /// <param name="code">Code to compare against.</param>
        /// <returns></returns>
        IClientCostCenterQuery ByHasSameCodeOf(int costCenterId, string code);
    }
}
