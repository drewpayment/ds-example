using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Contact;

namespace Dominion.Domain.Entities.EEOC
{
    public class EEOCLocation : Entity<EEOCLocation>
    {
        public virtual int    EeocLocationId { get; set; }
        public virtual string EeocLocationDescription { get; set; }
        public virtual int    ClientId { get; set; }
        public virtual bool   IsActive { get; set; }
        public virtual int?   UnitAddressId { get; set; }
        public virtual string UnitNumber { get; set; }
        public virtual bool   IsHeadquarters { get; set; }

        //FOREIGN KEYS
        public virtual Address UnitAddress { get; set; }
        public virtual Client Client { get; set; } 
        public virtual ICollection<Employee.Employee> Employees { get; set; } // many-to-one;


    }
}
