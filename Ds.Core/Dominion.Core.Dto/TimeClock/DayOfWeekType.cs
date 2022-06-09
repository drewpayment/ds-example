using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeClock
{
    public enum DayOfWeekType
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 3,
        Wednesday = 4,
        Thursday = 5,
        Friday = 6,
        Saturday = 7
    }

    public class DayOfWeekDto
    {
        public int DayOfWeekId { get; set; }
        public string Name { get; set; }

        public DayOfWeekDto()
        {
        }

        public DayOfWeekDto(DayOfWeekType dayOfWeekType)
        {
            this.DayOfWeekId = (int)dayOfWeekType;
            this.Name = dayOfWeekType.ToString();
        }

        public static List<DayOfWeekDto> GetDayOfWeekDtoList()
        {
            var daysOfWeekList = new List<DayOfWeekDto>
            {
                new DayOfWeekDto(DayOfWeekType.Sunday),
                new DayOfWeekDto(DayOfWeekType.Monday),
                new DayOfWeekDto(DayOfWeekType.Tuesday),
                new DayOfWeekDto(DayOfWeekType.Wednesday),
                new DayOfWeekDto(DayOfWeekType.Thursday),
                new DayOfWeekDto(DayOfWeekType.Friday),
                new DayOfWeekDto(DayOfWeekType.Saturday)
            };
            return daysOfWeekList;
        }
    }

    public static class DayOfWeekTypeExtensions
    {
        public static DayOfWeekType FromByteAsDayOfWeekType(byte dayOfWeekTypeByte)
        {
            DayOfWeekType startingDayOfWeekType;

            switch (dayOfWeekTypeByte)
            {
                case (byte)DayOfWeekType.Sunday:
                    startingDayOfWeekType = DayOfWeekType.Sunday;
                    break;
                case (byte)DayOfWeekType.Monday:
                    startingDayOfWeekType = DayOfWeekType.Monday;
                    break;
                case (byte)DayOfWeekType.Tuesday:
                    startingDayOfWeekType = DayOfWeekType.Tuesday;
                    break;
                case (byte)DayOfWeekType.Wednesday:
                    startingDayOfWeekType = DayOfWeekType.Wednesday;
                    break;
                case (byte)DayOfWeekType.Thursday:
                    startingDayOfWeekType = DayOfWeekType.Thursday;
                    break;
                case (byte)DayOfWeekType.Friday:
                    startingDayOfWeekType = DayOfWeekType.Friday;
                    break;
                case (byte)DayOfWeekType.Saturday:
                    startingDayOfWeekType = DayOfWeekType.Saturday;
                    break;
                //case 12:
                default:
                    startingDayOfWeekType = DayOfWeekType.Sunday;
                    break;
            }

            return startingDayOfWeekType;
        }

        public static CalendarDayOfWeekType ToCalendarDayOfWeekType(this DayOfWeekType dayOfWeekType)
        {
            CalendarDayOfWeekType calendarDayOfWeekType;

            switch (dayOfWeekType)
            {
                case DayOfWeekType.Sunday:
                    calendarDayOfWeekType = CalendarDayOfWeekType.Sunday;
                    break;
                case DayOfWeekType.Monday:
                    calendarDayOfWeekType = CalendarDayOfWeekType.Monday;
                    break;
                case DayOfWeekType.Tuesday:
                    calendarDayOfWeekType = CalendarDayOfWeekType.Tuesday;
                    break;
                case DayOfWeekType.Wednesday:
                    calendarDayOfWeekType = CalendarDayOfWeekType.Wednesday;
                    break;
                case DayOfWeekType.Thursday:
                    calendarDayOfWeekType = CalendarDayOfWeekType.Thursday;
                    break;
                case DayOfWeekType.Friday:
                    calendarDayOfWeekType = CalendarDayOfWeekType.Friday;
                    break;
                case DayOfWeekType.Saturday:
                    calendarDayOfWeekType = CalendarDayOfWeekType.Saturday;
                    break;
                default:
                    throw new InvalidCastException();
                    //calendarDayOfWeekType = CalendarDayOfWeekType.Sunday;
                    //break;
            }

            return calendarDayOfWeekType;
        }

        public static DayOfWeek ToDayOfWeek(this DayOfWeekType dayOfWeekType)
        {
            return ((DayOfWeek)((int)dayOfWeekType - 1));
        }

        public static DayOfWeekType GetOffsetDayOfWeekType(this DayOfWeekType dayOfWeekType, int offset)
        {
            return dayOfWeekType.GetOffsetDayOfWeek(offset).ToDayOfWeekType();
        }

        public static DayOfWeek GetOffsetDayOfWeek(this DayOfWeekType dayOfWeekType, int offset)
        {
            return dayOfWeekType.ToDayOfWeek().GetOffsetDayOfWeek(offset);
        }
    }

    public static class DayOfWeekExtensions
    {
        public static DayOfWeekType ToDayOfWeekType(this DayOfWeek dayOfWeek)
        {
            return ((DayOfWeekType)((int)dayOfWeek + 1));
        }

        public static DayOfWeek GetOffsetDayOfWeek(this DayOfWeek dayOfWeek, int offset)
        {
            return ((DayOfWeek)(((int)dayOfWeek + offset) % 7));
        }
    }
}
