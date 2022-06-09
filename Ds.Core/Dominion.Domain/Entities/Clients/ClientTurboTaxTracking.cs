using Dominion.Core.Dto.Client;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientTurboTaxTracking : Entity<ClientTurboTaxTracking>
    {
        public virtual int ClientTurboTaxTrackingId { get; set; }
        public virtual int ClientTurboTaxId { get; set; }
        public virtual string TrackingId { get; set; }
        public virtual ClientTurboTaxTrackingStatus TrackingStatusId { get; set; } 
        public virtual string Message { get; set; }
        public virtual string FileName { get; set; }

        //FOREIGN KEY
        public virtual ClientTurboTax ClientTurboTax { get; set; }
    }
}
