using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Onboarding
{
    public partial class ClientEssOptions
    {
        public virtual int ClientId { get; set; }
        public virtual int DirectDepositLimit { get; set; }
        public virtual bool IsAllowDirectDeposit { get; set; }
        public virtual bool IsAllowCheck { get; set; }
        public virtual bool IsAllowPaycard { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
    }
}
