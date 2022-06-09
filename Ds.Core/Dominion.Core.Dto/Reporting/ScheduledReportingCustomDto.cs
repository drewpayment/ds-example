using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Reporting
{
    public partial class ScheduledReportingCustomDto
    {
        //standard dto
        public StandardReportEnum StandardReportId { get; set; }
        //public int StandardReportId { get; set; }
        public bool IsHasSequencePicker { get; set; }
        public string SqlName { get; set; }
        public bool IsHasOrderPicker { get; set; }
        public bool IsHasRateInformation { get; set; }
        public bool IsReportingOnly { get; set; }
        public byte? DefaultSequenceNum { get; set; }
        public bool IsHasSubReport { get; set; }
        public byte EmployeeAccess { get; set; }
        public bool IsVisibleDefault { get; set; }

        //saved dto
        public int SavedReportId { get; set; }
        public int? ClientId { get; set; }
        public ReportType? ReportTypeId { get; set; }
        public int ReportType { get; set; }
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

        //scheduled dto
        public int? ScheduledReportId { get; set; }
        public int ScheduledReportEmailAddressId { get; set; }
        //public StandardReportEnum ReportId { get; set; }
        public int? ReportId { get; set; }
        public int? FrequencyId { get; set; }
        //public ScheduledReportFrequency? FrequencyId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public int? Copies { get; set; }
        public byte SequenceNum { get; set; }
        public int? PayrollCheckSeqId { get; set; }
        public int? OrderId { get; set; }
        public int? ScheduledReportDeliveryOptionId { get; set; }

        //visible dto
        public int ScheduledReportVisibleId { get; set; }
        public bool IsVisible { get; set; }
        public bool isScheduled { get; set; }

        //sub dto sequencing
        public IEnumerable<PayrollCheckSeqDto> SeqList { get; set; }

    }
}
