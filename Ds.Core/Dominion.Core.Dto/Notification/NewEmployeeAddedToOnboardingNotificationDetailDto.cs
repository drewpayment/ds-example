using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Employee;

namespace Dominion.Core.Dto.Notification
{
    public class NewEmployeeAddedToOnboardingNotificationDetailDto
    {
        public string                      ClientCode       { get; set; }
        public int                         NewEmployeeCount { get; set; }
        public IEnumerable<AddEmployeeDto> NewEmployees     { get; set; }
    }
}
