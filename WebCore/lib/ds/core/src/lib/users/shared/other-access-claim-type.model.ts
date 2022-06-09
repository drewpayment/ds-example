import { OtherAccessClaimType } from "./other-access-type.enum";
import { IClaimType } from './claim-type.model';

export interface IOtherAccessClaimType extends IClaimType {
    otherAccessType: OtherAccessClaimType;
}