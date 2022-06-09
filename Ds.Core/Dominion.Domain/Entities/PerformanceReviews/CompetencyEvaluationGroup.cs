using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    /// <summary>
    /// Connects a competency to a client's <see cref="EvaluationGroup"/>.  This is used when
    /// determining the score of an evaluation and creating a the 'breakdown' of that score
    /// </summary>
    public class CompetencyEvaluationGroup : IHasModifiedData
    {
        public int EvaluationGroupId { get; set; }
        public int? CompetencyGroupId { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public User.User ModifiedByUser { get; set; }
        public virtual EvaluationGroup EvaluationGroup { get; set; }
        public virtual CompetencyGroup CompetencyGroup { get; set; }
    }
}
