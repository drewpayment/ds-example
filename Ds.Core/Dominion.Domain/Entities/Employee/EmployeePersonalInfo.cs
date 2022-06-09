using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Employee
{
    public partial class EmployeePersonalInfo : Entity<EmployeePersonalInfo>, IHasModifiedData
    {
        public virtual int EmployeeId { get; set; }
        public virtual string Bio { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
