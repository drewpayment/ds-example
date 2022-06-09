using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients
{
    public class GeneralLedgerGroupHeader : Entity<GeneralLedgerGroupHeader>
    {
        public virtual int    GeneralLedgerGroupHeaderId { get; set; }
        public virtual string Description                { get; set; }
        public virtual float  SequenceId                 { get; set; }
        public virtual int    GeneralLedgerGroupId       { get; set; }
        
    }
}
