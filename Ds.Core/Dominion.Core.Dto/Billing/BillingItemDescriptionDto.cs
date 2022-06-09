using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Billing
{
    public class BillingItemDescriptionDto
    {
        public BillingItemDescriptionType BillingItemDescriptionId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string CostCenter { get; set; }
        public double? FlatIncrease { get; set; }
        public double? PerQtyIncrease { get; set; }
        public byte AllowFlatIncreaseFromZero { get; set; }
        public byte AllowPerQtyIncreaseFromZero { get; set; }
        public bool IsActive { get; set; }
    }
}
