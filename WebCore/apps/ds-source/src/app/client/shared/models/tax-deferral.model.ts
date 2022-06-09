import { Moment } from 'moment';

export interface TaxDeferral {
    clientTaxDeferralId: number;
    clientId: number;
    taxType: TaxDeferralType;
    isDeferred: boolean;
    endDate?: Date | string | Moment;
    modified?: Date | string | Moment;
    modifiedBy?: number;
}

export enum TaxDeferralType {
    EmployerSocialSecurity
}
