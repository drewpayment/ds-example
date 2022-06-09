using System;
using Dominion.Core.Dto.Billing;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Billing
{
    public partial class AutomaticBilling: Entity<AutomaticBilling>, IHasModifiedData
    {
        public virtual int                        AutomaticBillingId       { get; set; }
        public virtual int                        FeatureOptionId          { get; set; }
        public virtual BillingItemDescriptionType BillingItemDescriptionId { get; set; }
        public virtual int                        Line                     { get; set; }
        public virtual double                     Flat                     { get; set; }
        public virtual double                     PerQty                   { get; set; }
        public virtual int?                       BillingWhatToCountId     { get; set; }
        public virtual DateTime                   Modified                 { get; set; }
        public virtual int                        ModifiedBy               { get; set; }
        public virtual BillingFrequency           BillingFrequency         { get; set; }

        //FOREIGN KEYS
        public virtual BillingItemDescription BillingItemDescription { get; set; }
    }
}
