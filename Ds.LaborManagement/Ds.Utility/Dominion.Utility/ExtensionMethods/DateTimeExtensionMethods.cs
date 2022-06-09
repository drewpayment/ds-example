using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.ExtensionMethods
{
    public static class DateTimeExtensionMethods
    {
        public static TimeSpan ToTimeSpan(this DateTime dt, string timeFormat = "HH:mm:ss")
        {
            return TimeSpan.Parse(dt.ToString(timeFormat));
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> whose Date portion is set from the <see cref="baseDate"/> and TimeOfDay
        /// is set from the given <see cref="TimeSpan"/>. If no <see cref="baseDate"/> is specified, 
        /// <see cref="DateTime.Now"/> will be used.
        /// </summary>
        /// <param name="ts">Timespan to convert to a valid <see cref="DateTime"/>.</param>
        /// <param name="baseDate"><see cref="DateTime"/> to use as the Date portion of the <see cref="DateTime"/>. 
        /// If null, <see cref="DateTime.Now"/> will be used.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this TimeSpan ts, DateTime? baseDate = null)
        {
            return (baseDate ?? DateTime.Now).Date.Add(ts);
        }

        /// <summary>
        /// Adding days based on the value of the DayOfWeek enum.
        /// The concept is the date is start of a week and you know the DayOfWeek for the date you want within that week.
        /// You use that day of week enum value to get the official date within that date range.
        /// For schedules we know what week the user is scheduling for, but in some cases we need to schedule a specific date for that week using the start date.
        /// This only works for week starts that fall on a Sunday or a Monday. If not an exception is thrown.
        /// </summary>
        /// <param name="weekStart">A date that is either a sunday or monday. Represents the start of a week.</param>
        /// <param name="dow">The day of week.</param>
        /// <returns></returns>
        /// <exception cref="Exception">If the <param name="weekStart"></param> isn't on a Sunday or Monday.</exception>
        public static DateTime GetByDayOfWeek(
            this DateTime weekStart, 
            DayOfWeek dow)
        {
            double daysToAdd;
            
            if(weekStart.DayOfWeek != DayOfWeek.Sunday && weekStart.DayOfWeek != DayOfWeek.Monday)
                throw new Exception("Week start must be a Sunday or Monday.");

            if(weekStart.DayOfWeek == DayOfWeek.Sunday)
            {
                daysToAdd = (double)dow;
            }
            else
            {
                if(dow == DayOfWeek.Sunday)
                    daysToAdd = 7d;
                else
                    daysToAdd = (int)dow-1;
            }
                
            return weekStart.AddDays(daysToAdd);
        }

        /// <summary>
        /// Returns 23:59:59:999 of the current date.  This method will use start of the date and end
        /// the additional hours.  If you pass in a datetime object, the hours will be rounded down to the start
        /// of the day before the full days hours will be added.  For instance, if you pass in a date at 17:00 (5:00pm)
        /// it will be be rounded down to 0:00 (12:00am)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToEndOfDay(this DateTime date)
        {
            return date.Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
        }

        /// <summary>
        /// For use when dealing with legacy calls in to VB code.  Instead of having null values, there is a constant of 
        /// No_Date_Selected_Value.  This returns the date used in the constant
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToNoDateSelected(this DateTime date)
        {
            var newDate = DateTime.Parse("01/01/1900");
            return date;
        }

        /// <summary>
        /// Returns a new DateTime object with the minute rounded down 
        /// Used when inserting clock punches
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime RoundDownToMinute(this DateTime date)
        {
            date = date.AddSeconds(date.Second * -1);
            date = date.AddMilliseconds(date.Millisecond * -1);
            return date;
        }

        /// <summary>
        /// Returns the date of the specified <see cref="DayOfWeek"/> that falls 
        /// within the week the extended <see cref="weekOf"/> date falls in.
        /// </summary>
        /// <param name="weekOf">Date falling within the week to the the day-of-week for.</param>
        /// <param name="dow"><see cref="DayOfWeek"/> to get.</param>
        /// <param name="firstDayOfWeek"><see cref="DayOfWeek"/> to use for the first day of the week. 
        /// Defaults to <see cref="CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek"/>.</param>
        /// <returns></returns>
        public static DateTime GetDayOfWeekDate(this DateTime weekOf, DayOfWeek dow, DayOfWeek? firstDayOfWeek = null)
        {
            if(weekOf.DayOfWeek == dow)
                return weekOf;
            
            firstDayOfWeek = firstDayOfWeek ?? CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

            var dateShift = (int)dow - (int)weekOf.DayOfWeek;

            if(dow < firstDayOfWeek)
                dateShift += 7;

            var date = weekOf.AddDays(dateShift);
            return date;
        }

        /// <summary>
        /// Returns the number of whole years that have passed. If the date is null, null will be returned.
        /// </summary>
        /// <param name="d">Date to determine age for.</param>
        /// <param name="asOf">Date to calculate age from. Default is <see cref="DateTime.Now"/>.</param>
        /// <returns></returns>
        public static int? GetAgeInYears(this DateTime? d, DateTime? asOf = null)
        {
            return d?.GetAgeInYears(asOf);
        }

        /// <summary>
        /// Returns the number of whole years that have passed.
        /// </summary>
        /// <param name="d">Date to determine age for.</param>
        /// <param name="asOf">Date to calculate age from. Default is <see cref="DateTime.Now"/>.</param>
        /// <returns>Age in number of whole years.</returns>
        /// <remarks>
        /// From: http://stackoverflow.com/a/1404
        /// </remarks>
        public static int GetAgeInYears(this DateTime d, DateTime? asOf = null)
        {
            asOf = asOf ?? DateTime.Now;

            var age = asOf.Value.Year - d.Year;
            if(d > asOf.Value.AddYears(-age))
                age--;

            return age;
        }

        /// <summary>
        /// Convert minutes to millisecoinds.
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static decimal ToMilliseconds(this decimal minutes)
        {
            return minutes * 60m * 1000m;
        }

        /// <summary>
        /// Returns the whole number of days between two dates by subtracting the
        /// <see cref="startDate"/> from the <see cref="endDate"/>.
        /// </summary>
        /// <param name="endDate">The end date of the comparison. Typically, the "larger" or "more into the future" date.</param>
        /// <param name="startDate">The start date of the comparison. Typically the "smaller" or "less into the future" date. Default is <see cref="DateTime.Now"/>.</param>
        /// <param name="endDateInclusive">Should endDate be included the count of days?  ie: if true, Monday - Sunday would be 7 days.</param>
        /// <returns>Number of days between two dates in whole days.</returns>
        /// <remarks>
        /// From: https://stackoverflow.com/questions/1607336/calculate-difference-between-two-dates-number-of-days
        /// </remarks>
        public static int DaysBetweenDates(this DateTime endDate, DateTime? startDate = null, bool endDateInclusive = false)
        {
            var wholeDays = (endDate.Date - (startDate ?? DateTime.Now).Date).Days;

            if (endDateInclusive)
                wholeDays++;

            return wholeDays;
        }

        /// <summary>
        /// Returns the number of weeks between two dates by getting <see cref="DaysBetweenDates(DateTime, DateTime?, bool)"/> and dividing by 7
        /// </summary>
        /// <param name="endDate"></param>
        /// <param name="startDate"></param>
        /// <param name="endDateInclusive"></param>
        /// <returns>Number of weeks between two dates</returns>
        public static double WeeksBetweenDates(this DateTime endDate, DateTime? startDate = null, bool endDateInclusive = false)
        {
            var totalDays = endDate.DaysBetweenDates(startDate ?? DateTime.Now);

            if (endDateInclusive)
                totalDays++;

            return totalDays / 7;
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            var diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff);
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
        {
            var diff = (7 + (endOfWeek - dt.DayOfWeek)) % 7;
            return dt.AddDays(diff);
        }

        public static DateTime StartOfDay(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 00, 00, 00);
        }

        public static DateTime EndOfDay(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
        }

        public static bool HasValue(this DateTime? dt)
        {
            return dt != null && dt > DateTime.MinValue;
        }

        public static bool IsBetweenDates(this DateTime dt, DateTime start, DateTime end)
        {
            return dt >= start && dt <= end;
        }
    }
}
