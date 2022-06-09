import { IAccessRule } from "./access-rule.model";
import { IClaimType } from './claim-type.model';

export interface IAccessRuleClaim extends IAccessRule {
    claimType: IClaimType;
}