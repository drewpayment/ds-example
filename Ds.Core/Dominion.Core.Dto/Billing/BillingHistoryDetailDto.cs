using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Billing
{
    public class BillingHistoryDetailDto
    {
        public int BillingHistoryDetailId { get; set; }
        public int BillingHistoryId { get; set; }
        public int? BillingItemId { get; set; }
        public decimal? Quantity { get; set; }
        public int? LineNumber { get; set; }
        public double FlatAmount { get; set; }
        public double PerQuantityAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Comment { get; set; }
        public bool IsOneTime { get; set; }
        public BillingItemDto BillingItem { get; set; }
    }
}
