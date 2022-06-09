using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients
{
    public partial class SpecialInstruction : Entity<SpecialInstruction>
    {
        public virtual int InstructionId { get; set; }
        public virtual string Instruction { get; set; }
        public virtual string Code { get; set; }
        public virtual short Sequence { get; set; }
    }
}
