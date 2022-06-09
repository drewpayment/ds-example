using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Employee
{
    public class EmployeeExitInterviewRequest : Entity<EmployeeExitInterviewRequest>
    {
        public int EmployeeId { get; set; }
        public int RequestedBy { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime? SentOn { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
