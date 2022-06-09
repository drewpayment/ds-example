using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public class CompanyGoalDeleteDto
    {
        public int ParentId { get; set; }
        public int[] KeepIds { get; set; }

        public CompanyGoalDeleteDto()
        {
            KeepIds = new int[0];
        }
    }
}
