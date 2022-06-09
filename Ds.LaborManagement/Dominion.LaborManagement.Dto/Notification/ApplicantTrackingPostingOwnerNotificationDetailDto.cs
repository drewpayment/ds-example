using System;

namespace Dominion.LaborManagement.Dto.Notification
{
    [Serializable]
    public class ApplicantTrackingPostingOwnerNotificationDetailDto
    {
        public int PostingId { get; set; }
        public string PostingDescription { get; set; }
        public int ApplicantId { get; set; }
        public string ApplicantName { get; set; }
    }
}
