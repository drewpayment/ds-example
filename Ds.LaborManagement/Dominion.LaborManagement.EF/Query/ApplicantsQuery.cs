using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantsQuery : Query<Applicant, IApplicantsQuery>, IApplicantsQuery
    {
        public ApplicantsQuery(IEnumerable<Applicant> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantsQuery IApplicantsQuery.ByApplicantId(int applicantId)
        {
            FilterBy(x => x.ApplicantId == applicantId);
            return this;
        }

        IApplicantsQuery IApplicantsQuery.ByUserId(int userId)
        {
            FilterBy(x => x.UserId == userId);
            return this;
        }
        IApplicantsQuery IApplicantsQuery.ByEmployeeId(int employeeId)
        {
            FilterBy(x => x.EmployeeId == employeeId);
            return this;
        }
        IApplicantsQuery IApplicantsQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }
    }
}
