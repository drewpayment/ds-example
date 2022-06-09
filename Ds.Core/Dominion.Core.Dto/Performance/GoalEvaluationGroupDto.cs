using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class GoalEvaluationGroupDto
    {
        public int EvaluationGroupId { get; set; }
        public byte? GoalSourceTypeId { get; set; }
        public EvaluationGroupDto EvaluationGroup { get; set; }
    }
}
