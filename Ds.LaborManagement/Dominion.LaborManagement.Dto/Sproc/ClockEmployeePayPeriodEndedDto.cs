using System;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class ClockEmployeePayPeriodEndedDto
    {
        public int? EmployeeId { get; set; }
        public DateTime? PeriodEnded { get; set; }
        public DateTime? PeriodStartLocked { get; set; }
        public string WarningMessageLocked { get; set; }
        public string WarningMessageClosed { get; set; }
        public int AllowScheduleEdits { get; set; }
    }
}