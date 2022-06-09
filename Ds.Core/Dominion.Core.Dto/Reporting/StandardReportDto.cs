using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Reporting
{
    public partial class StandardReportDto
    {
        public StandardReportEnum StandardReportId { get; set; }
        //public int StandardReportId { get; set; }
        public string Name { get; set; }
        public bool IsHasSequencePicker { get; set; }
        public string SqlName { get; set; }
        public bool IsHasOrderPicker { get; set; }
        public bool IsHasRateInformation { get; set; }
        public bool IsReportingOnly { get; set; }
        public bool IsScheduled { get; set; }
        public byte? DefaultSequenceNum { get; set; }
        public bool IsHasSubReport { get; set; }
        public byte EmployeeAccess { get; set; }
        public bool IsVisibleDefault { get; set; }
        public bool IsShownOnPayrollHistory { get; set; }

        public ScheduledReportVisibleDto ScheduledReportVisible { get; set; }
    }
}
