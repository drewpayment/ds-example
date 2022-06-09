using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class BankDepositInfoDto
    {
        public Boolean IsCurrent  { get; set; }
        public Boolean IsPrevious { get; set; }
        public decimal TaxPaymentTotal { get; set; }
        public decimal EEPaymentTotal { get; set; }
        public decimal VendorPaymentTotal { get; set; }
        public decimal BankDepositTotal { get; set; }
        public Boolean ShowPrevious { get; set; }
        public decimal PercentDifference { get; set; }
        public string  MaterialIcon { get; set; }
    }
}
