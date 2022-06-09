using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class FeatureOptionDto
    {
        public int FeatureOptionId { get; set; }
        public string Description { get; set; }
        public bool IsClientEnabled { get; set; }
    }
}
