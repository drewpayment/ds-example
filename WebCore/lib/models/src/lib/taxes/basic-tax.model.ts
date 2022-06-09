export interface IBasicTax {
    taxId: number;
    taxType: number;
    taxCategory: number;
    stateId?: number;
    name: string;
    legacyTaxId?: number;
}