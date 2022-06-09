using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Billing
{
    public class BillingItemDto
    {
        public int BillingItemId { get; set; }
        public int ClientId { get; set; }
        public BillingItemDescriptionType BillingItemDescriptionId { get; set; }
        public BillingPriceChartType BillingPriceChartId { get; set; }
        public BillingFrequency BillingFrequency { get; set; }
        public int Line { get; set; }
        public double Flat { get; set; }
        public double PerQty { get; set; }
        public string Comment { get; set; }
        public bool IsOneTime { get; set; }
        public int? PayrollId { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
        public BillingPeriod BillingPeriod { get; set; }
        public Int16? BillingYear { get; set; }
        public bool IsStopDiscount { get; set; }
        public DateTime? StartBilling { get; set; }
        public BillingItemDescriptionDto BillingItemDescription { get; set; }
        public BillingPriceChartDto BillingPriceChart { get; set; }
        public BillingWhatToCount? BillingWhatToCount { get; set; }
        public int? FeatureOptionId { get; set; }
        public IEnumerable<BillingHistoryDetailDto> BillingHistoryDetails { get; set; }
        public string Note { get; set; }
    }
}
