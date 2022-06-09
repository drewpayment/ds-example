using System;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Entity for the dbo.GenPaycheckEarningHistory table.
    /// </summary>
    public partial class PaycheckEarningHistory : Entity<PaycheckEarningHistory>
    {
        public virtual int       GenPaycheckEarningHistoryId { get; set; } 
        public virtual int       GenPaycheckPayDataHistoryId { get; set; } 
        public virtual int       ClientEarningId             { get; set; } 
        public virtual double    EarningPercent              { get; set; } 
        public virtual bool      IsTips                      { get; set; } 
        public virtual byte      CalcShiftPremium            { get; set; } 
        public virtual double    AdditionalAmount            { get; set; } 
        public virtual int       AdditionalAmountTypeId      { get; set; } 
        public virtual int       Destination                 { get; set; } 
        public virtual double?   Hours                       { get; set; } 
        public virtual decimal?  Amount                      { get; set; } 
        public virtual decimal   TotalAmount                 { get; set; } 
        public virtual int?      ClientRateId                { get; set; } 
        public virtual double?   Rate                        { get; set; } 
        public virtual ClientEarningCategory ClientEarningCategoryId { get; set; } 
        public virtual bool      IsIncludeInDeductions       { get; set; } 
        public virtual int       ClientId                    { get; set; } 
        public virtual int       EmployeeId                  { get; set; } 
        public virtual bool      IsShiftPremium              { get; set; } 
        public virtual DateTime  Modified                    { get; set; } 
        public virtual string    ModifiedBy                  { get; set; } 
        public virtual double    ActualHours                 { get; set; } 
        public virtual double    ActualTotalAmount           { get; set; } 
        public virtual bool?     IsServiceChargeTips         { get; set; } 
        public virtual double    ActualAmount                { get; set; } 
        public virtual DateTime? PayrollCheckDate            { get; set; } 
        public virtual int?      PayrollId                   { get; set; } 
        public virtual int?      PaycheckId                  { get; set; } 

        //FOREIGN KEYS
        public virtual PaycheckPayDataHistory PaycheckPayDataHistory { get; set; } 
        public virtual ClientEarning          ClientEarning          { get; set; }
        public virtual ClientRate             ClientRate             { get; set; }
        public virtual Client                 Client                 { get; set; }
        public virtual Employee.Employee      Employee               { get; set; }
        public virtual Payroll                Payroll                { get; set; }
        public virtual PaycheckHistory        PaycheckHistory        { get; set; }
        public virtual EarningCategory        EarningCategory        { get; set; }
    }
}