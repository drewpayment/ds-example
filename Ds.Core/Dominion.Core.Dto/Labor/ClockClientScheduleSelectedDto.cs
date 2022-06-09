using System;

namespace Dominion.Core.Dto.Labor
{
    public class ClockClientScheduleSelectedDto
    {
        public int EmployeeId { get; set; }
        public int ClockClientScheduleId { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public int ClientId { get; set; }
    }
}
