namespace Dominion.Core.Dto.Performance
{
    public class ReviewProfileBasicDto
    {
        public int    ReviewProfileId { get; set; }
        public int    ClientId        { get; set; }
        public string Name            { get; set; }
        public bool   IsArchived      { get; set; }
    }
}