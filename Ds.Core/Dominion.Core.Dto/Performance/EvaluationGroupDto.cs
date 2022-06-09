using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class EvaluationGroupDto
    {
        public int EvaluationGroupId { get; set; }
        public int? ClientId { get; set; }
        public int? ParentId { get; set; }
        public int? RootId { get; set; }
        public string Name { get; set; }
        public int OrderSequence { get; set; }
        public bool HasWeightedSubGroups { get; set; }
        public decimal? Weight { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public bool IncludeUngroupedCompetencies { get; set; }
        public IEnumerable<EvaluationGroupDto> ChildGroups { get; set; }
        public CompetencyEvaluationGroupDto CompetencyEvaluationGroup { get; set; }
        public GoalEvaluationGroupDto GoalEvaluationGroup { get; set; }
    }
}
