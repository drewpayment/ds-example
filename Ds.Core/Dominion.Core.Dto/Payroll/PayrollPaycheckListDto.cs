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
    public class PayrollPaycheckListDto : ContactSearchDto
    {
        public int       GenPaycheckHistoryId { get; set; }
        public int       GenPayrollHistoryId  { get; set; }
        public decimal   CheckAmount          { get; set; }
        public int       OwnerId              { get; set; }
        public string    SubCheck             { get; set; }
        public int?      CheckNumber          { get; set; }
        public decimal   GrossPay             { get; set; }
        public decimal   NetPay               { get; set; }
        public string    Name                 { get; set; }
        public string    EmployeeNumber       { get; set; }
        public DateTime? CheckDate            { get; set; }
        public int?      PayrollId            { get; set; }
        public int?      ViewRates            { get; set; }
        public bool      IsVendor             { get; set; }
        public int?      VendorTypeId         { get; set; }
        public string EmployeePayType         { get; set; }
        public decimal? EmployeeDeductionAmount { get; set; }
        public decimal YTDTotal { get; set; }
        public decimal QTDTotal { get; set; }
        public decimal MTDTotal { get; set; }
    }
}
