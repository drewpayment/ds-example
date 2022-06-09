namespace Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply
{
    public class IndeedResume
    {
        public IndeedFilePropertyBag File { get; set; }
        public string Text { get; set; }
        public string HrXml { get; set; }
        public string Html { get; set; }
        /// <summary>
        /// Indeed's detailed resume for the applicant if the applicant has created an 'Indeed Resume'
        /// </summary>
        public IndeedResumeData Json { get; set; }
    }
}