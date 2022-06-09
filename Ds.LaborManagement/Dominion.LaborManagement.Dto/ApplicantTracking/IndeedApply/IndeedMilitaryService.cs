namespace Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply
{
    public class IndeedMilitaryService
    {
        public string ServiceBranch { get; set; }
        public string Branch { get; set; }
        public string Rank { get; set; }
        public string Description { get; set; }
        public string Commendations { get; set; }
        public int? StartDateMonth { get; set; }
        public int? StartDateYear { get; set; }
        public int? EndDateMonth { get; set; }
        public int? EndDateYear { get; set; }
        public bool EndCurrent { get; set; }
    }
}