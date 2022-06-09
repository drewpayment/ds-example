using System.Collections.Generic;

namespace Dominion.Core.Dto.Notification
{
    public class ContactPreferencesDto
    {
        public NotificationContactDto                              ContactInfo       { get; set; }
        public IEnumerable<ContactEmailAddressDto>                 EmailAddresses    { get; set; }
        public IEnumerable<ContactPhoneNumberDto>                  PhoneNumbers      { get; set; }
        public IEnumerable<NotificationContactPreferenceDto>       Preferences { get; set; }
    }
}
