using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Clients
{
    public class BankAccountNextCheck : Entity<BankAccountNextCheck>
    {
        public virtual int BankAccountNextCheckId { get; set; }
        public virtual string RoutingNumber { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual int NextCheck { get; set; }
        public virtual int? ClientId { get; set; }
    }
}
