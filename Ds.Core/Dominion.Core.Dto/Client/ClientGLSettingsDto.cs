using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientGLSettingsDto
    {
        public int    ClientId             { get; set; }
        public byte?  Group2               { get; set; }
        public byte?  Group3               { get; set; }
        public byte?  Group2Type           { get; set; }
        public byte?  Group3Type           { get; set; }
        public bool   GroupClassesTogether { get; set; }
        
        // Entity References
        public virtual ClientDto Client { get; set; }
    }
}
