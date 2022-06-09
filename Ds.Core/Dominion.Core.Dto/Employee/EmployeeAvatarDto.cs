using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeAvatarDto
    {
        public int EmployeeAvatarId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public string AvatarColor { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
    }
}
