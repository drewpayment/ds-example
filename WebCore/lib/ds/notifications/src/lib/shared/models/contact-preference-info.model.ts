import { INotificationContact } from "./notification-contact.model";
import { IContactEmailAddress } from "./contact-email-address.model";
import { IContactPhoneNumber } from "./contact-phone-number.model";
import { IContactNotificationPreference } from "./contact-notification-preference.model";


export interface IContactPreferenceInfo{
    contactInfo: INotificationContact;
    emailAddresses: IContactEmailAddress[];
    phoneNumbers: IContactPhoneNumber[];
    preferences: IContactNotificationPreference[];
}