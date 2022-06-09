using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class SupervisorSecurityListByUserIDClientID
    {
        public class Security
        {
            public int UserSecurityGroupID { get; set; }
            public string Name { get; set; }
            public bool IsAllowed { get; set; }
            public bool Active { get; set; }
            public int ForeignKeyID { get; set; }
        }

        public IEnumerable<Security> List { get; set; }
    }
}
