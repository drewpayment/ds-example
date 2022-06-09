namespace Dominion.Core.Dto.Labor
{
    public enum PunchRoundingType : byte
    {
        StartOfShift            = 1,
        EndofShift              = 2,
        UpToQuartHour           = 3,
        UpToTenthofHour         = 4,
        DownToQuartOfHour       = 5,
        DownToTenthofHour       = 6,
        NearestQuarterHour      = 7,
        DownUpToQuarterHour     = 8,
        DailyTotalToQuarterHour = 9,
        NearestFiveMinutes      = 10
    }
}