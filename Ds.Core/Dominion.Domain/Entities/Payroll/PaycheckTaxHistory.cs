using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Tax;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Entity for the dbo.genPaycheckTaxHistory table.
    /// </summary>
    public partial class PaycheckTaxHistory : Entity<PaycheckTaxHistory>
    {
        public virtual int       GenPaycheckTaxHistoryId     { get; set; } 
        public virtual int       GenPaycheckPayDataHistoryId { get; set; } 
        public virtual int?      ClientTaxId                 { get; set; } 
        public virtual decimal   GrossWages                  { get; set; } 
        public virtual decimal   TaxableWage                 { get; set; } 
        public virtual string    TaxType                     { get; set; } 
        public virtual int       MaritalStatusId             { get; set; } 
        public virtual int       Exemptions                  { get; set; } 
        public virtual decimal   CalculatedAmount            { get; set; } 
        public virtual decimal   AdditionalAmount            { get; set; } 
        public virtual decimal   TotalAmount                 { get; set; } 
        public virtual int       ClientId                    { get; set; } 
        public virtual int       EmployeeId                  { get; set; } 
        public virtual DateTime  Modified                    { get; set; } 
        public virtual string    ModifiedBy                  { get; set; } 
        public virtual DateTime? PayrollCheckDate            { get; set; } 
        public virtual int?      PayrollId                   { get; set; } 
        public virtual int?      PaycheckId                  { get; set; }
        public virtual decimal   EmployerPaidAmount          { get; set; }
        public virtual bool      IsEmployerPaidTax           { get; set; }

        //FOREIGN KEYS
        public virtual PaycheckPayDataHistory PaycheckPayDataHistory { get; set; } 
        public virtual ClientTax              ClientTax              { get; set; }
        public virtual Client                 Client                 { get; set; }
        public virtual Employee.Employee      Employee               { get; set; }
        public virtual Payroll                Payroll                { get; set; }
        public virtual PaycheckHistory        PaycheckHistory        { get; set; }
    }
}
