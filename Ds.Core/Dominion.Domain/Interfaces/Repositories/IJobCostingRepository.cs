using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Interfaces.Query;
using Dominion.LaborManagement.Dto.Sproc.JobCosting;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Defines a repository for Client Job Costing and related entities.
    /// </summary>
    public interface IJobCostingRepository : IRepository, IDisposable
    {
        /// <summary>
        /// Gets the list of parent Job Costing IDs for the given job costing.
        /// </summary>
        /// <param name="jobCostingId">The ID of the job costing that the parent ids should be retrieved for.</param>
        /// <returns></returns>
        IEnumerable<ClientJobCostingIdDto> GetParentJobCostingIds(int jobCostingId);

        /// <summary>
        /// Gets the job costing assignments available based on the given parameters.
        /// </summary>
        /// <param name="clientId">ID of client the job costings are associated with.</param>
        /// <param name="employeeId">ID of employee the job costings are associated with.</param>
        /// <param name="clientJobCostingId">ID of the job costing category.</param>
        /// <param name="isEnabled">Indication if the assignment is enabled/available.</param>
        /// <param name="clientJobCostingIds">Comma separated list of previous job costing selection ID's.</param>
        /// <param name="clientJobCostingAssignmentIds">Comma separated list of previous job costing assignment ID's.</param>
        /// <param name="userId">ID of the user requesting the job costings assignments.</param>
        /// <param name="searchText">Text to filter the results by. Will compare on the Description and Code columns.</param>
        /// <returns></returns>
        IEnumerable<ClientJobCostingAssignmentSprocDto> GetEmployeeJobCostingAssignmentList(int clientId, int employeeId, int clientJobCostingId, bool isEnabled, int[] clientJobCostingIds, int[] clientJobCostingAssignmentIds, int userId, string searchText = null);

        IClientJobCostingQuery QueryClientJobCosting();

        IClientJobCostingAssignmentQuery GetJobCostingAssignmentQuery();

        IClientJobCostingAssignmentSelectedQuery GetJobClientJobCostingAssignmentSelectedQuery();
    }
}
