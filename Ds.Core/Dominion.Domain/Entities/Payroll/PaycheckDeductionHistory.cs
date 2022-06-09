using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Entity for the dbo.genPaycheckDeductionHistory table.
    /// </summary>
    public partial class PaycheckDeductionHistory : Entity<PaycheckDeductionHistory>
    {
        public virtual int       GenPaycheckDeductionHistoryId   { get; set; } 
        public virtual int?      GenPaycheckPayDataHistoryId     { get; set; } 
        public virtual int?      ClientDeductionId               { get; set; } 
        public virtual int?      BondId                          { get; set; } 
        public virtual byte?     TaxOption                       { get; set; } 
        public virtual int?      DeferredCompId                  { get; set; } 
        public virtual string    RoutingNumber                   { get; set; } 
        public virtual string    AccountNumber                   { get; set; } 
        public virtual byte?     AccountType                     { get; set; } 
        public virtual string    TraceNumber                     { get; set; } 
        public virtual decimal   Amount                          { get; set; } 
        public virtual bool      IsMemoDeduction                 { get; set; } 
        public virtual int?      ClientVendorId                  { get; set; } 
        public virtual int?      GenDelinquentDeductionHistoryId { get; set; } 
        public virtual int       ClientId                        { get; set; } 
        public virtual int       EmployeeId                      { get; set; } 
        public virtual DateTime  Modified                        { get; set; } 
        public virtual string    ModifiedBy                      { get; set; } 
        public virtual DateTime? PayrollCheckDate                { get; set; } 
        public virtual int?      PayrollId                       { get; set; } 
        public virtual int?      PaycheckId                      { get; set; } 
        public virtual string    DeductionDescription            { get; set; }

        //FOREIGN KEYS
        public virtual PaycheckPayDataHistory PaycheckPayDataHistory { get; set; }
        public virtual ClientDeduction        ClientDeduction        { get; set; }
        public virtual Payroll                Payroll                { get; set; }
        public virtual PaycheckHistory        PaycheckHistory        { get; set; }
        public virtual Employee.Employee      Employee               { get; set; }
        public virtual Client                 Client                 { get; set; }
    }
}