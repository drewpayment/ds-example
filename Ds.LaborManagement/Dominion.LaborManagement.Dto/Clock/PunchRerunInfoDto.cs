using System;

namespace Dominion.LaborManagement.Dto.Clock
{
    public class PunchRerunInfoDto
    {
        public int       ClockEmployeePunchId { get; set; }
        public int       EmployeeId           { get; set; }
        public int       ClientId             { get; set; }
        public DateTime  ModifiedPunch        { get; set; }
        public DateTime? ShiftDate            { get; set; }
        public DateTime  RawPunch             { get; set; }
        public bool?     IsApproved           { get; set; }
    }
}