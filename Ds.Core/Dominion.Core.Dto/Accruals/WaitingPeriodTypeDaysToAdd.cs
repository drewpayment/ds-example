using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Accruals
{
    // From spUpdateWaitingPeriodDateByClientAccrualID
    // Days to add
    // WHEN WaitingPeriodTypeID = 0
    // THEN WaitingPeriodValue
    // WHEN WaitingPeriodTypeID = 1
    // THEN WaitingPeriodValue * 7
    // WHEN WaitingPeriodTypeID = 2
    // THEN WaitingPeriodValue * 30
    // WHEN WaitingPeriodTypeID = 3
    // THEN WaitingPeriodValue * 365
    public enum WaitingPeriodTypeDaysToAdd
    {
        Day = 1,
        Week = 7,
        Month = 30,
        Year = 365,
    }
}
