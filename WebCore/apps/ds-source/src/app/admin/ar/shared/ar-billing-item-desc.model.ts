export interface IArBillingItemDesc{
    billingItemDescriptionId: number;
    code: string;
    description: string;
    costCenter: string;
    flatIncrease?: number;
    perQtyIncrease?: number;
    allowFlatIncreaseFromZero: number;
    allowPerQtyIncreaseFromZero: number;
    isActive: boolean;
}