using Dominion.Core.Dto.Labor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    [Serializable]
    public class JobProfileBasicInfoDto
    {
        public int JobProfileId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Requirements { get; set; }
        public bool IsActive { get; set; }
        public string WorkingConditions { get; set; }
        public string Benefits { get; set; }
        public bool IsBenefitPortalOn { get; set; }
        public string EEOCLocation { get; set; }
        public string ClientDepartment { get; set; }

        public IEnumerable<JobProfileResponsibilitiesDto> JobProfileResponsibilities { get; set; }
        public IEnumerable<JobProfileSkillsDto> JobProfileSkills { get; set; }
    }
}