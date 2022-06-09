export interface ITaxFrequency {
    taxFrequencyId: TaxFrequencyTypeEnum,
    frequency: string
}

export enum TaxFrequencyTypeEnum {
    EveryPayroll = 1,
    Monthly = 2,
    Quarterly = 3
}
