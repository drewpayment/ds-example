import { UserType } from "../../shared";
import { IClaimType } from './claim-type.model';

export interface IUserTypeClaimType extends IClaimType {
    userType: UserType;
}