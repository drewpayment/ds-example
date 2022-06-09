namespace Dominion.Core.Dto.Client
{
    public class ClientTurboTaxTrackingDto
    {
        public int ClientTurboTaxTrackingId { get; set; }
        public int ClientTurboTaxId { get; set; }
        public string TrackingId { get; set; }
        public ClientTurboTaxTrackingStatus TrackingStatusId { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }

    }
}
