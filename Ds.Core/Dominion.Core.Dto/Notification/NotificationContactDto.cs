using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Notification
{
    public class NotificationContactDto
    {
        public int       NotificationContactId { get; set; }
        public int?      UserId                { get; set; } 
        public int?      EmployeeId            { get; set; }
        public string    Email                 { get; set; }
        public string    PhoneNumber           { get; set; }
        public DateTime  Modified              { get; set; }
        public int       ModifiedBy            { get; set; }
        
        public IEnumerable<NotificationContactPreferenceDto> NotificationContactPreferences { get; set; }
    }
}
