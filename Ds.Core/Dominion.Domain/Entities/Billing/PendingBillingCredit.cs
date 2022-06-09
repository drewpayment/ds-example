using Dominion.Core.Dto.Billing;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Billing
{
    public class PendingBillingCredit : Entity<PendingBillingCredit>, IHasModifiedData
    {
        public virtual int                        PendingBillingCreditId   { get; set; }
        public virtual int                        ClientId                 { get; set; }
        public virtual BillingItemDescriptionType BillingItemDescriptionId { get; set; }
        public virtual BillingPriceChartType      BillingPriceChartId      { get; set; }
        public virtual BillingFrequency           BillingFrequency         { get; set; }
        public virtual int                        Line                     { get; set; }
        public virtual double                     Flat                     { get; set; }
        public virtual double                     PerQty                   { get; set; }
        public virtual BillingWhatToCount?        BillingWhatToCount       { get; set; }
        public virtual string                     Comment                  { get; set; }
        public virtual bool                       IsOneTime                { get; set; }
        public virtual int?                       PayrollId                { get; set; }
        public virtual BillingPeriod              BillingPeriod            { get; set; }
        public virtual Int16?                     BillingYear              { get; set; }
        public virtual bool                       IsStopDiscount           { get; set; }
        public virtual DateTime?                  StartBilling             { get; set; }
        public virtual int?                       FeatureOptionId          { get; set; }
        public virtual int                        RequestedById            { get; set; }
        public virtual bool                       NeedsApproval            { get; set; }
        public virtual bool                       IsApproved               { get; set; }
        public virtual DateTime                   Modified                 { get; set; }
        public virtual int                        ModifiedBy               { get; set; }
        public virtual string                     Note                     { get; set; }


        public virtual Client                 Client                 { get; set; }
        public virtual BillingItemDescription BillingItemDescription { get; set; }
        public virtual BillingPriceChart      BillingPriceChart      { get; set; }
        public virtual User.User              RequestedBy            { get; set; }
    }
}
