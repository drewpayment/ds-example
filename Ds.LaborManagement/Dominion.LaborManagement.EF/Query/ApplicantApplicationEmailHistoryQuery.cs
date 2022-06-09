using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Core.Dto.Location;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantApplicationEmailHistoryQuery : Query<ApplicantApplicationEmailHistory, IApplicantApplicationEmailHistoryQuery>, IApplicantApplicationEmailHistoryQuery
    {
        public ApplicantApplicationEmailHistoryQuery(IEnumerable<ApplicantApplicationEmailHistory> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantApplicationEmailHistoryQuery IApplicantApplicationEmailHistoryQuery.ByCompanyCorrespondenceId(int? correspondenceId)
        {
            FilterBy(x => x.ApplicantCompanyCorrespondenceId == correspondenceId);
            return this;
        }
        IApplicantApplicationEmailHistoryQuery IApplicantApplicationEmailHistoryQuery.ByApplicationHeaderId(int applicationHeaderId)
        {
            FilterBy(x => x.ApplicationHeaderId == applicationHeaderId);
            return this;
        }
        IApplicantApplicationEmailHistoryQuery IApplicantApplicationEmailHistoryQuery.ByEmailHistoryId(int applicationEmailHistoryId)
        {
            FilterBy(x => x.ApplicantApplicationEmailHistoryId == applicationEmailHistoryId);
            return this;
        }
        IApplicantApplicationEmailHistoryQuery IApplicantApplicationEmailHistoryQuery.BySenderId(int userId)
        {
            FilterBy(x => x.SenderId == userId);
            return this;
        }

    }
}