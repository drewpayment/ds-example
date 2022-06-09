using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantLicenseQuery : IQuery<ApplicantLicense, IApplicantLicenseQuery>
    {
        IApplicantLicenseQuery ByApplicantId(int applicantId);
        IApplicantLicenseQuery ByEnabled(bool isEnabled);
        IApplicantLicenseQuery ByApplicantLicenseId(int id);
    }
}
