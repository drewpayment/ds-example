import { AccountFeature } from "./account-feature.enum";
import { IClaimType } from './claim-type.model';

export interface IAccountFeatureClaimType extends IClaimType {
    accountFeature: AccountFeature;
}