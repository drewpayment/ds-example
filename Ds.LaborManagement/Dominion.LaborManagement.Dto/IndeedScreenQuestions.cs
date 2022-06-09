namespace Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply
{
    public class IndeedScreenQuestions
    {
        public IndeedAnswers Answers { get; set; }
        public IndeedQuestions Questions { get; set; }
        public long RetrievedOnMillis { get; set; }
        public string Url { get; set; }
    }
}