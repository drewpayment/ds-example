using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class InitializeAppHistoryDto
    {
        public ApplicantPostingSchoolRequirementDto SchoolRequirementDto { get; set; }
        public IEnumerable<ApplicantEmploymentHistoryDto> EmploymentHistoryDtos { get; set; }
        public IEnumerable<ApplicantEducationHistoryDto> ApplicantEducationHistoryDtos { get; set; }
        public IEnumerable<ApplicantLicenseDto> LicenseDtos { get; set; }
        public int YearsOfEmployment { get; set; }
        public bool? isExperience { get; set; }
    }
}
