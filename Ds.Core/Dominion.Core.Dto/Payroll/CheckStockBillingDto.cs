using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public partial class CheckStockBillingDto
    {
        public int CheckStockBillingId { get; set; }
        public int CheckStockTypeId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            CheckStockBillingDto billing = (CheckStockBillingDto)obj;
            return CheckStockBillingId == billing.CheckStockBillingId;
        }

        public bool Equals(CheckStockBillingDto billing)
        {
            return CheckStockBillingId == billing.CheckStockBillingId;
        }

        public override int GetHashCode()
        {
            return CheckStockBillingId;
        }
    }
}
