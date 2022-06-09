using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;

namespace Dominion.Core.Dto.Payroll
{
    public class PayrollHistoryDto
    {
        public int PayrollHistoryId { get; set; }
        public int PayrollId { get; set; }
        public DateTime CheckDate { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public string BankAccount { get; set; }
        public int? BankId { get; set; }
        public string AltBankAccount { get; set; }
        public int? AltBankId { get; set; }
        public string TaxAccount { get; set; }
        public int? TaxBankId { get; set; }
        public string DebitAccount { get; set; }
        public int? DebitBankId { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public int ClientId { get; set; }
        public PayrollRunType? PayrollRunTypeId { get; set; }

        //REVERSE NAVIGATION
        //public ICollection<BillingHistory> BillingHistory { get; set; } // many-to-one;
        public ICollection<PaycheckHistoryDto> PaycheckHistory { get; set; }
        public ClientDto Client { get; set; }
        public PayrollDto Payroll { get; set; }
    }
}
