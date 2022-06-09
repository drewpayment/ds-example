using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class JobProfileResponsibilitiesDto
    {
        public int JobProfileId { get; set; }
        public int JobResponibilityId { get; set; }
        public string Description { get; set; }
    }
}
