using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Payroll
{
    public class PayrollNachaOpt : Entity<PayrollNachaOpt>
    {
        public virtual byte PayrollNachaOptId { get; set; }
        public virtual string PayrollNachaOpt_ { get; set; }
    }
}
