using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Notification;
using Dominion.Core.Services.Api.Notification;
using Dominion.Core.Services.Interfaces;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.Notification;
using Dominion.Utility.Constants;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.OpResult;
using Dominion.Core.Dto.Client;
using Newtonsoft.Json;
using EmailDto = Dominion.Core.Dto.Notification.EmailDto;

namespace Dominion.LaborManagement.Service.Internal.Notification
{
    public class ApplicantTrackingPostingOwnerNotificationBuilder : INotificationBuilder
    {
        private readonly IBusinessApiSession _session;
        private readonly ApplicantPostingDetailDto _postDetail;
        private readonly ApplicantDetailDto _applicantDetail;
        private readonly Uri _uri;
        private readonly IClientService _clientService;

        private ClientSMTPSettingDto _clientSMTP;

        NotificationType INotificationBuilder.NotificationType => NotificationType.ApplicantTrackingPostingOwner;

        string INotificationBuilder.NotificationDetails => GetNotificationDetails();
        

        public ApplicantTrackingPostingOwnerNotificationBuilder(IBusinessApiSession session, ApplicantPostingDetailDto postDetail,
            ApplicantDetailDto applicantDetail, Uri qualifyApplicantsLink, IClientService clientService)
        {
            _session = session;
            _postDetail = postDetail;
            _applicantDetail = applicantDetail;
            _uri = qualifyApplicantsLink;
            _clientService = clientService;
        }

        IOpResult<IEnumerable<INotificationRecipient>> INotificationBuilder.BuildNotifications()
        {
            // Fetch client smtp settings if present
            var setting = _clientService.GetClientSMTPSetting(_postDetail.ClientId);
            if (setting.Success && setting.HasData)
                _clientSMTP = setting.Data;

            var result = new OpResult<IEnumerable<INotificationRecipient>>();

            result.TryCatch(() =>
            {
                var postingOwnerInfo = _session.UnitOfWork.UserRepository
                    .QueryUsers()
                    .ByUserIds(_postDetail.PostingOwners.Select(x=> x.UserId).ToArray())
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
                                EmailBuilder = () => GetEmail(group.Select(u=>u.FullName).FirstOrDefault()),
                                SmsBuilder = GetSms
                            })
                        })
                        .ToOrNewList();

                    result.Data = recipients;
                }
            });

            return result;

        }

        private EmailDto GetEmail(string recipientName)
        {
            var email = new EmailDto
            {
                Subject = "New Applicant for " + _postDetail.Description + ".",                
                Body = BuildBody(recipientName),
                IsBodyHtml = true,
                ClientSMTPSetting = _clientSMTP
            };

            return email;
        }

        private string BuildBody(string recipientName)
        {
            var body = new StringBuilder();

            body.Append("Attention ");
            body.Append(recipientName);
            body.Append(",");
            body.Append(NotificationConstants.LINE_BREAK_TAG);
            body.Append(NotificationConstants.LINE_BREAK_TAG);
            body.Append("You have received a new application for ");
            body.Append(_postDetail.Description.Italicize());
            body.Append(" from ");
            body.Append(_applicantDetail.ApplicantName);
            body.Append(". To review, please proceed to ");
            body.Append("Applicant Tracking - Qualify Applicants".Italicize());
            body.Append(" or select the link below.");
            body.Append(NotificationConstants.LINE_BREAK_TAG);
            body.Append(NotificationConstants.LINE_BREAK_TAG);
            body.AppendLine(GetQualifyApplicantsLink());

            return body.ToString().Trim();
        }

        private string GetQualifyApplicantsLink()
        {
            var link = new StringBuilder();

            link.Append("<a href='");
            link.Append(_uri);
            link.Append("/QualifyApplicants.aspx' target='_blank'>Qualify Applicant</a>");

            return link.ToString().Trim();
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

            message.Append("You have received a new application for ");
            message.Append(_postDetail.Description);
            message.Append(" from ");
            message.Append(_applicantDetail.ApplicantName);
            message.Append(".");
            message.Append("To review, please access the Applicant Tracking - Qualify Applicants page.");

            return message.ToString().Trim();
        }

        private string GetNotificationDetails()
        {
            var dto = new ApplicantTrackingPostingOwnerNotificationDetailDto
            {
                PostingId = _postDetail.PostingId,
                PostingDescription = _postDetail.Description,
                ApplicantId = _applicantDetail.ApplicantId,
                ApplicantName = _applicantDetail.ApplicantName
            };

            var details = JsonConvert.SerializeObject(dto);

            return details;
        }

        
    }
}
