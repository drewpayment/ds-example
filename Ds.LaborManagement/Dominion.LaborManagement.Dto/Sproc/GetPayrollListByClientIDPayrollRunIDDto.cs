using System;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class GetPayrollListByClientIDPayrollRunIDDto
    {
        public int PayrollId { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CheckDate { get; set; }
        public string CheckDateOrder { get; set; }
        public string PayPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
