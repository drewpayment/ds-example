using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Billing
{
    public class BillingPriceChartDto
    {
        public BillingPriceChartType BillingPriceChartId { get; set; }
        public string Description { get; set; }
        public double? DiscountPercent { get; set; }
    }
}
