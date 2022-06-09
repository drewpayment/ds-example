using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Billing
{
    public enum BillingFrequency
    {
        EveryPayroll = 1,
        Monthly = 2,
        Quarterly = 3,
        Annual = 4,
        OddPayrollOut = 5
    }
}
