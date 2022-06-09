using System;

namespace Dominion.LaborManagement.Dto.Clock.Misc
{
    public class CheckPunchAllowedResult
    {
        public bool IsAllowed { get; set; }
        public DateTime PunchTime { get; set; }
        public DateTime StartTime { get; set; } 
        public DateTime StopTime { get; set; }
        public int ClockClientScheduleChangeHistoryId {get; set; }
        public string Message { get; set; }
    }
}