using System;

namespace Dominion.LaborManagement.Dto.Clock.Misc
{
    public class RealTimePunchResultDto
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public DateTime PunchTime { get; set; }
        public int? PunchId { get; set; }
        public int? TransferPunchId { get; set; }
        public bool IsDuplicatePunch { get; set; }
    }
}