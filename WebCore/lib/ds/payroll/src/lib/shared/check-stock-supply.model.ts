import { ICheckStockBilling } from '.';

export interface ICheckStockSupply {
    name            : String,
    id              : number,
    include         : Boolean,
    hasCheckNumber  : Boolean,
    checkNumber     : number | null,
    billingInfo     : ICheckStockBilling[] | null,
    selectedBilling : ICheckStockBilling | null
}