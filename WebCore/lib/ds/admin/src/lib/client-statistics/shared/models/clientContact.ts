export interface ClientContact{
    clientContactId: number;
    clientId: number;
    firstName: string;
    lastName: string;
    title: string;
    emailAddress: string;
    phoneNumber: string;
    phoneExtension: string;
    mobilePhoneNumber: string;
    fax: string;
    isPrimary: boolean;
    isDelivery: boolean;
    userPinId: number;
}
