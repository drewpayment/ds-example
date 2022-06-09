using Dominion.Domain.Entities.Banks;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientBank : Entity<ClientBank>
    {
        public virtual int    ClientId { get; set; }
        public virtual int    BankId   { get; set; }
        public virtual Client Client   { get; set; }
        public virtual Bank   Bank     { get; set; }
    }
}
