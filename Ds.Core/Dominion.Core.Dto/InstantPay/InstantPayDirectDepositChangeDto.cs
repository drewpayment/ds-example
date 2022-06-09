using System;

namespace Dominion.Core.Dto.InstantPay
{
    public class InstantPayDirectDepositChangeDto
    {
        public int                InstantPayChangeId { get; set; }
        public int                ClientId           { get; set; }
        public int                EmployeeId         { get; set; }
        public InstantPayProvider InstantPayProvider { get; set; }
        public string             AccountOriginator  { get; set; }
        public string             RoutingNumber      { get; set; }
        public string             AccountNumber      { get; set; }
        public bool               IsActive           { get; set; }
        public DateTime           ChangeDate         { get; set; }
    }
}
