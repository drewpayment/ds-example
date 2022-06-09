namespace Dominion.Core.Dto.Banks
{
    public class BankAccountNextCheckDto
    {
        public int    BankAccountNextCheckId { get; set; }
        public string RoutingNumber          { get; set; }
        public string AccountNumber          { get; set; }
        public int    NextCheck              { get; set; }
        public int?   ClientId               { get; set; }
    }
}
