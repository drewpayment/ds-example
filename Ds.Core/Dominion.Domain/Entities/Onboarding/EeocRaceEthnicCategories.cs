using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Onboarding
{
    public partial class EeocRaceEthnicCategories
    {
        public virtual int RaceId { get; set; }
        public virtual string Description { get; set; }
        public virtual string DetailedDescription { get; set; }

        public virtual ICollection<Employee.Employee> Employees { get; set; } // many-to-one;
    }
}
