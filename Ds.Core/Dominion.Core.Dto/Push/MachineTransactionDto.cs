namespace Dominion.Core.Dto.Push
{
    public class MachineTransactionDto : AttLogDto
    {
        public string ClockName { get; set; }
        public string IpAddress { get; set; }
    }
}
