using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Reporting
{
    public partial class ScheduledReportEmailAddressDto
    {
        public int ScheduledReportEmailAddressId { get; set; }
        public string ScheduledReportEmailAddress_ { get; set; }
        public int? ClientId { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }

        public IEnumerable<ScheduledReportingCustomDto> ReportList { get; set; }
    }
}
