using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class Bank
    {
        public virtual int BankId { get; set; }
        public virtual string Name { get; set; }
        public virtual string RoutingNumber { get; set; }
        public virtual string Address { get; set; }
        public virtual string CheckSequence { get; set; }
        public virtual int? AchBankId { get; set; }
    }
}
