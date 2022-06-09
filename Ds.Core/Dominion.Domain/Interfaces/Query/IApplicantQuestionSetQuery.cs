using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantQuestionSetQuery : IQuery<ApplicantQuestionSet, IApplicantQuestionSetQuery>
    {
        IApplicantQuestionSetQuery ByApplicationId(int applicationId);
        IApplicantQuestionSetQuery ByApplicationIds(List<int> appplicationIds);
        IApplicantQuestionSetQuery ByQuestionId(int questionId);
        IApplicantQuestionSetQuery ByExcludeQuestionIds(List<int> questionIds);
    }
}
