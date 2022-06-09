using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Reporting;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Reporting
{
    public partial class StandardReport
    {
        public virtual StandardReportEnum StandardReportId { get; set; }
        //public virtual int StandardReportId { get; set; }
        public virtual string Name { get; set; }
        public virtual bool IsHasSequencePicker { get; set; }
        public virtual string SqlName { get; set; }
        public virtual bool IsHasOrderPicker { get; set; }
        public virtual bool IsHasRateInformation { get; set; }
        public virtual bool IsReportingOnly { get; set; }
        public virtual bool IsScheduled { get; set; }
        public virtual byte? DefaultSequenceNum { get; set; }
        public virtual bool IsHasSubReport { get; set; }
        public virtual byte EmployeeAccess { get; set; }
        public virtual bool IsVisibleDefault { get; set; }
        public virtual bool IsShownOnPayrollHistory { get; set; }

        public StandardReport()
        {
        }

        // FOREIGN KEYS
        // public virtual ScheduledReportVisible ScheduledReportVisible { get; set; }
    }
}
