namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicationSectionDto
    {
        public int SectionID { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; }
        public ApplicantQuestionControlDto Question { get; set; }
    }
}
