using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantQuestionDdlItemQuery : Query<ApplicantQuestionDdlItem, IApplicantQuestionDdlItemQuery>, IApplicantQuestionDdlItemQuery
    {
        public ApplicantQuestionDdlItemQuery(IEnumerable<ApplicantQuestionDdlItem> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantQuestionDdlItemQuery IApplicantQuestionDdlItemQuery.ByApplicantQuestionDdlItemId(int applicantQuestionDdlItemId)
        {
            FilterBy(x => x.ApplicantQuestionDdlItemId == applicantQuestionDdlItemId);
            return this;
        }

        IApplicantQuestionDdlItemQuery IApplicantQuestionDdlItemQuery.ByQuestionId(int questionId)
        {
            FilterBy(x => x.QuestionId == questionId);
            return this;
        }

        IApplicantQuestionDdlItemQuery IApplicantQuestionDdlItemQuery.ByIsActive(bool flag)
        {
            FilterBy(x => x.IsEnabled == flag);
            return this;
        }

        IApplicantQuestionDdlItemQuery IApplicantQuestionDdlItemQuery.OrderByValue()
        {
            OrderBy(x => x.Value);
            return this;
        }
    }
}
