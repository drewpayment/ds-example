using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Reporting
{
    public partial class ScheduledReportEmailAddress : Entity<ScheduledReportEmailAddress>, IHasModifiedData
    {
        public virtual int ScheduledReportEmailAddressId { get; set; }
        public virtual string ScheduledReportEmailAddress_ { get; set; }
        public virtual int? ClientId { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }


        // FOREIGN KEYS
        public virtual Client Client { get; set; }
        public virtual ICollection<ScheduledReport> ScheduledReports { get; set; }
    }
}
