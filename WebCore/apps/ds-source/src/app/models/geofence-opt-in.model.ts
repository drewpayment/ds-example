import { IClientNotes } from './client-note.model';
import { Features } from '@ds/admin/client-statistics/shared/models/featureEnum';

export interface IGeofenceOptIn {
    firstName: string;
    lastName: string;
    userPin: string;
    clientNotes: IClientNotes;
    geofenceOptionID: GeofenceOptionType;
    optIn: boolean;
}

export enum GeofenceOptionType {
    ESSENTIAL = 0,
    PRO = 1,
    ENTERPRISE = 2,
}

export interface IClientAgreement {
    clientFeaturesAgreementID: number;
    clientID: number;
    firstName: string;
    lastName: string;
    userID: number;
    agreed: boolean;
    accountFeatureEnum: Features;
}
