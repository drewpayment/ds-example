using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Billing
{
    /// <summary>
    /// Entity representation of a dbo.genBillingDetailHistory record.
    /// </summary>
    public partial class BillingHistoryDetail : Entity<BillingHistoryDetail>
    {
        public virtual int      BillingHistoryDetailId { get; set; } 
        public virtual int      BillingHistoryId       { get; set; } 
        public virtual int?     BillingItemId          { get; set; } 
        public virtual decimal? Quantity               { get; set; } 
        public virtual int?     LineNumber             { get; set; } 
        public virtual double   FlatAmount             { get; set; } 
        public virtual double   PerQuantityAmount      { get; set; } 
        public virtual decimal  TotalAmount            { get; set; } 
        public virtual string   Comment                { get; set; } 
        public virtual bool     IsOneTime              { get; set; } 

        //FOREIGN KEYS
        public virtual BillingHistory BillingHistory { get; set; } 
        public virtual BillingItem    BillingItem    { get; set; }
    }
}