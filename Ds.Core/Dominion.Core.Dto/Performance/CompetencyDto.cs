using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;

namespace Dominion.Core.Dto.Performance
{
    public class CompetencyDto
    {
        public int CompetencyId { get; set; }
        public int? ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CompetencyGroupId { get; set; }
        public bool IsCore { get; set; }
        public bool IsArchived { get; set; }
        public int? DifficultyLevel { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }

        // UI PROPERTIES (NOT STORED IN SQL)
        public bool CanRemove { get; set; }

        // RELATIONSHIPS

        public ClientDto Client { get; set; }
        public CompetencyGroupDto CompetencyGroup { get; set; }
        public virtual IEnumerable<CompetencyModelBasicDto> Models { get; set; }
        public virtual IEnumerable<CompetencyEvaluationDto> Evaluations { get; set; }
    }
}
