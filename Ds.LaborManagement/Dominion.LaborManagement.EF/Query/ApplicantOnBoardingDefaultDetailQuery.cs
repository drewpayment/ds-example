using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantOnBoardingDefaultDetailQuery : Query<ApplicantOnBoardingDefaultDetail, IApplicantOnBoardingDefaultDetailQuery>, IApplicantOnBoardingDefaultDetailQuery
    {
        public ApplicantOnBoardingDefaultDetailQuery(IEnumerable<ApplicantOnBoardingDefaultDetail> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantOnBoardingDefaultDetailQuery IApplicantOnBoardingDefaultDetailQuery.ByApplicantOnBoardingDefaultDetailId(int applicantOnBoardingDefaultDetailId)
        {
            FilterBy(x => x.ApplicantOnBoardingDefaultDetailId == applicantOnBoardingDefaultDetailId);
            return this;
        }

        IApplicantOnBoardingDefaultDetailQuery IApplicantOnBoardingDefaultDetailQuery.ByApplicantOnBoardingProcessId(int applicantOnBoardingProcessId)
        {
            FilterBy(x => x.ApplicantOnBoardingProcessId == applicantOnBoardingProcessId);
            return this;
        }

        IApplicantOnBoardingDefaultDetailQuery IApplicantOnBoardingDefaultDetailQuery.ByApplicantPostingId(int applicantPostingId)
        {
            FilterBy(x => x.ApplicantPostingId == applicantPostingId);
            return this;
        }
    }
}