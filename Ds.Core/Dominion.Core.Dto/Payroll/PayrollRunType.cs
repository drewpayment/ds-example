namespace Dominion.Core.Dto.Payroll
{
    /// <summary>
    /// Payroll types. 
    /// Copied from dsPayroll.PayrollRun.
    /// </summary>
    public enum PayrollRunType
    {
        NormalPayroll     = 1,
        SpecialPayroll    = 2,
        Adjustment        = 3,
        Parallel          = 4,
        TaxcheckRun       = 5,
        RealTimeRun       = 6,
        YearEndAdjustment = 7,
    }
}
