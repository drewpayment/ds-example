using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Reporting
{
    public partial class ScheduledReport : Entity<ScheduledReport>, IHasModifiedData
    {
        public virtual int? ScheduledReportId { get; set; }
        public virtual int ScheduledReportEmailAddressId { get; set; }
        public virtual int? ReportId { get; set; }
        public virtual int? FrequencyId { get; set; }
        public virtual int? DeliveryMethodId { get; set; }
        public virtual int? Copies { get; set; }
        public virtual int? FileFormatId { get; set; }
        public virtual int? ClientId { get; set; }
        public virtual byte SequenceNum { get; set; }
        public virtual int? PayrollCheckSeqId { get; set; }
        public virtual int? OrderId { get; set; }
        public virtual int? ScheduledReportDeliveryOptionId { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }

        public ScheduledReport()
        {
        }
        //REVERSE NAVIGATION
        //public virtual ICollection<ScheduledReport> Childs { get; set; } // many-to-one;
        ////Foreign Keys
        //public virtual ScheduledReport Parent { get; set; } // many-to-one;
        public virtual ScheduledReportEmailAddress ScheduledReportEmailAddress { get; set; }

        public virtual SavedReport CustomReport { get; set; }
        public virtual Client Client { get; set; }
    }
}
