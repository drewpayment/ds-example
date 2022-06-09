export interface GeneralLedgerType {
    generalLedgerTypeId: number,
    description: string,
    generalLedgerGroupId: number | null,
    taxTypeId: number | null,
    canBeAccrued: boolean,
    canBeOffset: boolean,
    canBeDetail: boolean
}