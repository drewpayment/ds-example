using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Core.Dto.Location;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Core.Dto.Core;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantApplicationDetailQuery : Query<ApplicantApplicationDetail, IApplicantApplicationDetailQuery>, IApplicantApplicationDetailQuery
    {
        public ApplicantApplicationDetailQuery(IEnumerable<ApplicantApplicationDetail> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantApplicationDetailQuery IApplicantApplicationDetailQuery.ByApplicationDetailId(int applicationDetailId)
        {
            FilterBy(x => x.ApplicationDetailId == applicationDetailId);
            return this;
        }

        public IApplicantApplicationDetailQuery ByApplicationHeaderId(int applicationHeaderId)
        {
            FilterBy(x => x.ApplicationHeaderId == applicationHeaderId);
            return this;
        }
        public IApplicantApplicationDetailQuery ByApplicationIds(IEnumerable<int> applicationIds)
        {
            FilterBy(x => applicationIds.Contains(x.ApplicantApplicationHeader.ApplicantPosting.ApplicationId));
            return this;
        }
        public IApplicantApplicationDetailQuery ByApplicantId(int applicantId)
        {
            FilterBy(x => x.ApplicantApplicationHeader.ApplicantId == applicantId);
            return this;
        }
        public IApplicantApplicationDetailQuery ByFieldType(FieldType fieldType)
        {
            FilterBy(x => x.ApplicantQuestionControl.FieldTypeId == fieldType);
            return this;
        }
        public IApplicantApplicationDetailQuery ByQuestionId(int questionId)
        {
            FilterBy(x => x.QuestionId == questionId);
            return this;
        }
        public IApplicantApplicationDetailQuery BySectionId(int sectionId)
        {
            FilterBy(x => x.SectionId == sectionId);
            return this;
        }
        public IApplicantApplicationDetailQuery ByApplicationDateRange(DateTime startDate, DateTime endDate)
        {
            FilterBy(x =>   x.ApplicantApplicationHeader.DateSubmitted.Value >= startDate &&
                            x.ApplicantApplicationHeader.DateSubmitted.Value <= endDate);
            return this;
        }
		   public IApplicantApplicationDetailQuery ByQuestionId(int? questionId)
        {
            FilterBy(x => x.QuestionId == questionId);
            return this;
        }

        public IApplicantApplicationDetailQuery ByQuestionFieldType(FieldType fieldTypeId)
        {
            FilterBy(x => x.ApplicantQuestionControl.FieldTypeId == fieldTypeId);
            return this;
        }

        public IApplicantApplicationDetailQuery WhereQuestionIdIsNotNull()
        {
            FilterBy(x => x.QuestionId > 0);
            return this;
        }

        #region "Join Query between QuestionControl and Detail returning List of question ids"
        public IQueryResult<int> JoinApplicantQuestionControlWithDetail(IApplicantQuestionControlQuery query)
        {
            // Consider the Application Application that are completed
            FilterBy(x => x.ApplicantApplicationHeader.IsApplicationCompleted);

            return query.Result.InnerJoin( this.Result, new JoinQuestionControlAndQuestionDetail())
                .Group(new ApplicantQuestionControlsReferred())  ;
        }

        private class ApplicantQuestionControlsReferred : IGroupExpressions<int, int, int>
        {
            public Expression<Func<int, int>> GroupKey { get { return x => x; } }
            public Expression<Func<IGrouping<int, int>, int>> Select { get { return (x) => x.Key; } }
        }

        private class JoinQuestionControlAndQuestionDetail : IInnerJoinExpressions<ApplicantQuestionControl, ApplicantApplicationDetail, int, int>
        {
            public Expression<Func<ApplicantQuestionControl, int>> OuterKey
            {
                get { return qc => qc.QuestionId; }
            }

            public Expression<Func<ApplicantApplicationDetail, int>> InnerKey
            {
                get { return qD => qD.QuestionId.Value; }
            }

            public Expression<Func<ApplicantQuestionControl, ApplicantApplicationDetail, int>> Select
            {
                get { return (qc, qD) => qD.QuestionId.Value; }
            }
        }
        #endregion
    }
}