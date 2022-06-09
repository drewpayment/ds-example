using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class PayrollPayDataDto
    {
        public int       PayrollPayDataId         { get; set; }
        public int       EmployeeId               { get; set; }
        public int       PayrollId                { get; set; }
        public bool      IsPay                    { get; set; }
        public string    SubCheck                 { get; set; }
        public int?      ClientRateId             { get; set; }
        public DateTime? ModifiedDate             { get; set; }
        public string    ModifiedBy               { get; set; }
        public int?      ClientPayDataInterfaceId { get; set; }
        public string    SourceId                 { get; set; }
        public int?      ClientId                 { get; set; }
        public int       Week                     { get; set; }

        public Employee.EmployeeFullDto Employee  { get; set; }
        public PayrollDto Payroll                 { get; set; }
        public ICollection<PayrollPayDataDetailDto> PayDataDetails { get; set; } // many-to-one;
    }
}

