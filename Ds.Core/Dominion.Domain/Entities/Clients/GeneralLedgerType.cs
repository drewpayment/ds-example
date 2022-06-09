using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Clients
{
    public class GeneralLedgerType : Entity<GeneralLedgerType>
    {
        public virtual int    GeneralLedgerTypeId        { get; set; }
        public virtual string Description                { get; set; }
        public virtual int?   GeneralLedgerGroupId       { get; set; }
        public virtual int?   TaxTypeId                  { get; set; }
        public virtual float  SequenceId                 { get; set; }
        public virtual int?   GeneralLedgerGroupHeaderId { get; set; }
        public virtual bool   CanBeAccrued               { get; set; }
        public virtual bool   CanBeDetail                { get; set; }
        public virtual bool   CanBeOffset                { get; set; }
    }
}
