namespace Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply
{
    public class IndeedResumeData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Headline { get; set; }
        public string Summary { get; set; }
        public string PublicProfileUrl { get; set; }
        public string AdditionalInfo { get; set; }
        public string PhoneNumber { get; set; }
        public IndeedLocation Location { get; set; }
        public string Skills { get; set; }
        public IndeedArray<IndeedPosition> Positions { get; set; }
        public IndeedArray<IndeedEducation> Educations { get; set; }
        public IndeedArray<IndeedLink> Links { get; set; }
        public IndeedArray<IndeedAward> Awards { get; set; }
        public IndeedArray<IndeedCertification> Certifications { get; set; }
        public IndeedArray<IndeedAssociation> Associations { get; set; }
        public IndeedArray<IndeedPatent> Patents { get; set; }
        public IndeedArray<IndeedPublication> Publications { get; set; }
        public IndeedArray<IndeedMilitaryService> MilitaryServices { get; set; }
    }
}