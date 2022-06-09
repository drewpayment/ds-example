using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.LaborManagement.Dto.JobCosting;
using Dominion.LaborManagement.Dto.Sproc.JobCosting;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    /// <summary>
    /// Defines an interface for objects that are able to _provide_ job costing related calculations and data.
    /// </summary>
    public interface IJobCostingProvider
    {
        /// <summary>
        /// Gets the list of enabled job costings for the client with the given ID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClientJobCostingDto>> GetClientJobCostingList(int clientId);

        /// <summary>
        /// Gets the list of available assignments for the given parameters.
        /// </summary>
        /// <param name="clientId">The ID of the client to search.</param>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <param name="clientJobCostingId">The ID of the job costing to get available assignments for.</param>
        /// <param name="commaSeparatedParentJobCostingIds">The comma separated list of Job Costing IDs that represent the set of parents for the job costing.</param>
        /// <param name="commaSeparatedParentJobCostingAssignmentIds">The comma separated list of Assignment IDs that represent the set of associations for the parents of the job costing.</param>
        /// <param name="searchText">The search for the assignments.</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClientJobCostingAssignmentSprocDto>> GetEmployeeJobCostingAssignmentList(int clientId, int employeeId, int clientJobCostingId, string commaSeparatedParentJobCostingIds, string commaSeparatedParentJobCostingAssignmentIds, string searchText);

        /// <summary>
        /// Gets the list of available assignments for each given job costing.
        /// </summary>
        /// <param name="clientId">The ID of the client to get job costings for.</param>
        /// <param name="employeeId">The ID of the employee to get job costings for.</param>
        /// <param name="jobCostings">The list of DTOs that represent the job costings that the available assignments should be retrieved for.</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClientJobCostingListDto>> GetEmployeeJobCostingAssignments(int clientId, int employeeId, AssociatedClientJobCostingDto[] jobCostings);
    }
}
