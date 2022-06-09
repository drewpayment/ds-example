using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.Enums;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantPostingOwnerQuery : Query<ApplicantPostingOwner, IApplicantPostingOwnerQuery>, IApplicantPostingOwnerQuery
    {
        public ApplicantPostingOwnerQuery(IEnumerable<ApplicantPostingOwner> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantPostingOwnerQuery IApplicantPostingOwnerQuery.ByPostingId(int postingId)
        {
            FilterBy(x => x.PostingId == postingId);
            return this;
        }

        public IApplicantPostingOwnerQuery ByUserId(int userId)
        {
            FilterBy(x => x.UserId == userId);
            return this;
        }
    }
}