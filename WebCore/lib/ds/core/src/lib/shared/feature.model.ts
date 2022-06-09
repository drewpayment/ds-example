import { Features } from '@ds/admin/client-statistics/shared/models/featureEnum';

export interface IClientFeature {
    clientId: number;
    accountFeature: Features;
    modified?: Date | string;
    modifiedBy?: number;    
    isEnabled: boolean;
    isExisting: boolean;
    isClientEnabled: boolean;
    description: string;
    featureOptionsGroupId: number;
    automaticBillingItemCount: number;
}