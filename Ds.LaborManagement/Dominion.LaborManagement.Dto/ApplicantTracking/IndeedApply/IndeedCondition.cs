using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply
{
    public class IndeedCondition
    {
        public bool Exclude { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}