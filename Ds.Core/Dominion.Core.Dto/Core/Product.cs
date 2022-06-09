namespace Dominion.Core.Dto.Core
{
    //Could use this in place of the 'System' field in ClientOptionControl table.
    public enum Product : byte
    {
        Payroll                       = 1,
        TimeAndAttendance             = 2,
        Reporting                     = 3,
        LeaveManagement               = 4,
        ApplicantTracking             = 5,
        Onboarding                    = 6,
        BenefitAdministration         = 7,
        PerformanceReviews            = 8,
        GoalTracking                  = 9,
        DominionOnly                  = 10,
        AutomatedPoints               = 11,
        HumanResources                = 12
    }
}
