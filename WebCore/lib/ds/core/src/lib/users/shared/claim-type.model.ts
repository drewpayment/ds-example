import { ClaimSource } from './claim-source.enum';
import { OtherAccessClaimType } from './other-access-type.enum';

export interface IClaimType {
    claimTypeId: number | null;
    name: string;
    source: ClaimSource;
    otherAccessType?: OtherAccessClaimType;
}