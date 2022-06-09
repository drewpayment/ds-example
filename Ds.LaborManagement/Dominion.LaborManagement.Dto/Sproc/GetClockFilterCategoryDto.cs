using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class GetClockFilterCategoryDto
    {
        public int ClockFilterID { get; set; }

        public string Description { get; set; }

        public string WhereClause { get; set; }

        public int idx { get; set; }

        public string value { get; set; }


    }
}
