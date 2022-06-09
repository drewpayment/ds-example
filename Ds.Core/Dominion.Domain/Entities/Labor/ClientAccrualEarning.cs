using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Payroll;

namespace Dominion.Domain.Entities.Labor
{
    public partial class ClientAccrualEarning : Entity<ClientAccrualEarning>
    { 
        public virtual int ClientAccrualId { get; set; }
        public virtual int ClientAccrualEarningId { get; set; }
        public ClientAccrual ClientAccrual { get; set; }
        public ClientEarning ClientEarning { get; set; }
    }
}
