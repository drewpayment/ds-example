using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Notification
{
    public class BirthdayListNotificationDetailDto
    {
        public int                              ClientId       { get; set; }
        public DateTime                         DateRangeStart { get; set; }
        public DateTime                         DateRangeEnd   { get; set; }
        public IEnumerable<EmployeeBirthdayDto> BirthdayList   { get; set; }
    }
}
