using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.User;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public class EvaluationGroup : IHasModifiedData
    {
        public int EvaluationGroupId { get; set; }
        public int? ClientId { get; set; }
        public int? ParentId { get; set; }
        public int? RootId { get; set; }
        public string Name { get; set; }
        public Int16 OrderSequence { get; set; }
        public bool HasWeightedSubGroups { get; set; }
        public decimal? Weight { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public bool IncludeUngroupedCompetencies { get; set; }
        public EvaluationGroup Parent { get; set; }
        public EvaluationGroup Root { get; set; }
        public User.User ModifiedByUser { get; set; }
        public virtual CompetencyEvaluationGroup CompetencyEvaluationGroup { get; set;}
        public virtual GoalEvaluationGroup GoalEvaluationGroup { get; set; }
        public virtual ICollection<EvaluationGroupEvaluation> EvaluationGroupEvaluations { get; set; }
        public virtual ICollection<EvaluationGroup> ChildrenEvaluationGroups { get; set; }
    }
}
