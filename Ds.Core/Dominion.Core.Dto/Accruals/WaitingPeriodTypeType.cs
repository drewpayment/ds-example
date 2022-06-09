using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Accruals
{
    public enum WaitingPeriodTypeType
    {
        Days   = 0,
        Weeks  = 1,
        Months = 2,
        Years  = 3,
    }

    public static class WaitingPeriodTypeTypeExtensions
    {
        public static int CalculateDaysToAdd(WaitingPeriodTypeType type, int value)
        {
            int scalar;
            // emulates spUpdateWaitingPeriodDateByClientAccrualID
            switch (type)
            {
                case WaitingPeriodTypeType.Days:    scalar = 1;   break;
                case WaitingPeriodTypeType.Weeks:   scalar = 7;   break;
                case WaitingPeriodTypeType.Months:  scalar = 30;  break;
                case WaitingPeriodTypeType.Years:   scalar = 365; break;
                default:                            scalar = 1;   break; // Treat the same as days apparently?
            }

            return scalar * value;

            //WaitingPeriodLookup = new Dictionary<WaitingPeriodTypeType, int> { 
            //                                    { WaitingPeriodTypeType.Days, 1 },
            //                                    { WaitingPeriodTypeType.Weeks, 7 },
            //                                    { WaitingPeriodTypeType.Months, 30 },
            //                                    { WaitingPeriodTypeType.Years, 365 },
            //                                }.ToLookup(o => o.Key, o => o.Value);
        }
    }
}
