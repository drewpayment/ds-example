using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Dominion.Core.Dto.Notification;
using Dominion.Core.Services.Api.Notification;
using Dominion.Core.Services.Interfaces;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.Notification;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.OpResult;
using Dominion.Core.Dto.Client;
using Newtonsoft.Json;
using EmailDto = Dominion.Core.Dto.Notification.EmailDto;
using System.Configuration;

namespace Dominion.LaborManagement.Service.Internal.Notification
{
    public class ApplicantTrackingCorrespondenceNotificationBuilder : INotificationBuilder
    {
        private readonly IBusinessApiSession                               _session;
        private readonly ApplicantDetailDto                                _applicant;
        private readonly int?                                              _correspondenceId;
        private readonly string                                            _correspondenceBody;
        private readonly string                                            _correspondenceSubject;
        private readonly ApplicantTrackingCorrespondenceReplacementInfoDto _replacementInfo;
        private readonly IClientService                                    _clientService;

        private          ClientSMTPSettingDto                              _clientSMTP;
        private readonly bool?                                             _isText;


        NotificationType INotificationBuilder.NotificationType =>
            NotificationType.ApplicantTrackingCorrespondence;

        string INotificationBuilder.NotificationDetails => GetNotificationDetails();

        public ApplicantTrackingCorrespondenceNotificationBuilder(IBusinessApiSession session, ApplicantDetailDto applicant, int? correspondenceId,
            string correspondenceSubject, string correspondenceBody, ApplicantTrackingCorrespondenceReplacementInfoDto replacementInfo, IClientService clientService, bool? isText)
        {
            _session            = session;
            _applicant          = applicant;
            _correspondenceId   = correspondenceId;
            _correspondenceSubject = string.IsNullOrEmpty(correspondenceSubject) ? "Application Activity" : correspondenceSubject;
            _correspondenceBody = correspondenceBody;
            _replacementInfo    = replacementInfo;
            _clientService      = clientService;
            _isText             = isText;
        }

        IOpResult<IEnumerable<INotificationRecipient>> INotificationBuilder.BuildNotifications()
        {
            // Fetch client smtp settings if present
            var setting = _clientService.GetClientSMTPSetting(_replacementInfo.ApplicantClientId);
            if (setting.Success && setting.HasData)
                _clientSMTP = setting.Data;

            var result = new OpResult<IEnumerable<INotificationRecipient>>();

            result.TryCatch(() =>
            {
                var applicantInfo = _session.UnitOfWork.ApplicantTrackingRepository
                    .ApplicantsQuery()
                    .ByApplicantId(_applicant.ApplicantId)
                    .ExecuteQueryAs(a => new
                    {
                        a.UserId,
                        a.ApplicantId
                    })
                    .ToOrNewList();

                if (applicantInfo.Any())
                {
                    var recipients = applicantInfo
                        .GroupBy(r => new { r.UserId, r.ApplicantId })
                        .Select(group => new NotificationRecipient
                        {
                            UserId = group.Key.UserId,
                            ApplicantId = group.Key.ApplicantId,
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
                Subject = _correspondenceSubject,
                Body = BuildBody(),
                IsBodyHtml = true,
                ClientSMTPSetting = _clientSMTP
            };

            return email;
        }

        private SmsDto GetSms()
        {
            var sms = new SmsDto
            {
                Message = BuildSmsMessage()
            };

            return sms;
        }
       

        private string GetNotificationDetails()
        {
            var dto = new ApplicantTrackingCorrespondenceNotificationDetailDto
            {
                ApplicantId         = _applicant.ApplicantId,
                ApplicantName       = _applicant.ApplicantName,
                ApplicationHeaderId = _applicant.ApplicationHeaderId,
                CorrespondenceId    = _correspondenceId,
                PostingId           = _applicant.PostingId,
                Posting             = _applicant.Posting,
                Subject             = _correspondenceSubject,
                IsText              = _isText,
            };

            var details = JsonConvert.SerializeObject(dto);

            return details;
        }

        private string BuildBody()
        {
            if (_isText == true) return "";

            var mailBody = new StringBuilder(_correspondenceBody);
            var mainLink = ConfigurationManager.AppSettings.Get("MainRedirectRootUrl");

            mailBody.Replace("{*Applicant}", _replacementInfo.ApplicantFirstName + " " + _replacementInfo.ApplicantLastName);
            mailBody.Replace("{*ApplicantFirstName}", _replacementInfo.ApplicantFirstName);
            mailBody.Replace("{*UserName}", _replacementInfo.UserName);
            mailBody.Replace("{*Password}", string.IsNullOrEmpty(_replacementInfo.Password) ? "********" : _replacementInfo.Password);
            mailBody.Replace("{*OnboardingUrl}", "<a href='" + mainLink + "'>ONBOARDING</a>");
            mailBody.Replace("{*Posting}", _replacementInfo.Posting);
            mailBody.Replace("{*Date}", _replacementInfo.Date.ToShortDateString());
            mailBody.Replace("{*ApplicantAddress}", _replacementInfo.Address);
            mailBody.Replace("{*ApplicantPhoneNumber}", _replacementInfo.Phone);
            mailBody.Replace("{*CompanyAddress}", _replacementInfo.CompanyAddress);
            if (!string.IsNullOrEmpty(_replacementInfo.CompanyLogo))
                mailBody.Replace("{*CompanyLogo}", "<img src='" + _replacementInfo.CompanyLogo + "'/>");
            else
                mailBody.Replace("{*CompanyLogo}", "");

            mailBody.Replace("{*CompanyName}", _replacementInfo.CompanyName );

            return mailBody.ToString().Trim();
        }

        private string BuildSmsMessage()
        {
            if (_isText == false) return "";

            var message = new StringBuilder();

            if (_isText == true)
            {
                var mainLink = ConfigurationManager.AppSettings.Get("MainRedirectRootUrl");

                message.Append(_correspondenceBody);
                message.Replace("{*Applicant}", _replacementInfo.ApplicantFirstName + " " + _replacementInfo.ApplicantLastName);
                message.Replace("{*ApplicantFirstName}", _replacementInfo.ApplicantFirstName);
                message.Replace("{*UserName}", _replacementInfo.UserName);
                message.Replace("{*Password}", string.IsNullOrEmpty(_replacementInfo.Password) ? "********" : _replacementInfo.Password);
                message.Replace("{*OnboardingUrl}", "<a href='" + mainLink + "'>ONBOARDING</a>");
                message.Replace("{*Posting}", _replacementInfo.Posting);
                string companyAddress = Regex.Replace(_replacementInfo.CompanyAddress, @"<br\s{0,1}\/{0,1}>", @"\n");
                message.Replace("{*CompanyAddress}", companyAddress);
                message.Replace("{*CompanyName}", _replacementInfo.CompanyName);
            }
            else
            {
                message.Append("There is new activity regarding your application for ");
                message.Append(_applicant.Posting);
                message.Append(". Please check your email for more details.");
            }

            return message.ToString().Trim();
        }
    }
}
