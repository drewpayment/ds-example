using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Onboarding
{
    public class SchoolDistrict : Entity<SchoolDistrict>
    {
        public virtual int SchoolDistrictId { get; set; }
        public virtual int StateId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }

        public virtual ICollection<EmployeeOnboardingW4> EmployeeOnboardingW4 { get; set; }
    }
}
