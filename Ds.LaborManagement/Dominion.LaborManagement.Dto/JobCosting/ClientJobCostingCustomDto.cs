using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.JobCosting
{
    public class ClientJobCostingCustomDto
    {
        public ClientJobCostingDto ClientJobCosting { get; set; }
        public ICollection<ClientJobCostingDto> ClientJobCostingList { get; set; }

    }
}
