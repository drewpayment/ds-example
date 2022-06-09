using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Services.Api.Notification;
using Dominion.Core.Services.Interfaces;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.Notification;
using Dominion.LaborManagement.Service.Internal.Notification;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.Utility.OpResult;
using Dominion.Utility.Security;

namespace Dominion.LaborManagement.Service.Api.Notification
{
    class ApplicantTrackingNotificationService : IApplicantTrackingNotificationService
    {
        private readonly IBusinessApiSession _session;
        private readonly INotificationService _notifyService;
        private readonly IClientService _clientService;

        public ApplicantTrackingNotificationService(IBusinessApiSession session, INotificationService notifyService, IClientService clientService)
        {
            _session = session;
            _notifyService = notifyService;
            _clientService = clientService;
        }

        /// <summary>
        /// Sends 
        /// </summary>
        /// <returns></returns>
        IOpResult IApplicantTrackingNotificationService.ProcessCorrespondenceNotification(ApplicantDetailDto applicant, int? correspondenceId, string correspondenceSubject,string correspondenceBody, ApplicantTrackingCorrespondenceReplacementInfoDto replacementInfo, bool? isText)
        {
            var result = new OpResult();

            var builder = new ApplicantTrackingCorrespondenceNotificationBuilder(_session, applicant,
                                            correspondenceId, correspondenceSubject, correspondenceBody, replacementInfo, _clientService, isText);

            _notifyService.Notify(builder).MergeInto(result);
            
            return result;
        }

        IOpResult IApplicantTrackingNotificationService.ProcessPostingOwnerNotification(ApplicantPostingDetailDto postDetail, ApplicantDetailDto applicantDetail, Uri uri)
        {
            var result = new OpResult();

            if (result.Success)
            {
                var builder = new ApplicantTrackingPostingOwnerNotificationBuilder(_session, postDetail, applicantDetail, uri, _clientService);

                _notifyService.Notify(builder).MergeInto(result);
            }

            return result;
        }

        IOpResult IApplicantTrackingNotificationService.ProcessPostingOwnerApplicantNoteNotification(IEnumerable<ApplicantPostingOwnerDto> postOwners, ApplicantDetailDto applicantDetail)
        {
            var result = new OpResult();

            if (result.Success && _notifyService.TryToNotify(Core.Dto.Notification.NotificationType.NewNoteOnApplicant, _session.LoggedInUserInformation.ClientId).Data )
            {
                var builder = new ApplicantTrackingPostingOwnerApplicantNoteNotificationBuilder(_session, postOwners, applicantDetail,  _clientService);

                _notifyService.Notify(builder).MergeInto(result);
            }

            return result;
        }

        IOpResult IApplicantTrackingNotificationService.ProcessHiringWorkflowTaskNotification()
        {
            var result = new OpResult();

            _session.CanPerformAction(ApplicantTrackingActionType.ApplicantAdmin).MergeInto(result);

            if (result.Success)
            {
                var builder = new ApplicantTrackingHiringWorkflowTaskNotificationBuilder(_session);

                _notifyService.Notify(builder).MergeInto(result);
            }

            return result;
        }

        IOpResult IApplicantTrackingNotificationService.ProcessSubmittedApplicationNotification(ApplicantDetailDto applicant, int? correspondenceId, string correspondenceSubject, string correspondenceBody, ApplicantTrackingCorrespondenceReplacementInfoDto replacementInfo, string smsText)
        {
            var result = new OpResult();

            var isSubmitApplicationEnable = _session.UnitOfWork.NotificationRepository
                                                    .QueryClientNotificationPreferences()
                                                    .ByClientId(_session.LoggedInUserInformation.ClientId.Value)
                                                    .ByNotificationType(Core.Dto.Notification.NotificationType.SubmittedApplication)
                                                    .ExecuteQueryAs(x => x.IsEnabled)
                                                    .FirstOrDefault();

            if (isSubmitApplicationEnable)
            {
                var builder = new ApplicantTrackingSubmittedApplicationNotificationBuilder(_session, applicant,
                                                correspondenceId, correspondenceSubject, correspondenceBody, replacementInfo, _clientService, smsText);

                _notifyService.Notify(builder).MergeInto(result);
            }
            return result;
        }
    }
}
