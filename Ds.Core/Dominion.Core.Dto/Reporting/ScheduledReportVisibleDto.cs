using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Reporting
{
    public partial class ScheduledReportVisibleDto
    {
        public int ScheduledReportVisibleId { get; set; }
        public int ClientId { get; set; }
        public int ReportId { get; set; }
        public bool IsVisible { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
    }
}
