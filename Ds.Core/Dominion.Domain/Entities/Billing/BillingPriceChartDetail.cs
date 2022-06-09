using Dominion.Core.Dto.Billing;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Billing
{
    /// <summary>
    /// Entity representation of a dbo.BillingPriceChartDetail record.
    /// </summary>
    public partial class BillingPriceChartDetail : Entity<BillingPriceChartDetail>
    {
        public virtual int                   BillingPriceChartDetailId { get; set; } 
        public virtual BillingPriceChartType BillingPriceChartId       { get; set; } 
        public virtual int?                  BeginningCount            { get; set; } 
        public virtual int?                  EndingCount               { get; set; } 
        public virtual double?               Flat                      { get; set; } 
        public virtual double?               PerQty                    { get; set; } 

        //FOREIGN KEYS
        public virtual BillingPriceChart BillingPriceChart { get; set; } 
    }
}