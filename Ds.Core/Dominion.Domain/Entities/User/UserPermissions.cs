using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.User
{
    public class UserPermissions : Entity<UserPermissions>
    {
        public int UserId { get; set; }
        public bool IsEmployeeNavigatorAdmin { get; set; }
        public virtual User User { get; set; }
    }
}
