export interface ClientShift {
        additionalAmount: number,
        additionalAmountTypeId: number,
        additionalPremiumAmount: number,
        clientId: number,
        clientShiftId: number,
        description: string,
        destination: number,
        isFriday: boolean,
        isMonday: boolean,
        isSaturday: boolean,
        isSunday: boolean,
        isThursday: boolean,
        isTuesday: boolean,
        isWednesday: boolean,
        limit: number,
        modified: string,
        modifiedBy: string,
        shiftEndTolerance: string,
        shiftStartTolerance: string,
        startTime: string,
        stopTime: string
}