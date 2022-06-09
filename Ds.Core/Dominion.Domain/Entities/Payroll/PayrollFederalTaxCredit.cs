using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Tax;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Entity for the dbo.genPayrollFederalTaxCredit table.
    /// </summary>
    public partial class PayrollFederalTaxCredit : Entity<PayrollFederalTaxCredit>
    {
        public virtual int GenPayrollFederalTaxCreditID { get; set; }
        public virtual int GenPayrollHistoryID { get; set; }
        public virtual decimal FMLATotal { get; set; }
        public virtual decimal SickLeaveTotal { get; set; }
        public virtual decimal FMLACreditTaken { get; set; }
        public virtual decimal SickLeaveCreditTaken { get; set; }
        public virtual decimal FMLAMedicareTax { get; set; }
        public virtual decimal FMLAMedicareCreditTaken { get; set; }
        public virtual decimal SickLeaveMedicareTax { get; set; }
        public virtual decimal SickLeaveMedicareCreditTaken { get; set; }
        public virtual decimal MedicalInsurance { get; set; }
        public virtual decimal MedicalCreditTaken { get; set; }
        public virtual decimal ReportedCredit { get; set; }
        public virtual int ClientId { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual DateTime PayrollCheckDate { get; set; }
        public virtual int PayrollId { get; set; }
    }
}
