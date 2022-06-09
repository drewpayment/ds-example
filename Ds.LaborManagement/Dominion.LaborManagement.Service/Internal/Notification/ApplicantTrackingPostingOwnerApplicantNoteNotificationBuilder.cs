using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Notification;
using Dominion.Core.Services.Api.Notification;
using Dominion.Core.Services.Internal.Providers.Notifications;
using Dominion.Core.Services.Interfaces;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.Notification;
using Dominion.Utility.Constants;
using Dominion.Utility.Configs;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.OpResult;
using Dominion.Core.Dto.Client;
using Newtonsoft.Json;
using EmailDto = Dominion.Core.Dto.Notification.EmailDto;

namespace Dominion.LaborManagement.Service.Internal.Notification
{
    public class ApplicantTrackingPostingOwnerApplicantNoteNotificationBuilder : INotificationBuilder
    {
        private readonly IBusinessApiSession _session;
        private readonly IEnumerable<ApplicantPostingOwnerDto> _postOwners;
        private readonly ApplicantDetailDto _applicantDetail;
        private readonly IClientService _clientService;

        private ClientSMTPSettingDto _clientSMTP;

        NotificationType INotificationBuilder.NotificationType => NotificationType.NewNoteOnApplicant;

        string INotificationBuilder.NotificationDetails => GetNotificationDetails();

        public ApplicantTrackingPostingOwnerApplicantNoteNotificationBuilder(IBusinessApiSession session,
            IEnumerable<ApplicantPostingOwnerDto> postOwners,
            ApplicantDetailDto applicantDetail, 
            IClientService clientService)
        {
            _session = session;
            _postOwners = postOwners;
            _applicantDetail = applicantDetail;
            _clientService = clientService;
        }

        IOpResult<IEnumerable<INotificationRecipient>> INotificationBuilder.BuildNotifications()
        {
            // Fetch client smtp settings if present
            var setting = _clientService.GetClientSMTPSetting(_session.LoggedInUserInformation.ClientId.Value);
            if (setting.Success && setting.HasData)
                _clientSMTP = setting.Data;

            var result = new OpResult<IEnumerable<INotificationRecipient>>();

            result.TryCatch(() =>
            {
                var postingOwnerInfo = _session.UnitOfWork.UserRepository
                    .QueryUsers()
                    .ByUserIds(_postOwners.Select(x=> x.UserId).ToArray())
                    .ExecuteQueryAs(u => new
                    {
                        u.UserId,
                        FullName = u.FirstName + " " + u.LastName
                    })
                    .ToOrNewList();

                if (postingOwnerInfo.Any())
                {
                    var recipients = postingOwnerInfo
                        .GroupBy(r => new {r.UserId})
                        .Select(group => new NotificationRecipient
                        {
                            UserId = group.Key.UserId,
                            Notifications = group.Select(x => new NotificationContentBuilder
                            {
                                EmailBuilder = GetEmail,
                                SmsBuilder = GetSms
                            })
                        })
                        .ToOrNewList();

                    result.Data = recipients;
                }
            });

            return result;

        }

        private EmailDto GetEmail()
        {
            var email = new EmailDto
            {
                Subject = "Note entered for " + _applicantDetail.Posting + " Applicant.",                
                Body = BuildBody(),
                IsBodyHtml = true,
                ClientSMTPSetting = _clientSMTP
            };

            return email;
        }

        private string BuildBody()
        {
            var body = new StringBuilder();

            body.Append("A note was entered for "+ _applicantDetail.ApplicantName + " on "+ _applicantDetail.Posting + ".");
            body.Append(NotificationConstants.LINE_BREAK_TAG);
            body.Append(NotificationConstants.LINE_BREAK_TAG);
            body.Append(GetEmailLink() + " to Dominion to see the note.");
            body.Append(NotificationConstants.LINE_BREAK_TAG);
            body.Append(NotificationConstants.LINE_BREAK_TAG);
            body.Append("You are receiving this email because you are the Post Owner.".Italicize());            

            return body.ToString().Trim();
        }

        private string GetEmailLink()
        {
            var hostApp = new StringBuilder();
            hostApp.Append(ConfigValues.LegacyRootUrl);
            // Ensure there is a '/' at the end of the string
            if (hostApp.ToString().Trim().Last() != '/')
                hostApp.Append("/");

            hostApp.Append(NotificationConstants.APPLICANTTRACKING_QUALIFY_APPLICANTS_URL);

            string link = "<a href='";
            link+= ($"{hostApp.ToString()}' target='_blank'>{NotificationConstants.SIGN_IN}</a>");

            return link;
        }

        private SmsDto GetSms()
        {
            var sms = new SmsDto
            {
                Message = BuildSmsMessage()
            };

            return sms;
        }

        private string BuildSmsMessage()
        {
            var message = new StringBuilder();

            message.Append("Note entered for " + _applicantDetail.Posting + " Applicant.");
            
            return message.ToString().Trim();
        }

        private string GetNotificationDetails()
        {
            var dto = new ApplicantTrackingPostingOwnerNotificationDetailDto
            {
                PostingId = _applicantDetail.PostingId,
                PostingDescription = _applicantDetail.Posting,
                ApplicantId = _applicantDetail.ApplicantId,
                ApplicantName = _applicantDetail.ApplicantName,
            };

            var details = JsonConvert.SerializeObject(dto);

            return details;
        }

        
    }
}
