using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Banks
{
    public partial class Bank : Entity<Bank>
    {
        public virtual int    BankId        { get; set; }
        public virtual string Name          { get; set; }
        public virtual string RoutingNumber { get; set; }
        public virtual string Address       { get; set; }
        public virtual string CheckSequence { get; set; }
        public virtual int?   AchBankId     { get; set; }

        public virtual ICollection<ClientBank> ClientBanks { get; set; }
    }
}
