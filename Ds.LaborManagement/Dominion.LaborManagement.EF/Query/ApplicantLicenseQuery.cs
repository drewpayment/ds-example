using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantLicenseQuery : Query<ApplicantLicense, IApplicantLicenseQuery>, IApplicantLicenseQuery
    {
        public ApplicantLicenseQuery(IEnumerable<ApplicantLicense> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        public IApplicantLicenseQuery ByApplicantId(int applicantId)
        {
            FilterBy(x => x.ApplicantId == applicantId);
            return this;
        }

        public IApplicantLicenseQuery ByEnabled(bool isEnabled)
        {
            FilterBy(x => x.IsEnabled == isEnabled);
            return this;
        }

        public IApplicantLicenseQuery ByApplicantLicenseId(int id)
        {
            FilterBy(x => x.ApplicantLicenseId == id);
            return this;
        }
    }
}
