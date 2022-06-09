using System.Collections.Generic;
using Dominion.Core.Dto.Common;

namespace Dominion.Core.Dto.Tax.EmployeeTaxAdmin
{
    public class EmployeeTaxSetupDto : EmployeeTaxDto
    {
        public IEnumerable<KeyValueDto> FilingStatuses { get; set; }
        public IEnumerable<KeyValueDto> WithholdingOptions { get; set; }
    }
}
