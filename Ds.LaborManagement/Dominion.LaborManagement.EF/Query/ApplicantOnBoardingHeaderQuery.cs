using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantOnBoardingHeaderQuery : Query<ApplicantOnBoardingHeader, IApplicantOnBoardingHeaderQuery>, IApplicantOnBoardingHeaderQuery
    {
        public ApplicantOnBoardingHeaderQuery(IEnumerable<ApplicantOnBoardingHeader> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantOnBoardingHeaderQuery IApplicantOnBoardingHeaderQuery.ByApplicantOnBoardingHeaderId(int applicantOnBoardingHeaderId)
        {
            FilterBy(x => x.ApplicantOnBoardingHeaderId == applicantOnBoardingHeaderId);
            return this;
        }

        IApplicantOnBoardingHeaderQuery IApplicantOnBoardingHeaderQuery.ByApplicantApplicationHeaderId(int applicantApplicationHeaderId)
        {
            FilterBy(x => x.ApplicantApplicationHeaderId == applicantApplicationHeaderId);
            return this;
        }
        IApplicantOnBoardingHeaderQuery IApplicantOnBoardingHeaderQuery.ByApplicantOnBoardingProcessId(int applicantOnBoardingProcessId)
        {
            FilterBy(x => x.ApplicantOnBoardingProcessId == applicantOnBoardingProcessId);
            return this;
        }

     IApplicantOnBoardingHeaderQuery IApplicantOnBoardingHeaderQuery.ByApplicantId(int applicantId)
        {
            FilterBy(x => x.ApplicantId == applicantId);
            return this;
        }
    }
}