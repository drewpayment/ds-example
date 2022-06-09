using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Reporting
{
    public partial class ScheduledReportDto
    {
        public int? ScheduledReportId { get; set; }
        public int ScheduledReportEmailAddressId { get; set; }
        public int? ReportId { get; set; }
        public int? FrequencyId { get; set; }
        //public ScheduledReportFrequency? FrequencyId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public int? Copies { get; set; }
        public int? FileFormatId { get; set; }
        //public ScheduledReportFileFormat? FileFormatId { get; set; }
        public int? ClientId { get; set; }
        public byte SequenceNum { get; set; }
        public int? PayrollCheckSeqId { get; set; }
        public int? OrderId { get; set; }
        public int? ScheduledReportDeliveryOptionId { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }

        //REVERSE NAVIGATION
        //public ICollection<ScheduledReportDto> Childs { get; set; } // many-to-one;
        ////Foreign Keys
        //public ScheduledReportDto Parent { get; set; } // many-to-one;
    }
}
