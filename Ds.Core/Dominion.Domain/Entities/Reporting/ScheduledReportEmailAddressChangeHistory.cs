using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Reporting
{
    public partial class ScheduledReportEmailAddressChangeHistory : Entity<ScheduledReportEmailAddressChangeHistory>
    {
        public virtual int ChangeId { get; set; }
        public virtual int ScheduledReportEmailAddressId { get; set; }
        public virtual string ScheduledReportEmailAddress { get; set; }
        public virtual DateTime ChangeDate { get; set; }
        public virtual int? ClientId { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual string ChangeMode { get; set; }
    }
}
