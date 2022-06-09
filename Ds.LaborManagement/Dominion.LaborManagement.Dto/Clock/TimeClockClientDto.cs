namespace Dominion.LaborManagement.Dto.Clock
{
    public class TimeClockClientDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string StateId { get; set; }
    }
}