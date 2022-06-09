using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Accruals
{
    /// <summary>
    /// Per dbo.ServiceCarryOverFrequency
    /// </summary>
    public enum ServiceCarryOverFrequencyType
    {
        Rate         = 1,
        Flat         = 2,
        BalanceLimit = 3,
    }
}
