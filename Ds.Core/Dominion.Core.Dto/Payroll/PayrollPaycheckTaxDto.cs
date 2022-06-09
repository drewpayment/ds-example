using Dominion.Core.Dto.Contact.Search;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class PayrollPaycheckTaxDto : ContactSearchDto
    {
        public int? GenTaxDefermentHistoryId { get; set; }
        public int? GenPayrollHistoryId { get; set; }
        public int? GenPaycheckPayDataHistoryId { get; set; }
        public decimal MedicareTax { get; set; }
        public decimal EmployerMedicareTax { get; set; }
        public decimal SocSecTax { get; set; }
        public decimal EmployerSocSecTax { get; set; }
        public decimal EmployerFutaTax { get; set; }
        public decimal FedWH { get; set; }
        public decimal StateWithholding { get; set; }
        public string TaxName { get; set; }
        public decimal SUTATax { get; set; }
        public decimal DisabilityTax { get; set; }
        public decimal LocalTax { get; set; }
        public int StateId { get; set; }
        public int? ClientTaxId { get; set; }
        public decimal SSDefermentTax { get; set; }
        public decimal FFCRACredit { get; set; }
        public decimal Amount { get; set; }
        public DateTime CheckDate { get; set; }
        public decimal YTDTotal { get; set; }
        public decimal QTDTotal { get; set; }
        public decimal MTDTotal { get; set; }

    }
}

