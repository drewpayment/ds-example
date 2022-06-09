using System.Collections.Generic;
using Dominion.Core.Dto.Common;

namespace Dominion.Core.Dto.Tax.EmployeeTaxAdmin
{
    public class EmployeeTaxCostCenterConfigurationDto
    {
        public IEnumerable<KeyValueDto> AvailableCostCenters { get; set; }
        public IEnumerable<KeyValueDto> SelectedCostCenters { get; set; }
    }
}
