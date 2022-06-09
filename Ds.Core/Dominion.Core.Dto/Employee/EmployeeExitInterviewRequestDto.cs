using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    [Serializable]
    public class EmployeeExitInterviewRequestDto
    {
        public int EmployeeId { get; set; }
        public int RequestedBy { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime? SentOn { get; set; }
        public EmployeeBasicDto Employee { get; set; }
    }
}
