using Dominion.LaborManagement.Dto.Scheduling;

namespace Dominion.LaborManagement.Dto.Clock.Misc
{
    public class RealTimePunchWithHoursResultDto
    {
        public RealTimePunchResultDto punchResult { get; set; }
        public CheckPunchTypeResultDto punchTypeResult { get; set; }
        public ScheduledHoursWorkedResult scheduledHoursResult { get; set; }
    }
}
