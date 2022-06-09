
export interface IContactNotificationPreference {
    notificationContactPreferenceId: number;
    notificationContactId: number;
    notificationTypeId: number;
    contactLabel: string;
    contactDescription: string;
    canClientControl: boolean;
    sendEmail: boolean;
    sendSms: boolean;
    modified: Date;
    modifiedBy: number;
}