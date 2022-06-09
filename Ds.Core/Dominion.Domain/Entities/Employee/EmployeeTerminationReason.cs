using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;



namespace Dominion.Domain.Entities.Employee
{
    public class EmployeeTerminationReason : Entity<EmployeeTerminationReason>
    {
        public virtual int          EmployeeTerminationReasonID         { get; set; }
        public virtual int          EmployeeTerminationReasonTypeID     { get; set; }
        public virtual string       Description                         { get; set; }
        public EmployeeTerminationReason()
        {
        }
    }
}
