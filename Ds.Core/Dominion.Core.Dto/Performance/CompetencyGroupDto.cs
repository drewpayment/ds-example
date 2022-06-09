using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class CompetencyGroupBaseDto
    {
        public int CompetencyGroupId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
    }

    public class CompetencyGroupDto : CompetencyGroupBaseDto
    {
        // RELATIONSHIPS

        public ICollection<CompetencyDto> Competencies { get; set; }
    }

    public class CompetencyGroupEvaluationDto : CompetencyGroupBaseDto
    {
        public ICollection<CompetencyEvaluationDto> Competencies { get; set; }
    }
}
