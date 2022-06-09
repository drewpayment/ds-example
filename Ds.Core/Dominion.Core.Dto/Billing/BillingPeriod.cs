using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Billing
{
    /// <summary>
    /// This replaces the 'BillingPeriod' table in the db.
    /// </summary>
    public enum BillingPeriod
    {
        None = 0, //A RON NULL
        NextPayroll = 1,
        NextNormalPayroll = 2,
        CreditCarryOver = 3
    }
}
