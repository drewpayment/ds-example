import { Moment } from 'moment';

export interface BetaFeature {
    betaFeatureId: number;
    name: string;
    description: string;
    startDate?: Date | Moment | string;
    endDate?: Date | Moment | string;
    modified?: Date | Moment | string;
    modifiedBy?: number;
    deletedAt?: Date | Moment | string;
}

export interface UserBetaFeature {
    userBetaFeatureId: number;
    userId: number;
    betaFeatureId: number;
    isBetaActive: boolean;
    modified?: Date | Moment | string;
    modifiedBy?: number;    
}

export enum BetaFeatureType {
    MenuWrapper = 1
}
