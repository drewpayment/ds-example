import { IClientNotificationPreference } from "./client-notification-preference.model";

export interface ICategorizedClientNotificationPreferences {
    requiredPreferences: IClientNotificationPreference[];
    optionalPreferences: IClientNotificationPreference[];
}