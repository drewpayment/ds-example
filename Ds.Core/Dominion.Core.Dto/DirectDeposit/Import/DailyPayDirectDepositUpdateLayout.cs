using System;

namespace Dominion.Core.Dto.DirectDeposit.Import
{
    public class DailyPayDirectDepositUpdateLayout
    {
        public string   user_id        { get; set; }
        public string   pay_group      { get; set; }
        public DateTime effective_date { get; set; }
        public string   account_type   { get; set; }
        public string   account_number { get; set; }
        public string   routing_number { get; set; }
        public string   location       { get; set; }
    }
}
