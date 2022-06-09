namespace Dominion.LaborManagement.Dto.ApplicantTracking.Application
{
    public class QuestionItemOptionDto
    {
        public int    ItemId          { get; set; }
        public string Value           { get; set; }
        public string Description     { get; set; }
        public bool   IsDefault       { get; set; }
        public int?   FlaggedResponse { get; set; }
        public bool   IsEnabled       { get; set; }
    }
}
