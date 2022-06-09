using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class EmployeeApproveHoursSettings
    {
        public bool HideGrandTotals { get; set; }
        public bool HideWeeklyTotals { get; set; }
        public bool HideDailyTotals { get; set; }
        [Obsolete("Use DefaultDaysFilter instead")]
        public bool ShowAllDays { get; set; }
        [Obsolete("Use Default DaysFilter instead")]
        public bool HideNoActivity { get; set; }

        public int DefaultDaysFilter { get; set; }

        public int? EmployeesPerPage { get; set; }
    }
}
