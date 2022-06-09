namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Entity representation of an individual payroll preview paycheck earning. 
    /// </summary>
    public partial class PreviewPaycheckEarning
    {
        public virtual int      PreviewPaycheckEarningId { get; set; } 
        public virtual int      PreviewPaycheckPayDataId { get; set; } 
        public virtual int      ClientEarningId          { get; set; } 
        public virtual double   EarningPercent           { get; set; } 
        public virtual bool     IsTips                   { get; set; } 
        public virtual byte     CalcShiftPremium         { get; set; } 
        public virtual double   AdditionalAmount         { get; set; } 
        public virtual int      AdditionalAmountTypeId   { get; set; } 
        public virtual int      Destination              { get; set; } 
        public virtual double?  Hours                    { get; set; } 
        public virtual decimal? Amount                   { get; set; } 
        public virtual decimal  TotalAmount              { get; set; } 
        public virtual int?     ClientRateId             { get; set; } 
        public virtual double?  Rate                     { get; set; } 
        public virtual int      ClientEarningCategoryId  { get; set; } 
        public virtual bool     IsIncludeInDeductions    { get; set; } 
        public virtual bool     IsShiftPremium           { get; set; } 
        public virtual bool     IsEarningDeduction       { get; set; } 
        public virtual int      ClientId                 { get; set; } 
        public virtual int      EmployeeId               { get; set; } 
        public virtual double   ActualHours              { get; set; } 
        public virtual double   ActualTotalAmount        { get; set; } 
        public virtual bool     IsServiceChargeTips      { get; set; } 
        public virtual double   ActualAmount             { get; set; } 

        //FOREIGN KEYS
        public virtual PreviewPaycheckPayData PreviewPaycheckPayData { get; set; }
        public virtual ClientEarning ClientEarning { get; set; }
    }
}
