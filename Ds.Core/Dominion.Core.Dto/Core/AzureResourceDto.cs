using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public class AzureResourceDto
    {
        public int ResourceId { get; set; }
        public string ResourceGuid { get; set; }
        public string Name { get; set; }

        // RELATIONSHIP
        public virtual ResourceDto Resource { get; set; }
    }
}
