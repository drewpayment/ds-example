namespace Dominion.Core.Dto.W2
{
    public class ClientW2ProcessingNotesDto
    {
        public int ClientId { get; set; }
        public int Year { get; set; }
        public string AnnualNotes { get; set; }
        public string MiscNotes { get; set; }
    }
}
