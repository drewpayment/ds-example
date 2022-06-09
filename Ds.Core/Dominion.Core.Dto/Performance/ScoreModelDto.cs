using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class ScoreModelDto
    {

        public ScoreModelDto()
        {
            IsActive = true;
        }
        public int ScoreModelId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public bool IsScoreEmployeeViewable { get; set; }
        public bool HasScoreRange { get; set; }
        public int? SubTotalEvaluationGroupId { get; set; }
        public bool IsActive { get; set; }
        public int ScoreMethodTypeId { get; set; }
        public bool AdditionalEarnings { get; set; }
        public EvaluationGroupDto EvaluationGroup { get; set; }
        public ScoreMethodTypeDto ScoreMethodType { get; set; }
        public IEnumerable<ScoreRangeLimitDto> ScoreRangeLimits { get; set; }
        public bool EvaluationGroupsAreCustom { get; set; }

    }
}
