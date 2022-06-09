using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class BenefitListByDate
    {
        public class Benefit
        {
            public int EmployeeID { get; set; }
            public DateTime EventDate { get; set; }

            public bool IsHoliday { get; set; }

            public string Description { get; set; }

            public int? ClockEmployeePunchID { get; set; }

            public bool IsApproved { get; set; }
            public int ClockEmployeeBenefitID { get; set; }

            public int? RequestTimeOffDetailID { get; set; }
            public int? ApprovedBy { get; set; }
        }

        public IEnumerable<Benefit> Benefits { get; set; }
    }
}
