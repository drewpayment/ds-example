using System.Collections.Generic;

using Dominion.Core.Dto.Billing;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Billing
{
    /// <summary>
    /// Entity representation of a dbo.BillingPriceChart record.
    /// </summary>
    public partial class BillingPriceChart : Entity<BillingPriceChart>
    {
        public virtual BillingPriceChartType BillingPriceChartId { get; set; } 
        public virtual string                Description         { get; set; } 
        public virtual double?               DiscountPercent     { get; set; } 

        //REVERSE NAVIGATION
        public virtual ICollection<BillingPriceChartDetail> BillingPriceChartDetails { get; set; } 
        public virtual ICollection<BillingItem> BillingItems { get; set; }
    }
}
