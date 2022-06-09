using System;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates.DailyPay
{
    public class DailyPayUserRosterLayout
    {
        public string    user_id               { get; set; }
        public string    first_name            { get; set; }
        public string    last_name             { get; set; }
        public string    account_number        { get; set; }
        public string    email                 { get; set; }
        public string    phone_number          { get; set; }
        public string    location              { get; set; }
        public float?    annual_salary         { get; set; }
        public DateTime? employment_start_date { get; set; }
    }
}
