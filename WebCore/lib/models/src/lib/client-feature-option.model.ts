export interface ClientFeatureOption {
    clientId: number;
    accountFeature: number;
    modifiedBy: number;
    modified: Date;
    description: string;
    isEnabled: Boolean;
    isExisting: Boolean;
    isClientEnabled: boolean;
    automaticBillingItemCount: number;
}