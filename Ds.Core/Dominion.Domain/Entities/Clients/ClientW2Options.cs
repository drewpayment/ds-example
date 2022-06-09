using Dominion.Domain.Entities.Base;
using System;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientW2Options : Entity<ClientW2Options>
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime TaxYear { get; set; }
        public byte PrintThirdPartyPayOnW2 { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
        public bool W2sReady { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string AnnualNotes { get; set; }
        public string MiscNotes { get; set; }
        public DateTime? TenNinetyNineDate { get; set; }
        public bool IsScheduled { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool ApprovedForClient { get; set; }
    }
}
