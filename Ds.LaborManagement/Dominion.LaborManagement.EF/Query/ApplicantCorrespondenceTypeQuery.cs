using System.Collections.Generic;
using System.Linq;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantCorrespondenceTypeQuery : Query<ApplicantCorrespondenceTypeInfo, IApplicantCorrespondenceTypeQuery>, IApplicantCorrespondenceTypeQuery
    {
        #region Constructor

        public ApplicantCorrespondenceTypeQuery(IEnumerable<ApplicantCorrespondenceTypeInfo> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        IApplicantCorrespondenceTypeQuery IApplicantCorrespondenceTypeQuery.OrderByApplicantCorrespondenceTypeId()
        {
            OrderBy(x => x.ApplicantCorrespondenceTypeId);
            return this;
        }

        IApplicantCorrespondenceTypeQuery IApplicantCorrespondenceTypeQuery.OrderByDescription()
        {
            OrderBy(x => x.Description);
            return this;
        }
    }
}