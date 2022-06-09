using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class EmployeeDayPendingBenefit
    {
        public double Hours { get; set; }
        public int Status { get; set; }
        public int RequestTimeOffID { get; set; }
        public string Description { get; set; }
    }

}
