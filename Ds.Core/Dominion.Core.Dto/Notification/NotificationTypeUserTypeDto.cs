using Dominion.Core.Dto.User;

namespace Dominion.Core.Dto.Notification
{
    public class NotificationTypeUserTypeDto
    {
        public NotificationType NotificationTypeId { get; set; }
        public UserType         UserTypeId         { get; set; }
    }
}
