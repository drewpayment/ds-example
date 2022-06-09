using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Core
{
    public class AzureResource : Entity<AzureResource>
    {
        public int ResourceId { get; set; }
        public Guid ResourceGuid { get; set; }
        public string Name { get; set; }

        // RELATIONSHIP
        public virtual Resource Resource { get; set; }
    }
}
