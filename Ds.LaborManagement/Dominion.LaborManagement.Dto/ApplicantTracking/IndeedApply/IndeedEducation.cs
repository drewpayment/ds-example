namespace Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply
{
    public class IndeedEducation
    {
        public string Degree { get; set; }
        public string Field { get; set; }
        public string School { get; set; }
        public string Location { get; set; }
        public int? StartDateYear { get; set; }
        public int? EndDateYear { get; set; }
        public bool EndCurrent { get; set; }
    }
}