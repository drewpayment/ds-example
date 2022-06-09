using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Reporting
{
    public class GenerateReportDto
    {
        public int    PayrollId    { get; set; }
        public int    ReportTypeId { get; set; }
        public string CheckDate    { get; set; }
        public string AbsoluteUrl  { get; set; }
    }
}
