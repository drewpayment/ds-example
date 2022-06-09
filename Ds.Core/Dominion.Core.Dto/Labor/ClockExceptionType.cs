namespace Dominion.Core.Dto.Labor
{
    public enum ClockExceptionType
    {
        ArrivedEarly                     = 1,
        Tardy                            = 2,
        LeftEarly                        = 3,
        LeftLate                         = 4,
        Long                             = 5,
        Short                            = 6, 
        UnderSchedule                    = 7,
        OverSchedule                     = 8,
        MissingPunch                     = 9,
        PunchesOnBenefitDay              = 10,
        NoPunchesOnScheduledDay          = 11,
        DidNotWorkBeforeHoliday          = 12,
        DidNotWorkAfterHoliday           = 13,
        DidNotWorkBeforeHolidayScheduled = 14,
        DidNotWorkAfterHolidayScheduled  = 15,
        UnscheduledHours                 = 16,
        BadLocation                      = 17,
        NoLocation                       = 18,
        None                             = 99
    }

    public enum ClockExceptionGroupType
    {
        Punch = 1,
        Daily = 2,
        Lunch = 3,
        Holiday = 4
    }
}