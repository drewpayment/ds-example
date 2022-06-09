using System.Collections.Generic;

namespace Dominion.Core.Dto.Notification
{
    public class ICIMSImportNotificationInfoDto
    {
        public int              ClientId       { get; set; }
        public string           ClientCode     { get; set; }
        public IEnumerable<int> EmployeeIdList { get; set; }
    }
}
