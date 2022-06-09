using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Reporting
{
    public partial class ScheduledReportEmailAddressChangeHistoryDto
    {
        public int ChangeId { get; set; }
        public int ScheduledReportEmailAddressId { get; set; }
        public string ScheduledReportEmailAddress { get; set; }
        public DateTime ChangeDate { get; set; }
        public int? ClientId { get; set; }
        public int? ModifiedBy { get; set; }
        public string ChangeMode { get; set; }
    }
}
