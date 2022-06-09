using System;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates.DailyPay
{
    public class DailyPayGrossEarningsLayout
    {
        public string    user_id          { get; set; }
        public double     shift_earnings   { get; set; }
        public DateTime  shift_date       { get; set; }
        // public string    pay_group        { get; set; }
        public double?    shift_hours      { get; set; }
        public double?    pay_rate         { get; set; }
        public DateTime? shift_started_at { get; set; }
        public DateTime? shift_ended_at   { get; set; }
    }
}
