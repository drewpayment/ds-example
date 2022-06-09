using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientIpSecurity : Entity<ClientIpSecurity>
    {
        public int ClientIpSecurityId { get; set; }
        public int ClientId { get; set; }
        public string IpAddress { get; set; }
        public byte TimeClockOnly { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public bool IsTimeClockOnly => TimeClockOnly == 1;
        public virtual User.User ModifiedByUser { get; set; }
        public virtual Client Client { get; set; }
    }
}
