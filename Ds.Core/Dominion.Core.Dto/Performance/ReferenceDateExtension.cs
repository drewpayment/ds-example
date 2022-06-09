using Dominion.Core.Dto.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public static class ReferenceDateExtension
    {
        public static DateTime CalculateDuration(this DateUnit referenceDate, int duration)
        {
            switch (referenceDate)
            {
                case DateUnit.Day:
                    return DateTime.MinValue.AddDays(duration);
                case DateUnit.Week:
                    return DateTime.MinValue.AddDays(duration * 7);
                case DateUnit.Month:
                    return DateTime.MinValue.AddMonths(duration);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
