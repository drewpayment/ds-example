using System.Collections.Generic;

using Dominion.Core.Dto.Billing;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Billing
{
    /// <summary>
    /// Entity representation of a dbo.BillingItemDescription record.
    /// </summary>
    public partial class BillingItemDescription : Entity<BillingItemDescription>
    {
        public virtual BillingItemDescriptionType BillingItemDescriptionId    { get; set; } 
        public virtual string                     Code                        { get; set; } 
        public virtual string                     Description                 { get; set; } 
        public virtual string                     CostCenter                  { get; set; } 
        public virtual double?                    FlatIncrease                { get; set; } 
        public virtual double?                    PerQtyIncrease              { get; set; } 
        public virtual byte                       AllowFlatIncreaseFromZero   { get; set; } 
        public virtual byte                       AllowPerQtyIncreaseFromZero { get; set; } 
        public virtual bool                       IsActive                    { get; set; } 

        //REVERSE NAVIGATION
        public virtual ICollection<BillingItem> BillingItems { get; set; } // many-to-one;
    }
}
