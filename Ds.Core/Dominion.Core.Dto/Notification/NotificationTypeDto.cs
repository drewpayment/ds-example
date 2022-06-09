using System.Collections.Generic;
using Dominion.Core.Dto.Core;

namespace Dominion.Core.Dto.Notification
{
    public class NotificationTypeDto : IHasNotificationType
    {
        public NotificationType NotificationTypeId       { get; set; }
        public Product          ProductId                { get; set; }
        public string           ClientLabel              { get; set; }
        public string           ClientDescription        { get; set; }
        public string           ContactLabel             { get; set; }
        public string           ContactDescription       { get; set; }
        public bool             CanClientControl         { get; set; }
        public bool             IsEnabledDefault_Client  { get; set; }
        public bool             IsEnabledDefault_User    { get; set; }

        public ProductDto Product { get; set; }

        public IEnumerable<NotificationTypeUserTypeDto> NotificationTypeUserTypes { get; set; }
    }
}
