using System;

namespace Dominion.Core.Dto.LeaveManagement
{
    public class ClientAccrualProratedScheduleDto
    {
        public int       ClientAccrualProratedScheduleId { get; set; }
        public int       ClientId                        { get; set; }
        public int       ClientAccrualId                 { get; set; }
        public DateTime  ScheduleFrom                    { get; set; }
        public DateTime  ScheduleTo                      { get; set; }
        public int?      AwardAfterDaysValue             { get; set; }
        public DateTime? AwardOnDate                     { get; set; }
        public decimal   AwardValue                      { get; set; }
        public DateTime  Modified                        { get; set; }
        public int       ModifiedBy                      { get; set; }
    }
}
