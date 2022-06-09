using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Onboarding
{
    public class SchoolDistrictDto
    {
        public int SchoolDistrictId { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
