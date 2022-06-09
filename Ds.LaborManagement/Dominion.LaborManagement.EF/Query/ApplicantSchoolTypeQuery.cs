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
    public class ApplicantSchoolTypeQuery : Query<ApplicantSchoolType, IApplicantSchoolTypeQuery>, IApplicantSchoolTypeQuery
    {
        #region Constructor

        public ApplicantSchoolTypeQuery(IEnumerable<ApplicantSchoolType> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        IApplicantSchoolTypeQuery IApplicantSchoolTypeQuery.OrderByApplicantSchoolTypeId()
        {
            OrderBy(x => x.ApplicantSchoolTypeId);
            return this;
        }

        IApplicantSchoolTypeQuery IApplicantSchoolTypeQuery.OrderByDescription()
        {
            OrderBy(x => x.Description);
            return this;
        }

        IApplicantSchoolTypeQuery IApplicantSchoolTypeQuery.OrderByApplicationOrder()
        {
            OrderBy(x => x.ApplicationOrder);
            return this;
        }

        IApplicantSchoolTypeQuery IApplicantSchoolTypeQuery.BySchoolTypeId(int schoolTypeId)
        {
            FilterBy(x => x.ApplicantSchoolTypeId == schoolTypeId);
            return this;
        }
    }
}