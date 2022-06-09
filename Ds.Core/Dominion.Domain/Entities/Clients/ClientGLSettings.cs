using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientGLSettings : Entity<ClientGLSettings>
    {
        public virtual int    ClientId             { get; set; }
        public virtual byte?  Group2               { get; set; }
        public virtual byte?  Group3               { get; set; }
        public virtual byte?  Group2Type           { get; set; }
        public virtual byte?  Group3Type           { get; set; }
        public virtual bool   GroupClassesTogether { get; set; }
        
        // Entity References
        public virtual Client Client { get; set; }
    }
}
