using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Notification
{
    public class AnniversaryListNotificationDetailDto
    {
        public int ClientId { get; set; }
        public DateTime DateRangeStart { get; set; }
        public DateTime DateRangeEnd { get; set; }
        public IEnumerable<EmployeeAnniversaryDto> AnniversaryList { get; set; }
    }
}
