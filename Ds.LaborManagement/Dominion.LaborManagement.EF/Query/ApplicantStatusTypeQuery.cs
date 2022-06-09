using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantStatusTypeQuery : Query<ApplicantStatusTypeDetail, IApplicantStatusTypeQuery>, IApplicantStatusTypeQuery
    {
        #region Constructor

        public ApplicantStatusTypeQuery(IEnumerable<ApplicantStatusTypeDetail> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        IApplicantStatusTypeQuery IApplicantStatusTypeQuery.ByStatusTypeId(Dto.ApplicantTracking.ApplicantStatusType statusTypeId)
        {
            FilterBy(x => x.ApplicantStatusId == statusTypeId);
            return this;
        }

        IApplicantStatusTypeQuery IApplicantStatusTypeQuery.ByIsActive(bool isActive)
        {
            FilterBy(x => x.IsActive == isActive);
            return this;
        }
    }
}