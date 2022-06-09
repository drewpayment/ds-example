using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Notification
{
    public class CategorizedClientNotificationPreferencesDto
    {
        public IEnumerable<ClientNotificationPreferenceDto> RequiredPreferences { get; set; }
        public IEnumerable<ClientNotificationPreferenceDto> OptionalPreferences { get; set; }
    }
}
