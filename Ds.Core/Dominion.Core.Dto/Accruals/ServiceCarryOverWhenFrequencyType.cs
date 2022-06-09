using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Accruals
{
    /// <summary>
    /// Per dbo.ServiceCarryOverWhenFrequency
    /// </summary>
    public enum ServiceCarryOverWhenFrequencyType
    {
        AnniversaryYear             = 1,
        FiscalYear                  = 2,
        CalendarYear                = 3,
        FirstOfMonthAnniversaryYear = 4,
        EachPayroll                 = 5,
    }
}
