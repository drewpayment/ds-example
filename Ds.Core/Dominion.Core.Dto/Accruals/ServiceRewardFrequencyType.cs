using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Accruals
{
    /// <summary>
    /// Per dbo.ServiceRewardFrequency
    /// </summary>
    public enum ServiceRewardFrequencyType
    {
        RateMax = 1,
        Flat    = 2,
        RateMin = 3,
    }
}
