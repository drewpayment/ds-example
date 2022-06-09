export interface IAddressInfo {
    addressId: number;
    addressLine1: string;
    addressLine2: string;
    city: string;
    stateId: number;
    countryId: number;
    countyId?: number;
    zipCode: string;
}