using System;

namespace Dominion.Core.Dto.AR
{
    public class ArReportPayrollDto
    {
        public int      PayrollId     { get; set; }
        public int      DisplayOrder  { get; set; }
        public string   CheckDateDesc { get; set; }
        public DateTime CheckDate     { get; set; }
    }
}
