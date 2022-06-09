namespace Dominion.LaborManagement.Dto.Clock.Misc
{
    public class RealTimePunchPairResultDto
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public RealTimePunchResultDto First { get; set; }
        public RealTimePunchResultDto Second { get; set; }

    }
}