using System;
using System.Data.SqlTypes;

namespace Dominion.Utility.DataGeneration
{
public static class DateTimeRandomizer
{
    private static Random random = new Random();

    public static DateTime GenerateYearsFromToday(int years)
    {
        var today = DateTime.Now;
        var startYear = today.Year - years;
        var startDateTime = new DateTime(startYear, 1, 1);
        
        return Generate(startDateTime, today);
    }

    public static DateTime Generate(DateTime? start = null, DateTime? end = null)
    {
        if (start.HasValue && end.HasValue && start.Value >= end.Value)
            throw new Exception("start date must be less than end date!");

        DateTime min = start ?? DateTime.MinValue;
        DateTime max = end ?? DateTime.MaxValue;

        // for timespan approach see: http://stackoverflow.com/questions/1483670/whats-the-best-practice-for-getting-a-random-date-time-between-two-date-times/1483677#1483677
        // for random long see: http://stackoverflow.com/questions/677373/generate-random-values-in-c/677384#677384
        var timeSpan = max - min;
        var bytes = new byte[8];
        random.NextBytes(bytes);

        var int64 = Math.Abs(BitConverter.ToInt64(bytes, 0)) % timeSpan.Ticks;
        var newSpan = new TimeSpan(int64);
        return min + newSpan;
    }
    
    public static DateTime GenerateSql(SqlDateTime? start = null, SqlDateTime? end = null)
    {
        var startDateTime = 
            (start.HasValue)
            ? start.Value.Value
            : DateTime.MinValue;

        var endDateTime = 
            (end.HasValue)
            ? end.Value.Value
            : DateTime.MaxValue;
        
        return Generate(startDateTime, endDateTime);
    }   
}
}
