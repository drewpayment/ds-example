export interface Pto {
    ptoInfo: PTOInfo[];
}

export interface PTOInfo {
    employeeName: string,
    originalRequestDate: Date,
    pendingUnits: number,
    policyName: string,
    startingUnits: number,
    unitsAvailable: number
}