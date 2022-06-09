using System.Collections.Generic;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantQuestionControlQuery : IQuery<ApplicantQuestionControl, IApplicantQuestionControlQuery>
    {
        IApplicantQuestionControlQuery BySectionId(int sectionId);
        IApplicantQuestionControlQuery ByQuestionId(int questionId);
        IApplicantQuestionControlQuery ByQuestionText(string questionText);
        IApplicantQuestionControlQuery ByQuestionIds(IEnumerable<int> questionIds);
        IApplicantQuestionControlQuery ByClientId(int clientId);
        IApplicantQuestionControlQuery ByFieldTypeId(FieldType fieldTypeId);
        IApplicantQuestionControlQuery ByIsActive(bool flag);
        IApplicantQuestionControlQuery ByApplication(IEnumerable<int> applicationIds);
        IQueryResult<ApplicantQuestionControlAndQuestionSets> getApplicationQuestionSets(
            IApplicantQuestionSetQuery questionSetQuery);

    }
}