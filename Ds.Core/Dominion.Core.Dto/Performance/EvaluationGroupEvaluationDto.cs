using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class EvaluationGroupEvaluationDto
    {
        public int EvaluationGroupId { get; set; }
        public int EvaluationId { get; set; }
        public decimal Score { get; set; }
    }
}
