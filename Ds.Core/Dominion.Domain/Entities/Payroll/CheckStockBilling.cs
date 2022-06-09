using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class CheckStockBilling : Entity<CheckStockBilling>
    {
        public virtual int CheckStockBillingId { get; set; }
        public virtual int CheckStockTypeId { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal Price { get; set; }

        public CheckStockBilling()
        {
        }
    }
}
