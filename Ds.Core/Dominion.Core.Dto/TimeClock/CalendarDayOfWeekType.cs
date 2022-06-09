using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeClock
{
    //Mediachase.Web.UI.WebControls.CalendarDayOfWeek
    //Namespace Mediachase.Web.UI.WebControls
    //    <Flags>
    //    Public Enum CalendarDayOfWeek
    //        Sunday = 1
    //        Monday = 2
    //        Tuesday = 4
    //        Wednesday = 8
    //        Thursday = 16
    //        Friday = 32
    //        Saturday = 64
    //    End Enum
    //End Namespace

    /// <summary>
    /// Copied from <see cref="Mediachase.Web.UI.WebControls.CalendarDayOfWeek"/>,
    /// in order to provide a way to reference that enum within other parts of the project,
    /// and provide conversions between it and <see cref="DayOfWeekType"/> and <see cref="DayOfWeek"/>,
    /// without having to directly reference the legacy <see cref="Mediachase.Web.UI.WebControls.CalendarDayOfWeek"/>
    /// dependency.
    /// </summary>
    [Flags]
    public enum CalendarDayOfWeekType
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Saturday = 64
    }

    public static class CalendarDayOfWeekTypeExtensions
    {
        public static DayOfWeekType ToDayOfWeekType(this CalendarDayOfWeekType calendarDayOfWeekType)
        {
            DayOfWeekType dayOfWeekType;

            switch (calendarDayOfWeekType)
            {
                case CalendarDayOfWeekType.Sunday:
                    dayOfWeekType = DayOfWeekType.Sunday;
                    break;
                case CalendarDayOfWeekType.Monday:
                    dayOfWeekType = DayOfWeekType.Monday;
                    break;
                case CalendarDayOfWeekType.Tuesday:
                    dayOfWeekType = DayOfWeekType.Tuesday;
                    break;
                case CalendarDayOfWeekType.Wednesday:
                    dayOfWeekType = DayOfWeekType.Wednesday;
                    break;
                case CalendarDayOfWeekType.Thursday:
                    dayOfWeekType = DayOfWeekType.Thursday;
                    break;
                case CalendarDayOfWeekType.Friday:
                    dayOfWeekType = DayOfWeekType.Friday;
                    break;
                case CalendarDayOfWeekType.Saturday:
                    dayOfWeekType = DayOfWeekType.Saturday;
                    break;
                default:
                    throw new InvalidCastException();
                    //dayOfWeekType = DayOfWeekType.Sunday;
                    //break;
            }

            return dayOfWeekType;
        }

        public static DayOfWeek ToDayOfWeek(this CalendarDayOfWeekType calendarDayOfWeekType)
        {
            return calendarDayOfWeekType.ToDayOfWeekType().ToDayOfWeek();
        }
    }
}
