using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public class GoalEvaluationGroup : IHasModifiedData
    {
        public int EvaluationGroupId { get; set; }
        public byte? GoalSourceTypeId { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public EvaluationGroup EvaluationGroup { get; set; }
        public User.User ModifiedByUser { get; set; }
    }
}
