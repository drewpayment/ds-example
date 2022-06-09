using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Reporting;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Reporting
{
    public partial class SavedReport: Entity<SavedReport>, IHasModifiedOptionalData
    { 
        public virtual int SavedReportId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual ReportType ReportTypeId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsHideHeaders { get; set; }
        public virtual int? FileFormatId { get; set; }
        public virtual int TimeFrameId { get; set; }
        public virtual bool IsGroupPerPage { get; set; }
        public virtual bool? IsFormLayout { get; set; }
        public virtual byte AdjustmentOption { get; set; }
        public virtual bool? IsHideGroupSub { get; set; }
        public virtual int DetailOption { get; set; }
        public virtual int? OwnerUserId { get; set; }
        public virtual bool? IsOwnerViewOnly { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual DateTime? Modified { get; set; }

        public ICollection<SavedReportField> Fields { get; set; }

        public SavedReport()
        {
        }

        // FOREIGN KEYS
        public virtual Client Client { get; set; }

        public virtual ICollection<ScheduledReport> ScheduledReports { get; set; }
    }
}
