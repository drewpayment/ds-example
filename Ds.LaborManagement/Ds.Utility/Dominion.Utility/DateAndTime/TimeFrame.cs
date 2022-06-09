using System;

namespace Dominion.Utility.DateAndTime
{
    public static class TimeFrame
    {
        /// <summary>
        /// From: https://stackoverflow.com/a/21343435/1464577
        ///
        /// Example usage:
        /// startTime=1:45PM, endTime=1:50PM, timeToCheck=1:50PM
        /// Result: false
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="timeToCheck"></param>
        /// <returns></returns>
        public static bool IsInTimeFrame(DateTime startTime, DateTime endTime, DateTime timeToCheck)
        {
            var start = TimeSpan.Parse(startTime.ToString("HH:m"));
            var end = TimeSpan.Parse(endTime.ToString("HH:m"));
            var now = timeToCheck.TimeOfDay;
            var isInTimeFrame = false;

            if (start <= end)
            {
                if (now >= start && now <= end)
                {
                    isInTimeFrame = true;
                }
            }
            else
            {
                if (now >= start || now <= end)
                {
                    isInTimeFrame = true;
                }
            }

            return isInTimeFrame;
        }

        /// <summary>
        /// Example usage:
        /// startTime=1:45PM, endTime=1:50PM, timeToCheck=1:50PM
        /// Result: true
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="timeToCheck"></param>
        /// <returns></returns>
        public static bool IsBetweenTimeFrame(DateTime startTime, DateTime endTime, DateTime timeToCheck)
        {
            var start = TimeSpan.Parse(startTime.ToString("HH:m"));
            var end = TimeSpan.Parse(endTime.ToString("HH:m"));
            var now = timeToCheck.TimeOfDay;
            var isInTimeFrame = false;

            if (start <= end)
            {
                if (now >= start && now < end)
                {
                    isInTimeFrame = true;
                }
            }
            else
            {
                if (now >= start || now < end)
                {
                    isInTimeFrame = true;
                }
            }

            return isInTimeFrame;
        }
    }
}