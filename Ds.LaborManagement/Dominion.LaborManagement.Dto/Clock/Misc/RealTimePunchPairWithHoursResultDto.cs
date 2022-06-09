using Dominion.LaborManagement.Dto.Scheduling;

namespace Dominion.LaborManagement.Dto.Clock.Misc
{
    public class RealTimePunchPairWithHoursResultDto
    {
        public RealTimePunchPairResultDto punchResult { get; set; }
        public CheckPunchTypeResultDto punchTypeResult { get; set; }
        public ScheduledHoursWorkedResult scheduledHoursResult { get; set; }
    }
}
