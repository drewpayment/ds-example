using Dominion.Core.Dto.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class GoalWeightDto
    {
        public int      GoalId   { get; set; }
        public int EvaluationId { get; set; }
        public bool     OnReview { get; set; }
        public decimal? Weight   { get; set; }
        public ApprovalProcessStatus? ApprovalProcessStatus { get; set; }
        public RemarkDto NewRemark { get; set; }
    }
}
