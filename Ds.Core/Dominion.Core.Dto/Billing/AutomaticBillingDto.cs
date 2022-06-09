using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Billing
{
    public class AutomaticBillingDto
    {
        public int      AutomaticBillingId       { get; set; }
        public int      FeatureOptionId          { get; set; }
        public BillingItemDescriptionType BillingItemDescriptionId { get; set; }
        public int      Line                     { get; set; }
        public double   Flat                     { get; set; }
        public double   PerQty                   { get; set; }
        public int?     BillingWhatToCountId     { get; set; }
        public DateTime Modified                 { get; set; }
        public int      ModifiedBy               { get; set; }
        public BillingFrequency BillingFrequency { get; set; }

        // FKs
        public BillingItemDescriptionDto BillingItemDescription { get; set; }
    }
}
