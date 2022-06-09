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
    public partial class ScheduledReportVisible : Entity<ScheduledReportVisible>, IHasModifiedData
    {
        public virtual int ScheduledReportVisibleId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int ReportId { get; set; }
        public virtual bool IsVisible { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }

        // FOREIGN KEYS
        public virtual Client Client { get; set; }

        //public virtual StandardReport StandardReport { get; set; }
    }
}
