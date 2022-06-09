using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientJobCostingAssignmentQuery : IQuery<ClientJobCostingAssignment, IClientJobCostingAssignmentQuery>
    {
        /// <summary>
        /// Filters assignments by client Id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClientJobCostingAssignmentQuery ByClientId(int clientId);

        /// <summary>
        /// Filters assignments by whether or not they are enabled.  The default value is true
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        IClientJobCostingAssignmentQuery ByIsEnabled(bool isEnabled = true);

        /// <summary>
        /// Filters assignments by client Job Costing Assignment Id
        /// </summary>
        /// <param name="clientJobCostingAssignmentId"></param>
        /// <returns></returns>
        IClientJobCostingAssignmentQuery ByClientJobCostingAssignmentId(int clientJobCostingAssignmentId);

        IClientJobCostingAssignmentQuery ByClientJobCostingParents(int[] parentJobCostingIds);
    }
}