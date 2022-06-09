using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public partial class ApplicantSkillDto
    {
        public int ApplicantSkillId { get; set; }
        public int ApplicantId { get; set; }
        public string Name { get; set; }
        public byte Rating { get; set; }
        public decimal? Experience { get; set; }
        public string ExperienceType { get; set; }
        public bool IsEnabled { get; set; }
    }
}
