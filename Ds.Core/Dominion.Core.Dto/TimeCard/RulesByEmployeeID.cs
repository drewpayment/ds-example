using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class RulesByEmployeeID
    {
        public class Rule
        {
            public int? ClockAllocateHoursFrequencyID { get; set; }
            public int? WeeklyStartingDayOfWeekID { get; set; }
            public string Name { get; set; }
            public int ClientID { get; set; }
            public int? PunchOption { get; set; }
            public bool ApplyHoursOption { get; set; }
            public int EmployeeID { get; set; }
            public int? ClockClientExceptionID { get; set; }
            public int NoPunchesOnScheduledDayException { get; set; }
            public int NoPunchesBeforeHoliday { get; set; }
            public int NoPunchesAfterHoliday { get; set; }
            public int NoPunchesOnScheduledDayBeforeHoliday { get; set; }
            public int NoPunchesOnScheduledDayAfterHoliday { get; set; }
            public int PunchesOnBenefitDay { get; set; }
            public bool ShowHoursInHundreths { get; set; }

            public int? ClockAllocateHoursOptionID { get; set; }

            public bool HideMultipleSchedules { get; set; }

            public bool EditPunches { get; set; }

            public bool EditBenefits { get; set; }
        }

        public IEnumerable<Rule> Rules { get; set; }
        public DataTable Table { get; set; }
    }
}
