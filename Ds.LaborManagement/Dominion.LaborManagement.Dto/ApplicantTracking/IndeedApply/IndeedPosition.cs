namespace Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply
{
    public class IndeedPosition
    {
        public string Title { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int? StartDateMonth { get; set; }
        public int? StartDateYear { get; set; }
        public int? EndDateMonth { get; set; }
        public int? EndDateYear { get; set; }
        public bool EndCurrent { get; set; }
    }
}