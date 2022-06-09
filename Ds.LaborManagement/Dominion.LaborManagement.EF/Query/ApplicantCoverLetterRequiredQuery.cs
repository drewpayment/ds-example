using System.Collections.Generic;
using System.Linq;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantCoverLetterRequiredQuery : Query<ApplicantCoverLetterRequired, IApplicantCoverLetterRequiredQuery>, IApplicantCoverLetterRequiredQuery
    {
        #region Constructor

        public ApplicantCoverLetterRequiredQuery(IEnumerable<ApplicantCoverLetterRequired> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        IApplicantCoverLetterRequiredQuery IApplicantCoverLetterRequiredQuery.OrderByApplicantCoverLetterRequiredId()
        {
            OrderBy(x => x.ApplicantCoverLetterRequiredId);
            return this;
        }

        IApplicantCoverLetterRequiredQuery IApplicantCoverLetterRequiredQuery.OrderByDescription()
        {
            OrderBy(x => x.Description);
            return this;
        }
    }
}