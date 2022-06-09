using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply
{
    public class IndeedScreenerQuestions
    {
        public IEnumerable<IndeedAnswer> Answers { get; set; }
        public IEnumerable<IndeedQuestion> Questions { get; set; }
        public long RetrievedOnMillis { get; set; }
        public string Url { get; set; }
    }
}