using System;

namespace Dominion.LaborManagement.Dto.Clock.Misc
{
    /// <summary>
    /// Class used for return the result of a Real Time Punch Request
    /// </summary>
    public class RealTimePunchResult : PunchResult
    {
        public bool Succeeded { get; set; }
        public bool IsDuplicatePunch { get; set; }
        public override string Message { get; set; }
        public DateTime PunchTime { get; set; }
        public override int? PunchId { get; set; }
        public int? TransferPunchId { get; set; }

        public RealTimePunchResultDto ToDto()
        {
            return new RealTimePunchResultDto()
            {
                Message = Message,
                PunchTime = PunchTime,
                Succeeded = Succeeded,
                PunchId = PunchId,
                TransferPunchId = TransferPunchId,
                IsDuplicatePunch = IsDuplicatePunch
            };
        }
    }
}