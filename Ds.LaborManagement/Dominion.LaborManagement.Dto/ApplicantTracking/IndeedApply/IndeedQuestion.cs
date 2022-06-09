using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply
{
    public class IndeedQuestion
    {
        
        public IEnumerable<HierarchicalOption> HierarchicalOptions { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }
        public IEnumerable<IndeedQuestionOption> Options { get; set; }
        public string Value { get; set; }
        public string Label { get; set; }
        public bool? Required { get; set; }
        public string Format { get; set; }
        public int? Limit { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public IndeedCondition Condition { get; set; }
        public string Type { get; set; }
        public string Question { get; set; }
        }
}