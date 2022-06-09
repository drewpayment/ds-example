using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.JobCosting
{
    /// <summary>
    /// Defines a DTO that represents a Job Costing that has been associated with an job costing assignment.
    /// </summary>
    public class AssociatedClientJobCostingDto
    {
        /// <summary>
        /// The ID of the Job Costing.
        /// </summary>
        public int ClientJobCostingId { get; set; }
        /// <summary>
        /// The array of IDs of the parent job costings.
        /// </summary>
        public int[] ParentJobCostingIds { get; set; }
        
        /// <summary>
        /// The array of Job Costing Assignment IDs that have been selected for each parent ID listed in <see cref="ParentJobCostingIds"/>.
        /// </summary>
        public int[] ParentJobCostingAssignmentIds { get; set; }
    }
}
