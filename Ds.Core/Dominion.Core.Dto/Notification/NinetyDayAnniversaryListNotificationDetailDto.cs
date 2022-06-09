using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Notification
{
    public class NinetyDayAnniversaryListNotificationDetailDto
    {
        public int ClientId { get; set; }
        public DateTime MonthStartDate { get; set; }
        public IEnumerable<EmployeeNinetyDayAnniversaryDto> NinetyDayAnniversaryList { get; set; }
    }
}
