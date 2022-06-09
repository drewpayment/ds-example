using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Labor;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.Notification;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Api.Notification
{
    public interface IApplicantTrackingNotificationService
    {
        IOpResult ProcessCorrespondenceNotification(ApplicantDetailDto applicant, int? correspondenceId, string correspondenceSubject, 
            string correspondenceBody, ApplicantTrackingCorrespondenceReplacementInfoDto replacementInfo, bool? isText = null);

        IOpResult ProcessPostingOwnerNotification(ApplicantPostingDetailDto postDetail, ApplicantDetailDto applicantDetail, Uri uri);
        IOpResult ProcessPostingOwnerApplicantNoteNotification(IEnumerable<ApplicantPostingOwnerDto> postOwners, ApplicantDetailDto applicantDetail);

        IOpResult ProcessHiringWorkflowTaskNotification();
        IOpResult ProcessSubmittedApplicationNotification(ApplicantDetailDto applicant, int? correspondenceId, string correspondenceSubject, string correspondenceBody, ApplicantTrackingCorrespondenceReplacementInfoDto replacementInfo,string smsText);
    }
}
