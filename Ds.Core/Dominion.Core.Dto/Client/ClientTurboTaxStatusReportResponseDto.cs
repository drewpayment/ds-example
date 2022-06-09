namespace Dominion.Core.Dto.Client
{
    public class ClientTurboTaxStatusReportResponseDto
    {
        public string trackingId   { get; set; }
        public string uploadStatus { get; set; }
        public string uploadType   { get; set; }
        public string response     { get; set; }
    }
}
