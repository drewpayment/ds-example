
using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    /// <summary>
    /// Stores the total score of an <see cref="EvaluationGroup"/> for an <see cref="Evaluation"/>
    /// </summary>
    public class EvaluationGroupEvaluation : Entity<EvaluationGroupEvaluation>
    {
        public int EvaluationGroupId { get; set; }
        public int EvaluationId { get; set; }
        public decimal Score { get; set; }
        public EvaluationGroup EvaluationGroup { get; set; }
        public Evaluation Evaluation { get; set; }
    }
}
