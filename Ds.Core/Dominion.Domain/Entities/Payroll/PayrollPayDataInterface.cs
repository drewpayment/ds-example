using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Currently available payroll pay data interface import types.
    /// </summary>
    public enum PayrollPayDataInterfaceType : byte
    {
        DominionSpreadsheet = 1, 
        AdpCsv = 2, 
        Atm = 3, 
        LaborSource = 4, 
        PunchDetail = 5, 
        OneTimeDeduction = 6, 
        TimeInTheBox = 7, 
        AdpFixedFile = 8, 
        Pos1Csv = 9, 
        TimeclockPlusFixedFile = 10, 
        KronosCsv = 11, 
        AccountLinxCsv = 12, 
        MultipleOnetimeTaxAndDeductions = 13, 
        AtmAccrualBalances = 14, 
        HaasTrucking = 15, 
        TimeInTheBoxMpu1 = 16, 
        TracyTimeDbf = 17, 
        LifetimeHours = 18, 
        Expensify = 19
    }

    /// <summary>
    /// Represents an interface import type used for importing payroll pay data values.
    /// </summary>
    public class PayrollPayDataInterface : Entity<PayrollPayDataInterface>
    {
        public virtual PayrollPayDataInterfaceType PayrollPayDataInterfaceId { get; set; }
        public virtual string Description { get; set; }
    }
}