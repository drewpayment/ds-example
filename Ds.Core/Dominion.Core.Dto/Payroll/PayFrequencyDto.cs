using System;

namespace Dominion.Core.Dto.Payroll
{
    /// <summary>
    /// DTO containing basic info regarding a <see cref="PayFrequencyType"/>.
    /// </summary>
    [Serializable]
    public class PayFrequencyDto
    {
        public virtual PayFrequencyType    PayFrequencyId               { get; set; } 
        public virtual string              Name                         { get; set; } 
        public virtual int                 AnnualPayPeriodCount         { get; set; } 
        public virtual string              Code                         { get; set; } 
        public virtual string              CalendarFrequencyDescription { get; set; } 
       
    }
    [Serializable]
    public class PayFrequencyListDto
    {
        public virtual PayFrequencyType    PayFrequencyId               { get; set; } 
        public virtual string              Name                         { get; set; } 
        public virtual bool                IsPayrollOption              { get; set; }
       
    }
}
