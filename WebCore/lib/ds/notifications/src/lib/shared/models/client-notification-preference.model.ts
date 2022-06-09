export interface IClientNotificationPreference {
    clientNotificationPreferenceId: number;
    clientId: number;
    notificationTypeId: number;
    clientLabel: string;
    clientDescription: string;
    canClientControl: boolean;
    isEnabled: boolean;
    userMustAcknowledge: boolean;
    allowCustomFrequency: boolean;
    modified: Date;
    modifiedBy: number;
}