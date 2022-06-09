using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Mapping;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantQuestionSetQuery : Query<ApplicantQuestionSet, IApplicantQuestionSetQuery>, IApplicantQuestionSetQuery
    {
        public ApplicantQuestionSetQuery(IEnumerable<ApplicantQuestionSet> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        public IApplicantQuestionSetQuery ByApplicationId(int applicationId)
        {
            FilterBy(x => x.ApplicationId == applicationId);
            return this;
        }

        public IApplicantQuestionSetQuery ByApplicationIds(List<int> appplicationIds)
        {
            FilterBy(x => appplicationIds.Contains(x.ApplicationId));
            return this;
        }

        public IApplicantQuestionSetQuery ByQuestionId(int questionId)
        {
            FilterBy(x => x.QuestionId == questionId);
            return this;
        }

        public IApplicantQuestionSetQuery ByExcludeQuestionIds(List<int> questionIds)
        {
            FilterBy(x => !questionIds.Contains(x.QuestionId));
            return this;
        }
    }
}
