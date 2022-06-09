using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.JobCosting
{
    /// <summary>
    /// Defines a DTO that represents a ClientJobCosting entity.
    /// </summary>
    public class ClientJobCostingDto
    {
        public int ClientJobCostingId { get; set; }
        public int ClientId { get; set; }
        public int JobCostingTypeId { get; set; }
        public string JobCostingTypeDescription { get; set; }
        public bool HideOnScreen { get; set; }
        public bool IsRequired { get; set; }
        public int Sequence { get; set; }
        public int Level { get; set; }
        public int[] ParentJobCostingIds { get; set; } = new int[0];
        public string Description { get; set; }

        //custom
        public ClientJobCostingAssignmentDto jobCostingAssignmentSelection { get; set; }
    }
}
