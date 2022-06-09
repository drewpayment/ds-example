using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Labor
{
    public class ClockOvertimeFrequency : Entity<ClockOvertimeFrequency>
    {
        public byte ClockOvertimeFrequencyId { get; set; }
        public string OvertimeFrequency { get; set; }
    }
}
