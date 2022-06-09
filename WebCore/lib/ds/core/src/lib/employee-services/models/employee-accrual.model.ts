export interface EmployeeAccrual {
    employeeAccrualId: number;
    employeeId: number;
    clientAccrualId: number;
    modified: Date;
    modifiedBy: number;
    isAllowScheduledAwards: boolean;
    waitingPeriodDate: Date;
    isActive: boolean
}