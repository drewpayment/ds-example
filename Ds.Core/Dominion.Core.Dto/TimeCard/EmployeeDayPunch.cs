using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class EmployeeDayPunch : EmployeeDayRecord
    {
        public DateTime Time { get; set; }
        public int? ClockEmployeePunchID { get; set; }
        public int? ClockEmployeeLunchID { get; set; }
        public bool IsRequestedPunch { get; set; }

        public int? TimeZoneID { get; set; }

        public string ClockName { get; set; }

        public EmployeeDayJobCostingInfo JobCostingInfo { get; set; } = new EmployeeDayJobCostingInfo();
    }
}
