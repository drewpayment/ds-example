using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Core;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantApplicationDetailQuery : IQuery<ApplicantApplicationDetail, IApplicantApplicationDetailQuery>
    {
        IApplicantApplicationDetailQuery ByApplicationDetailId(int applicationDetailId);
        IApplicantApplicationDetailQuery ByApplicationHeaderId(int applicationHeaderId);
        IApplicantApplicationDetailQuery BySectionId(int sectionId);
        IApplicantApplicationDetailQuery ByQuestionId(int? questionId);
        IApplicantApplicationDetailQuery ByQuestionFieldType(FieldType fieldTypeId);
        IApplicantApplicationDetailQuery ByApplicationDateRange(DateTime startDate, DateTime endDate);
        IApplicantApplicationDetailQuery ByApplicationIds(IEnumerable<int> applicationIds);
        IApplicantApplicationDetailQuery ByApplicantId(int applicantId);
        IApplicantApplicationDetailQuery ByFieldType(FieldType fieldType);
        IApplicantApplicationDetailQuery WhereQuestionIdIsNotNull();
        IQueryResult<int> JoinApplicantQuestionControlWithDetail(IApplicantQuestionControlQuery query);
    }
}