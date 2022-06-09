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
    public class PayrollPaycheckTaxTotalsDto : ContactSearchDto
    {
        public IEnumerable<PayrollPaycheckTaxDto> FederalTaxes { get; set; }
        public IEnumerable<PayrollPaycheckTaxDto> StateTaxes { get; set; }
        public IEnumerable<PayrollPaycheckTaxDto> LocalTaxes { get; set; }
    }
}

