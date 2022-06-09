export interface UserClientAccess {
    userId: number;
    clientId: number;
    clientName: string;
    hasAccess: boolean;
    isClientAdmin: boolean;
    isBenefitAdmin: boolean;
}
