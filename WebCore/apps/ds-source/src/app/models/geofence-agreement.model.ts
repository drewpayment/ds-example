import { PayFrequencyTypeEnum } from '@ajs/employee/hiring/shared/models';

export interface IGeofenceAgreement {
    firstName: string;
    lastName: string;
    pin: number;
    userId: number;
    clientId: number;
}

export interface PayFrequencyCount {
    key: PayFrequencyTypeEnum;
    total: number;
    multiplier: number;
}
