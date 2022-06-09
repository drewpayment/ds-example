using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantCompanyCorrespondenceQuery : Query<ApplicantCompanyCorrespondence, IApplicantCompanyCorrespondenceQuery>, IApplicantCompanyCorrespondenceQuery
    {
        #region Constructor

        public ApplicantCompanyCorrespondenceQuery(IEnumerable<ApplicantCompanyCorrespondence> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        IApplicantCompanyCorrespondenceQuery IApplicantCompanyCorrespondenceQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }

        IApplicantCompanyCorrespondenceQuery IApplicantCompanyCorrespondenceQuery.ByCorrespondenceTypeId(Dto.ApplicantTracking.ApplicantCorrespondenceType? correspondenceTypeId)
        {
            if (correspondenceTypeId != null)
            {
                FilterBy(x => (Dto.ApplicantTracking.ApplicantCorrespondenceType)x.ApplicantCorrespondenceTypeId == correspondenceTypeId);
            }
            return this;
        }

        IApplicantCompanyCorrespondenceQuery IApplicantCompanyCorrespondenceQuery.ByIsActive(bool isActive)
        {
            FilterBy(x => x.IsActive == isActive);
            return this;
        }

        IApplicantCompanyCorrespondenceQuery IApplicantCompanyCorrespondenceQuery.ByIsText(bool isText)
        {
            FilterBy(x => (!isText && x.IsText == null) ||  x.IsText == isText);
            return this;
        }

        IApplicantCompanyCorrespondenceQuery IApplicantCompanyCorrespondenceQuery.OrderByApplicantCompanyCorrespondenceId()
        {
            OrderBy(x => x.ApplicantCompanyCorrespondenceId);
            return this;
        }

        IApplicantCompanyCorrespondenceQuery IApplicantCompanyCorrespondenceQuery.OrderByDescription()
        {
            OrderBy(x => x.Description);
            return this;
        }
        IApplicantCompanyCorrespondenceQuery IApplicantCompanyCorrespondenceQuery.ByCorrespondenceId(int correspondenceId)
        {
            FilterBy(x => x.ApplicantCompanyCorrespondenceId == correspondenceId);
            return this;
        }
    }
}