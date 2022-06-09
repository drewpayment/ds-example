using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply
{
    public class HierarchicalOption
    {
        public IndeedCondition Condition { get; set; }
        public string Id { get; set; }
        public IEnumerable<IndeedQuestionOption> Options { get; set; }
    }
}