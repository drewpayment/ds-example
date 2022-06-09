namespace Dominion.Core.Dto.Payroll
{
    /// <summary>
    /// System Pay Frequency types (per dbo.PayFrequency table)
    /// </summary>
    public enum PayFrequencyType
    {
        Weekly            = 1,
        BiWeekly          = 2,
        SemiMonthly       = 3,
        Monthly           = 4,
        AlternateBiWeekly = 5,
        Quarterly         = 6,
        Annually          = 7
    }
}
