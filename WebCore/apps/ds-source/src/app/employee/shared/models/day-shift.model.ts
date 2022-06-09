export interface DayShift {
    dayShiftIndex: number,
    shiftStart: Date | null,
    shiftEnd: Date | null,
    clientDepartmentId: number | null,
    clientDepartmentName: string,
    clientCostCenterId: number | null,
    clientCostCenterName: string,
    hasClientDepartment: Boolean,
    hasClientCostCenter: Boolean,
    isValid: Boolean,
    shiftTimeDIsplayText: string
}