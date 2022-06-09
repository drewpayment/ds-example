using System;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates.DailyPay
{
    public class DailyPayNetEarningsLayout
    {
        public string   user_id        { get; set; }
        public DateTime pay_period_end { get; set; }
        public DateTime payday         { get; set; }
        public float    net_pay        { get; set; }
        public float    gross_pay      { get; set; }
        public string   account_number { get; set; }
    }
}
