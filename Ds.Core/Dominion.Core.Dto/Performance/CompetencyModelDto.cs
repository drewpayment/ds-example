using Dominion.Core.Dto.Labor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dominion.Core.Dto.Performance
{
    public class CompetencyModelDto : CompetencyModelBasicDto
    {
        public string                                             Description          { get; set; }
        public IEnumerable<EmployeePerformanceConfigurationDto>   EmpPerfConfigs       { get; set; }
        public IEnumerable<JobProfileDto>                         JobProfiles          { get; set; }
        public CompetencyGroupDto                                 CompetencyGroupDto   { get; set; }
    }

    public class CompetencyModelBasicDto
    {
        public int                                                CompetencyModelId    { get; set; }
        public int?                                               ClientId             { get; set; }
        public string                                             Name                 { get; set; }
        public IEnumerable<CompetencyDto>                         Competencies         { get; set; }
    }
}
