namespace Dominion.Core.Dto.Location
{
    public class ReciprocalStateDto
    {
        public int StateId { get; set; }
        public int ReciprocalStateId { get; set; }

        public string StateName { get; set; }
        public string ReciprocalStateName { get; set; }
    }
}