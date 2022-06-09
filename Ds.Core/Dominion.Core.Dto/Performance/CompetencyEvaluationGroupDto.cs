using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class CompetencyEvaluationGroupDto
    {
        public int EvaluationGroupId { get; set; }
        public int? CompetencyGroupId { get; set; }
        public EvaluationGroupDto EvaluationGroup { get; set; }
        public CompetencyGroupDto CompetencyGroup { get; set; }
    }
}
