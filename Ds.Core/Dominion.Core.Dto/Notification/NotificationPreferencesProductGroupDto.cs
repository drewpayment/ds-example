using System.Collections.Generic;
using Dominion.Core.Dto.Core;

namespace Dominion.Core.Dto.Notification
{
    public class NotificationPreferencesProductGroupDto
    {
        public Product                                       ProductId                      { get; set; }
        public string                                        Name                           { get; set; }
        public string                                        IconName                       { get; set; }
        public bool                                          IsEnabled                      { get; set; }
        public IEnumerable<NotificationContactPreferenceDto> ContactNotificationPreferences { get; set; }
        public CategorizedClientNotificationPreferencesDto   ClientNotificationPreferences  { get; set; }
        
    }
}
