export interface BillingItemDescription {
    billingItemDescriptionId: number;
    code: string;
    description: string;
    costCenter: string;
    flatIncrease: number;
    perQtyIncrease: number;
    allowFlatIncreaseFromZero: Boolean;
    allowPerQtyIncreaseFromZero: Boolean;
    isActive: Boolean;
}