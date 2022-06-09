using System.Collections.Generic;

namespace Dominion.Core.Dto.Client
{
    public class ClientFeatureDto
    {
        public int ClientId { get; set; }
        public bool IsActive { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string PostalCode { get; set; }
        public bool HasLeaveManagement { get; set; }
        public ClientContactDto Contact { get; set; }
        public bool HasUnemploymentSetup { get; set; }
        public List<ClientAccountFeatureDto> AccountFeatures { get; set; }
    }
}
