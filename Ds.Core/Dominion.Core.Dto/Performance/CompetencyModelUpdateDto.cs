using Dominion.Core.Dto.Labor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class CompetencyModelUpdateDto
    {
        public int CompetencyModelId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public IEnumerable<CompetencyDto> AddedCompetencies { get; set; }
        public IEnumerable<CompetencyDto> RemovedCompetencies { get; set; }
        public IEnumerable<EmployeePerformanceConfigurationDto> AddedEmpPerfConfigs { get; set; }
        public IEnumerable<EmployeePerformanceConfigurationDto> RemovedEmpPerfConfigs { get; set; }
        public IEnumerable<JobProfileDto> AddedJobProfiles { get; set; }
        public IEnumerable<JobProfileDto> RemovedJobProfiles { get; set; }
    }
}
