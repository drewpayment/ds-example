using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Accounting
{
    public partial class GeneralLedgerAccount : Entity<GeneralLedgerAccount>, IHasModifiedData
    {
        public virtual int AccountId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Description { get; set; }
        public virtual string Number { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set;  }
    }
}
