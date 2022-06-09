using System.Collections.Generic;
using System.Linq;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantResumeRequiredQuery : Query<ApplicantResumeRequired, IApplicantResumeRequiredQuery>, IApplicantResumeRequiredQuery
    {
        #region Constructor

        public ApplicantResumeRequiredQuery(IEnumerable<ApplicantResumeRequired> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        IApplicantResumeRequiredQuery IApplicantResumeRequiredQuery.OrderByApplicantResumeRequiredId()
        {
            OrderBy(x => x.ApplicantResumeRequiredId);
            return this;
        }

        IApplicantResumeRequiredQuery IApplicantResumeRequiredQuery.OrderByDescription()
        {
            OrderBy(x => x.Description);
            return this;
        }
    }
}