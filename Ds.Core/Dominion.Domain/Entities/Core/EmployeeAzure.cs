using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Core
{
    public partial class EmployeeAzure : Entity<EmployeeAzure>
    {
        public virtual int EmployeeId { get; set; }
        public virtual string EmployeeGuid { get; set; }

        public virtual Employee.Employee Employee { get; set; }
    }
}
