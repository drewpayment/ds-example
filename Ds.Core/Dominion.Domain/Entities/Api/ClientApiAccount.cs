using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Api
{
    public partial class ClientApiAccount : Entity<ClientApiAccount>
    {
        public virtual int ClientId { get; set; }
        public virtual int ApiAccountId { get; set; }

        //FOREIGN KEYS
        public virtual Client Client { get; set; }
        public virtual ApiAccount ApiAccount { get; set; }
    }
}