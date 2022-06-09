using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Billing;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Billing
{
    /// <summary>
    /// Entity representation of a dbo.BillingItem record.
    /// </summary>
    public partial class BillingItem : Entity<BillingItem>
    {

        public virtual int BillingItemId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual BillingItemDescriptionType BillingItemDescriptionId { get; set; }
        public virtual BillingPriceChartType BillingPriceChartId { get; set; }
        public virtual int Line { get; set; }
        public virtual double Flat { get; set; }
        public virtual double PerQty { get; set; }
        public virtual BillingWhatToCount? BillingWhatToCount { get; set; }
        public virtual string Comment { get; set; }
        public virtual bool IsOneTime { get; set; }
        public virtual int? PayrollId { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual BillingPeriod BillingPeriod { get; set; }
        public virtual Int16? BillingYear { get; set; }
        public virtual bool IsStopDiscount { get; set; }
        /// <summary>
        /// Determines when a client should start being billed for an item.
        /// </summary>
        public virtual DateTime? StartBilling { get; set; }
        public virtual int? FeatureOptionId { get; set; }
        public virtual string Note { get; set; }

        //FOREIGN KEYS
        public virtual BillingItemDescription BillingItemDescription { get; set; }
        public virtual BillingPriceChart BillingPriceChart { get; set; }
        public virtual BillingFrequency BillingFrequency { get; set; }

        public virtual ICollection<BillingHistoryDetail> BillingHistoryDetails { get; set; }
        public virtual Client Client { get; set; }
    }
}
