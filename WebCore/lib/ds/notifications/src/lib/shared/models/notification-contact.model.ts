export interface INotificationContact {
    notificationContactId: number;
    userId?: number;
    employeeId?: number;
    email: string;
    phoneNumber: string;
    modified: Date;
    modifiedBy: number;
}