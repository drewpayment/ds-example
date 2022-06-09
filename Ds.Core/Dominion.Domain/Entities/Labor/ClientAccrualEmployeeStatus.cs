using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Labor
{
    public class ClientAccrualEmployeeStatus : Entity<ClientAccrualEmployeeStatus>
    {
        public virtual int    ClientAccrualEmployeeStatusId { get; set; }
        public virtual string Description                   { get; set; }
        public virtual bool   Active                        { get; set; }
    }
}
