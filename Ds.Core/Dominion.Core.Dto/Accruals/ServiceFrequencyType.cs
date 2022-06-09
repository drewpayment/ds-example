using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Accruals
{
    /// <summary>
    /// Per dbo.ServiceFrequency
    /// </summary>
    public enum ServiceFrequencyType
    {
        OneTime      = 1,
        Completed    = 2,
        UpTo         = 3,
        FirstPayroll = 4,
        LifeTime     = 5,
    }
}
