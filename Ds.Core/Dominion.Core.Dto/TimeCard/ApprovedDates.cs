using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class ApprovedDates
    {
        public class Day
        {
            public int? ClientCostCenterID;
            public int EmployeeID { get; set; }
            public DateTime EventDate { get; set; }
            public bool IsApproved { get; set; }

            public string ApprovingUser { get; set; }
        }
        public IEnumerable<Day> Days { get; set; }
    }
}
