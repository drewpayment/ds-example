using System.Collections.Generic;
using Dominion.Core.Dto.Employee.Search;

namespace Dominion.Core.Dto.Performance
{
    public class ReviewStatusDto
    {
        public ReviewStatus                   Status            { get; set; }
        public ReviewDto                      Review            { get; set; }
        public EmployeeSearchDto              Employee          { get; set; }
        public bool                           IsSetupIncomplete { get; set; }
        public ScoreGroup                     Score             { get; set; }
        public IEnumerable<EvaluationStatusDto> EvaluationStatuses { get; set; }
    }
}