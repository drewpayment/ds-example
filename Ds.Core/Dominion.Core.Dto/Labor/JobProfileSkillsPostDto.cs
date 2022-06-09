using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class JobProfileSkillsPostDto
    {
        public int? JobSkillId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public int JobProfileId { get; set; }
    }
}
