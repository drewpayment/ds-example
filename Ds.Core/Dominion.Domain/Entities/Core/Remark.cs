using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Domain.Entities.PerformanceReviews;

namespace Dominion.Domain.Entities.Core
{
    public class Remark : Entity<Remark>, IHasModifiedData
    {
        public int      RemarkId          { get; set; }
        public string   Description       { get; set; }
        public int      AddedBy           { get; set; }
        public bool     IsSystemGenerated { get; set; }
        public DateTime AddedDate         { get; set; }
        public int      ModifiedBy        { get; set; }
        public DateTime Modified          { get; set; }
        public bool IsArchived            { get; set; }

        // RELATIONSHIPS
        public virtual User.User User { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }
        public virtual ICollection<ReviewRemark> Reviews { get; set; }
        public virtual ICollection<GoalEvaluation> GoalEvaluations { get; set; }
        public virtual ICollection<CompetencyEvaluation> CompetencyEvaluations { get; set; }
        public virtual ICollection<EvaluationFeedbackResponse> EvaluationFeedbackResponses { get; set; }
    }
}
