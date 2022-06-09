import { Moment } from 'moment';

export interface EmployeeLeaveManagementInfo {
    requestTimeOffId: number;
    employeeName: string;
    clientAccrualId: number;
    clientEarningId: number;
    employeeId: number;
    planDescription: string;
    requestHoursPrior?: number;
    requestHoursAfter?: number;
    hoursInDay?: number;
    units: number;
    requestFrom?: Date | Moment | string;
    until?: Date | Moment | string;
    dateRequested?: Date | Moment | string;
    status: number;
    statusDescription: string;
    hours: number;
    payrollId?: number;
    periodEnded: Date | Moment | string;
    clockEmployeeBenefitId?: number;
    leaveManagementPendingAwardId: number;
    pendingAwardType: number;
    AwardOrder: number;
}