using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Accruals
{
    /// <summary>
    /// Per dbo.ServiceRenewFrequency
    /// </summary>
    public enum ServiceRenewFrequencyType
    {
        Payroll          = 1,
        AnniversaryYear  = 3,
        CalendarMonth    = 4,
        CalendarYear     = 5,
        FiscalYear       = 6,
        AnniversaryMonth = 7,
        Quarterly        = 8,
        LastOfMonth      = 9,
        EverySixMonths   = 22,
        FirstOfMonthAnniversaryYear = 23,
    }
}
