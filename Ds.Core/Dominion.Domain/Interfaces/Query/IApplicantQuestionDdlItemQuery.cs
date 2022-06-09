using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantQuestionDdlItemQuery : IQuery<ApplicantQuestionDdlItem, IApplicantQuestionDdlItemQuery>
    {
        IApplicantQuestionDdlItemQuery ByQuestionId(int questionId);
        IApplicantQuestionDdlItemQuery ByApplicantQuestionDdlItemId(int applicantQuestionDdlItemId);
        IApplicantQuestionDdlItemQuery ByIsActive(bool flag);
        IApplicantQuestionDdlItemQuery OrderByValue();
    }
}