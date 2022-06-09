namespace Dominion.LaborManagement.Dto.Clock.Misc
{
    /// <summary>
    /// Class used to return the result of a real time punch pair request.
    /// </summary>
    public class RealTimePunchPairResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public RealTimePunchResult First { get; set; }
        public RealTimePunchResult Second { get; set; }

        public RealTimePunchPairResultDto ToDto() => new RealTimePunchPairResultDto()
        {
            Message = Message,
            Succeeded = Succeeded,
            First = First?.ToDto(),
            Second = Second?.ToDto()
        };
    }
}