using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Notification
{
    [Serializable]
    public class ApplicantTrackingCorrespondenceNotificationDetailDto
    {
        public int    ApplicantId         { get; set; }
        public string ApplicantName       { get; set; }
        public int    ApplicationHeaderId { get; set; }
        public int?    CorrespondenceId    { get; set; }
        public int    PostingId           { get; set; }
        public string Posting             { get; set; }
        public string Subject { get; set; }
        public bool? IsText { get; set; }
    }
}
