namespace Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply
{
    public class IndeedApplication
    {
        public IndeedAnalytics Analytics { get; set; }
        public IndeedApplicant Applicant { get; set; }
        public long AppliedOnMillis { get; set; }
        public string Id { get; set; }
        public IndeedJob Job { get; set; }
        public string Locale { get; set; }
        public IndeedScreenerQuestions Questions { get; set; }
    }
}
