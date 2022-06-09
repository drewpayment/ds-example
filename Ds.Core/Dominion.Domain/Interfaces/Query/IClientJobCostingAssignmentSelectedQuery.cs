using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientJobCostingAssignmentSelectedQuery : IQuery<ClientJobCostingAssignmentSelected,
        IClientJobCostingAssignmentSelectedQuery>
    {

        /// <summary>
        /// Filters by client id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClientJobCostingAssignmentSelectedQuery ByClientId(int clientId);

        /// <summary>
        /// Filters selected assignmnents by whether or not they are enabled.  The default value is true;
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        IClientJobCostingAssignmentSelectedQuery ByIsEnabled(bool isEnabled = true);

        IClientJobCostingAssignmentSelectedQuery BySelectedJobCostings(int[] parentJobCostingIds);
    }
}