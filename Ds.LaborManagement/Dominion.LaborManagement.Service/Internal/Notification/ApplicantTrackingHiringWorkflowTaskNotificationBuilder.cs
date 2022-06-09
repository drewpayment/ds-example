using System.Collections.Generic;
using System.Text;
using Dominion.Core.Dto.Notification;
using Dominion.Core.Services.Api.Notification;
using Dominion.Core.Services.Interfaces;
using Dominion.LaborManagement.Dto.Notification;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.OpResult;
using Newtonsoft.Json;

namespace Dominion.LaborManagement.Service.Internal.Notification
{
    public class ApplicantTrackingHiringWorkflowTaskNotificationBuilder : INotificationBuilder
    {
        private readonly IBusinessApiSession _session;

        NotificationType INotificationBuilder.NotificationType => NotificationType.ApplicantTrackingHiringWorkflowTask;

        string INotificationBuilder.NotificationDetails => GetNotificationDetails();
        

        public ApplicantTrackingHiringWorkflowTaskNotificationBuilder(IBusinessApiSession session)
        {
            _session = session;
        }

        IOpResult<IEnumerable<INotificationRecipient>> INotificationBuilder.BuildNotifications()
        {
            var result = new OpResult<IEnumerable<INotificationRecipient>>();

            result.TryCatch(() =>
            {
                var recipients = new List<INotificationRecipient>().ToOrNewList();

                // Logic to determine recipients of the notification here (build a list of NotificationRecipient objects)

                result.Data = recipients.ToOrNewList();
            });

            return result;

        }

        private EmailDto GetEmail()
        {
            var email = new EmailDto
            {
                Subject = "",
                Body = BuildBody(),
                IsBodyHtml = true
            };

            return email;
        }

        private SmsDto GetSms()
        {
            var sms = new SmsDto
            {
                Message = ""
            };

            return sms;
        }

       private string GetNotificationDetails()
        {
            // TODO: add the necessary properties to ApplicantTrackingHiringWorkflowNotificationDetailDto. 
            // These properties should be the pieces of info we want to save related to this type of notification (i.e. ClientId, EmployeeId, etc.)

            var dto = new ApplicantTrackingHiringWorkflowNotificationDetailDto
            {
                // Populate the ApplicantTrackingHiringWorkflowNotificationDetailDto
            };

            var details = JsonConvert.SerializeObject(dto);

            return details;
        }

        private string BuildBody()
        {
            var sb = new StringBuilder();

            // Logic to build the body of the email here.

            return sb.ToString().Trim();
        }
    }
}
