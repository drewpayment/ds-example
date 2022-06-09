import { IContactNotificationPreference } from "./contact-notification-preference.model";
import { ICategorizedClientNotificationPreferences } from "./categorized-client-notification-preferences.model";


export interface INotificationPreferencesProductGroups {
    productId: number;
    name: string;
    iconName: string;
    isEnabled: boolean;
    $accordionToggle?: boolean;
    contactNotificationPreferences?: IContactNotificationPreference[];
    clientNotificationPreferences?: ICategorizedClientNotificationPreferences;
}