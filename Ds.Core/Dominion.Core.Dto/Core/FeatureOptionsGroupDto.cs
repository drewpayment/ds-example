using Dominion.Core.Dto.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public class FeatureOptionsGroupDto
    {
        public int FeatureOptionsGroupId { get; set; }
        public string Description { get; set; }
        public virtual IEnumerable<ClientAccountFeatureDto> FeatureOptions { get; set; }
    }
}
