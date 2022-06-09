import { IClaimType } from './claim-type.model';

export interface IUserActionTypeClaimType extends IClaimType {
    userActionTypeId: number;
    designation: string;
    description: string;
}