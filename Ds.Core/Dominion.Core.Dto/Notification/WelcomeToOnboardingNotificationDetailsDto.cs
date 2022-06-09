using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Onboarding;

namespace Dominion.Core.Dto.Notification
{
    public class WelcomeToOnboardingNotificationDetailsDto
    {
        public int? ClientId { get; set; }
        public int EmployeeId { get; set; }
        public int AdminID { get; set; }
        public EmployeeOnboardingEmailDto dto { get; set; }
    }
}
