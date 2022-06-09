using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Reporting
{
    public partial class SavedReportDto
    {
        public int SavedReportId { get; set; }
        public int ClientId { get; set; }
        public ReportType? ReportTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsHideHeaders { get; set; }
        public int? FileFormatId { get; set; }
        //public ScheduledReportFileFormat? FileFormatId { get; set; }
        public int TimeFrameId { get; set; }
        public bool IsGroupPerPage { get; set; }
        public bool? IsFormLayout { get; set; }
        public byte AdjustmentOption { get; set; }
        public bool? IsHideGroupSub { get; set; }
        public int DetailOption { get; set; }
        public int? OwnerUserId { get; set; }
        public bool? IsOwnerViewOnly { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
        public ICollection<SavedReportFieldDto> Fields { get; set; }
    }
}
