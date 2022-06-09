using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using System;
using System.Collections.Generic;
using ApplicantStatusType = Dominion.LaborManagement.Dto.ApplicantTracking.ApplicantStatusType;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantApplicationHeaderQuery : IQuery<ApplicantApplicationHeader, IApplicantApplicationHeaderQuery>
    {
        IApplicantApplicationHeaderQuery ByApplicationHeaderId(int applicationHeaderId);
        IApplicantApplicationHeaderQuery ByApplicantId(int applicantId);
        //IApplicantApplicationHeaderQuery ByPostingId(int postingId);
        IApplicantApplicationHeaderQuery ByApplicantStatusTypeId(ApplicantStatusType? applicantStatusTypeId);
        IApplicantApplicationHeaderQuery RejectableApplications();
        IApplicantApplicationHeaderQuery ByDateSubmitted(DateTime? startDate, DateTime? endDate);
        IApplicantApplicationHeaderQuery ByApplicationCompleted(bool flag);
        IApplicantApplicationHeaderQuery ByPostingId(int? postingId);
        IApplicantApplicationHeaderQuery ByApplicantPostingEnabled(bool flag);
        IApplicantApplicationHeaderQuery ByApplicantPostingClosed(bool flag);
        IApplicantApplicationHeaderQuery ByJobTypeId(int? jobTypeId);
        IApplicantApplicationHeaderQuery ByJobProfileId(int? jobProfileId);
        IApplicantApplicationHeaderQuery ByPostingCategoryId(int? postingCategoryId);
        IApplicantApplicationHeaderQuery ByPostingOwnerId(int? postingOwnerId);
        IApplicantApplicationHeaderQuery ByDivisionId(int? divisionId);
        IApplicantApplicationHeaderQuery ByDepartmentId(int? departmentId);
        IApplicantApplicationHeaderQuery ViewDenied(bool flag);
        IApplicantApplicationHeaderQuery ByKeyword(string keyword);
        IApplicantApplicationHeaderQuery ByName(string name);
        IApplicantApplicationHeaderQuery ByPostingNumber(int? postingNumber);
        IApplicantApplicationHeaderQuery ByApplicantPostingClientId(int? clientId);
        IApplicantApplicationHeaderQuery ByApplicantClientId(int? clientId);
        IApplicantApplicationHeaderQuery IgnoreNullStatus();
        IApplicantApplicationHeaderQuery OrderByDateSubmitted();
        IApplicantApplicationHeaderQuery ByApplicantIdIn(IEnumerable<int> ids);
        IApplicantApplicationHeaderQuery ByApplicantRejectionReasonId(int? applicantRejectionReasonId);
        IApplicantApplicationHeaderQuery ByRatingSelection(List<bool> ratings);
        IApplicantApplicationHeaderQuery ByExternalApplicationId(string externalApplicationId);
        IApplicantApplicationHeaderQuery ByCoverLetterIsNotNull();
    }
}