using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Employee;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientRate : Entity<ClientRate>
    {
        public virtual int ClientRateId { get; set; } 
        public virtual int ClientId { get; set; } 
        public virtual string Description { get; set; } 
        public virtual string Code { get; set; } 
        public virtual DateTime? Modified { get; set; } 
        public virtual string ModifiedBy { get; set; } 
        
        //REVERSE NAVIGATION
        public virtual ICollection<EmployeeClientRate> EmployeeClientRate { get; set; } // many-to-one;


        public virtual Client Client { get; set; }
    }
}
