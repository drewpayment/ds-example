using Dominion.Core.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public class UserNameDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? UserId { get; set; }
        public bool  IsActive { get; set; }
        public int EmployeeId { get; set; }
        public bool IsDirectSupervisor { get; set; }
        public bool IsUserDisabled { get; set; }
        public UserType UserType { get; set; }
    }
}
