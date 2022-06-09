using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public enum ApplicantStatusType : byte
    {
        Applicant = 0,
        Candidate = 1,
        ReadyForOnboarding = 2,
        Rejected = 3,
        Incomplete = 4,
        Hired = 5,
        Reviewed = 6
    }
}