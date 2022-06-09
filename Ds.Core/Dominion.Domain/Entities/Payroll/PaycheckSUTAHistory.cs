using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Entities.Tax;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Entity for the dbo.genPaycheckSUTAHistory table.
    /// </summary>
    public partial class PaycheckSUTAHistory : Entity<PaycheckSUTAHistory>
    {
        public virtual int GenPaycheckSUTAHistoryId { get; set; }
        public virtual int GenPaycheckPayDataHistoryId { get; set; }
        public virtual int StateId { get; set; }
        public virtual decimal LimitWages { get; set; }
        public virtual decimal ExcessWages { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual decimal FUTAWages { get; set; }
        public virtual decimal FUTATax { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual int ClientTaxID { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? PayrollCheckDate { get; set; }
        public virtual int? PayrollId { get; set; }
        public virtual int? PaycheckId { get; set; }

        //FOREIGN KEYS
        public virtual PaycheckPayDataHistory PaycheckPayDataHistory { get; set; }
        public virtual ClientTax ClientTax { get; set; }
        public virtual Client Client { get; set; }
        public virtual Employee.Employee Employee { get; set; }
        public virtual Payroll Payroll { get; set; }
        public virtual PaycheckHistory PaycheckHistory { get; set; }
        public virtual State State { get; set; }
    }
}
