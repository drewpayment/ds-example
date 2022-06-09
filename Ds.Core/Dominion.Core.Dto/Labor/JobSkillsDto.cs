using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class JobSkillsDto
    {
        public int JobSkillId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public bool isSelect { get; set; } = false;
        public bool isSkillEditing { get; set; } = false;
        public string EditDescription { get; set; }

    }
}
