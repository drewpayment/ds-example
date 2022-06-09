using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantRejectionReasonQuery : Query<ApplicantRejectionReason, IApplicantRejectionReasonQuery>, IApplicantRejectionReasonQuery
    {
        #region Constructor

        public ApplicantRejectionReasonQuery(IEnumerable<ApplicantRejectionReason> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        IApplicantRejectionReasonQuery IApplicantRejectionReasonQuery.ByApplicantRejectionReasonId(int rejectionReasonId)
        {
            FilterBy(x => x.ApplicantRejectionReasonId == rejectionReasonId);
            return this;
        }

        IApplicantRejectionReasonQuery IApplicantRejectionReasonQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }

        IApplicantRejectionReasonQuery IApplicantRejectionReasonQuery.ByIsActive(bool isActive)
        {
            FilterBy(x => x.IsEnabled == isActive);
            return this;
        }
		
		public IApplicantRejectionReasonQuery OrderByDescription()
        {
            OrderBy(x => x.Description);
            return this;
        }

        IApplicantRejectionReasonQuery IApplicantRejectionReasonQuery.ByDescription(string descriptionText)
        {
            FilterBy(x => x.Description.ToLower().Contains(descriptionText.ToLower()));
            return this;
        }

        public IQueryResult<RejectionReasonsDto> JoinApplicantApplicationHeaderAndApplicantRejectionReason(
            IApplicantRejectionReasonQuery rejectionQuery, IApplicantApplicationHeaderQuery headerQuery)
        {
            return rejectionQuery.Result.InnerJoin(headerQuery.Result,
                new JoinApplicantApplicationHeaderAndApplicantRejectionReasonClass()).Group(new GroupByApplicantRejectionReasonIdAndDescription());
        }

        private class JoinApplicantApplicationHeaderAndApplicantRejectionReasonClass : IInnerJoinExpressions<ApplicantRejectionReason, ApplicantApplicationHeader, int, ApplicantApplicationHeaderAndApplicantRejectionReasonDto>
        {
            public Expression<Func<ApplicantRejectionReason, int>> OuterKey
            {
                get { return reason => reason.ApplicantRejectionReasonId; }
            }

            public Expression<Func<ApplicantApplicationHeader, int>> InnerKey
            {
                get { return header => header.ApplicantRejectionReasonId ?? -1; }
            }
            public Expression<Func<ApplicantRejectionReason, ApplicantApplicationHeader, ApplicantApplicationHeaderAndApplicantRejectionReasonDto>> Select
            {
                get { return (reason, header) => new ApplicantApplicationHeaderAndApplicantRejectionReasonDto()
                {
                    ApplicantRejectionReasonId = reason.ApplicantRejectionReasonId,
                    Description = reason.Description
                };}
            }
        }

        private class GroupByApplicantRejectionReasonIdAndDescription : IGroupExpressions<ApplicantApplicationHeaderAndApplicantRejectionReasonDto, GroupByApplicantRejectionReasonIdAndDescriptionKey, RejectionReasonsDto>
        {
            public Expression<Func<ApplicantApplicationHeaderAndApplicantRejectionReasonDto, GroupByApplicantRejectionReasonIdAndDescriptionKey>> GroupKey
            {
                get
                {
                    return (dto) => new GroupByApplicantRejectionReasonIdAndDescriptionKey()
                    {
                        ApplicantRejectionReasonID = dto.ApplicantRejectionReasonId,
                        Description = dto.Description
                    };
                }
            }

            public Expression<Func<IGrouping<GroupByApplicantRejectionReasonIdAndDescriptionKey, ApplicantApplicationHeaderAndApplicantRejectionReasonDto>, RejectionReasonsDto>> Select
            {
                get
                {
                    return (grouping) => new RejectionReasonsDto()
                    {
                        RejectionCount = grouping.Count(),
                        ApplicantRejectionReasonId = grouping.Key.ApplicantRejectionReasonID,
                        Description = grouping.Key.Description
                    };
                }
            }
        }

        private class GroupByApplicantRejectionReasonIdAndDescriptionKey
        {
            public int ApplicantRejectionReasonID {get; set; }
            public string Description { get; set; }
        }
    }
}