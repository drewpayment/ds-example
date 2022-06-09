using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public class CompetencyGroup : Entity<CompetencyGroup>
    {
        public int CompetencyGroupId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }

        // RELATIONSHIPS

        public ICollection<Competency> Competencies { get; set; }
        public ICollection<CompetencyEvaluationGroup> CompetencyEvaluationGroups { get; set; }
    }
}
