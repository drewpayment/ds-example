import { Moment } from 'moment';

export interface TimeOffRequest {
    timeOffRequestId?: number;
    clientAccrualId: number;
    employeeId: number;
    requestFrom: Date | Moment | string;
    requestUntil: Date | Moment | string;
    modifiedBy?: number;
    amountInOneDay: number;
    requesterNotes: string;
    balanceBeforeApp?: number;
    balanceAfterApp?: number;
    originalRequestDate?: Date | Moment | string;
    clientId?: number;
    status?: TimeOffStatusType;
}

export interface TimeOffRequestResult {
    requestTimeOffId: number;    
}

export enum TimeOffStatusType {
    NoValueSelected,
    Pending,
    Approved,
    Rejected,
    Cancelled,
}
