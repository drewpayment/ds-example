using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class GetClientJobCostingInfoByClientIDResultsDto
    {
        public ICollection<GetClientJobCostingInfoByClientIDDto.table1> results1 { get; set; }
        public ICollection<GetClientJobCostingInfoByClientIDDto.table2> results2 { get; set; }
        public ICollection<GetClientJobCostingInfoByClientIDDto.table3> results3 { get; set; }
    }
}
