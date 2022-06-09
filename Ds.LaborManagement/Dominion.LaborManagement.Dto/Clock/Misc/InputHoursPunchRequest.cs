using Dominion.LaborManagement.Dto.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Clock.Misc
{
    public class InputHoursPunchRequest
    {
        public string IpAddress { get; set; }
        public ClockEmployeeBenefitDto Data { get; set; }
        public RealTimePunchLocation PunchLocation { get; set; }
    }

    public class InputHoursPunchRequestResult : PunchResult
    {
        public ClockEmployeeBenefitDto Data { get; set; }
    }

    public class InputHoursPunchResultWithDetail : InputHoursPunchRequestResult
    {
        public CheckPunchTypeResultDto PunchTypeResult { get; set; }
        public ScheduledHoursWorkedResult ScheduledHoursResult { get; set; }
    }
}
