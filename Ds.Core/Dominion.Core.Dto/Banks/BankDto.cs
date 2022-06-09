namespace Dominion.Core.Dto.Banks
{
    public class BankDto : BankBasicDto
    {
        public string Address       { get; set; }
        public string CheckSequence { get; set; }
        public int?   AchBankId     { get; set; }
    }
}