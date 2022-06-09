using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Employee;

namespace Dominion.Core.Dto.Performance
{
    public class EmployeePerformanceConfigurationDto
    {
        public int EmployeeId { get; set; }
        public int? CompetencyModelId { get; set; }
        public bool HasAdditionalEarnings { get; set; }
        public CompetencyModelDto CompetencyModel { get; set; }
        public EmployeeBasicDto Employee { get; set; }
        public OneTimeEarningSettingsDto OneTimeEarningSettings { get; set; }
        public int ClientId { get; set; }
    }
}
