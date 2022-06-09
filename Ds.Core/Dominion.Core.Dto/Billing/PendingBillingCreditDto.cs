using Dominion.Core.Dto.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Billing
{
    public class PendingBillingCreditDto
    {
        public int                        PendingBillingCreditId   { get; set; }
        public int                        ClientId                 { get; set; }
        public BillingItemDescriptionType BillingItemDescriptionId { get; set; }
        public BillingPriceChartType      BillingPriceChartId      { get; set; }
        public BillingFrequency           BillingFrequency         { get; set; }
        public int                        Line                     { get; set; }
        public double                     Flat                     { get; set; }
        public double                     PerQty                   { get; set; }
        public BillingWhatToCount?        BillingWhatToCount       { get; set; }
        public string                     Comment                  { get; set; }
        public bool                       IsOneTime                { get; set; }
        public int?                       PayrollId                { get; set; }
        public BillingPeriod              BillingPeriod            { get; set; }
        public Int16?                     BillingYear              { get; set; }
        public bool                       IsStopDiscount           { get; set; }
        public DateTime?                  StartBilling             { get; set; }
        public int?                       FeatureOptionId          { get; set; }
        public int                        RequestedById            { get; set; }
        public bool                       NeedsApproval            { get; set; }
        public bool                       IsApproved               { get; set; }
        public DateTime                   Modified                 { get; set; }
        public int                        ModifiedBy               { get; set; }
        public string                     RequestedByName          { get; set; }
        public string                     Note                     { get; set; }


        public ClientDto                 Client                 { get; set; }
        public BillingItemDescriptionDto BillingItemDescription { get; set; }
        public BillingPriceChartDto      BillingPriceChart      { get; set; }
    }
}
