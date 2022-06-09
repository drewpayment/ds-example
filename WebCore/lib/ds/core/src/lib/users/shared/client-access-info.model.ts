import { IClaimType } from './claim-type.model';

export interface IClientAccessInfo {
    clientId: number;
    name: string;
    code: string;
    claims: IClaimType[];
}