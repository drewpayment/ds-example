using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.User
{
    public class UserSession : Entity<UserSession>
    {
        public int UserId { get; set; }
        public DateTime? LastLogin { get; set; }
        public int? LastClientId { get; set; }
        public int? LastEmployeeId { get; set; }
        public string IpAddress { get; set; }
        public int? InvalidLoginAttempts { get; set; }

        public virtual User User { get; set; }
        public virtual Client LastClient { get; set; }
        public virtual Employee.Employee LastEmployee { get; set; }
    }
}
