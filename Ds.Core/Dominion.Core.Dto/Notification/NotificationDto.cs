using System;

namespace Dominion.Core.Dto.Notification
{
    [Serializable]
    public partial class NotificationDto
    {
        public int      NotificationId     { get; set; }
        public int      NotificationTypeId { get; set; }
        public string   Details            { get; set; }
        public DateTime DateGenerated      { get; set; }

    }
}
