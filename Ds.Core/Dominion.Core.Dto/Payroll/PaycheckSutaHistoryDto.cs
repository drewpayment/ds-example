using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class PaycheckSutaHistoryDto
    {
        public int GenPaycheckSUTAHistoryId { get; set; }
        public int GenPaycheckPayDataHistoryId { get; set; }
        public int StateId { get; set; }
        public decimal LimitWages { get; set; }
        public decimal ExcessWages { get; set; }
        public decimal Amount { get; set; }
        public decimal FUTAWages { get; set; }
        public decimal FUTATax { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientTaxId { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? PayrollCheckDate { get; set; }
        public int? PayrollId { get; set; }
        public int? PaycheckId { get; set; }
    }
}
