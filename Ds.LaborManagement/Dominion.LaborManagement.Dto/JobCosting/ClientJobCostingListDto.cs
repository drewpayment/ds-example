using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.LaborManagement.Dto.Sproc.JobCosting;

namespace Dominion.LaborManagement.Dto.JobCosting
{
    public class ClientJobCostingListDto
    {
        public int ClientJobCostingId { get; set; }
        public ClientJobCostingAssignmentSprocDto[] AvailableAssignments { get; set; }
    }
}
