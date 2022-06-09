using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class GetClockEmployeeApproveHoursSettingsDto
    {
        public int ClockEmployeeApproveHoursSettingsID { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public bool HideWeeklyTotals { get; set; }
        public bool HideGrandTotals { get; set; }
        public bool HideNoActivity { get; set; }
        public bool HideActivity { get; set; }
        public bool HideDailyTotals { get; set; }
        public bool ShowAllDays { get; set; }
        public int? DefaultDaysFilter { get; set; }
        public int? EmployeesPerPage { get; set; }
        public int? PayPeriod { get; set; }

    }
}
