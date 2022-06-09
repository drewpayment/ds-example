using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Tax;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Entity for the dbo.genTaxDefermentHistory table.
    /// </summary>
    public partial class TaxDefermentHistory : Entity<TaxDefermentHistory>
    {
        public virtual int GenTaxDefermentHistoryId { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual int TaxTypeId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual DateTime? PayrollCheckDate { get; set; }
        public virtual int PayrollId { get; set; }
        public virtual int GenPayrollHistoryId { get; set; }
    }
}
