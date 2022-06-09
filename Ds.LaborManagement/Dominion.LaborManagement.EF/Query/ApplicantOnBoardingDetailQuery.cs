using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantOnBoardingDetailQuery : Query<ApplicantOnBoardingDetail, IApplicantOnBoardingDetailQuery>, IApplicantOnBoardingDetailQuery
    {
        public ApplicantOnBoardingDetailQuery(IEnumerable<ApplicantOnBoardingDetail> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantOnBoardingDetailQuery IApplicantOnBoardingDetailQuery.ByApplicantOnBoardingDetailId(int applicantOnBoardingDetailId)
        {
            FilterBy(x => x.ApplicantOnBoardingDetailId == applicantOnBoardingDetailId);
            return this;
        }

        IApplicantOnBoardingDetailQuery IApplicantOnBoardingDetailQuery.ByApplicantOnBoardingHeaderId(int applicantOnBoardingHeaderId)
        {
            FilterBy(x => x.ApplicantOnBoardingHeaderId == applicantOnBoardingHeaderId);
            return this;
        }
    }
}