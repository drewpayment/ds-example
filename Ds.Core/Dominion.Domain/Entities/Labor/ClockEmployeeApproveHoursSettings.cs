using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Labor
{
    public class ClockEmployeeApproveHoursSettings : Entity<ClockEmployeeApproveHoursSettings>
    {
        public int ClockEmployeeApproveHoursSettingsId { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public bool? HideWeeklyTotals { get; set; }
        public bool? HideGrandTotals { get; set; }
        public bool HideNoActivity { get; set; }
        public bool HideActivity { get; set; }
        public bool HideDailyTotals { get; set; }
        public bool ShowAllDays { get; set; }
        public int? DefaultDaysFilter { get; set; }
        public int? EmployeesPerPage { get; set; }
        public int? PayPeriod { get; set; }
    }
}
