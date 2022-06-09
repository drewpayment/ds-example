import { Moment } from 'moment';


export interface EmployeeNavigatorSyncLog {
    employeeNavigatorSyncLogId: number;
    employeeId: number;
    clientId: number;
    syncType: EnSyncType;
    syncDirection: EnSyncDirectionType;
    syncStatus: EnSyncStatusType;
    hasRetriedSync: boolean;
    createdAt: Date|Moment|string;
    modifiedBy: number;
    modified: Date|Moment|string;
}

export enum EnSyncType {
    deduction,
    employeeDemographic,
}

export enum EnSyncDirectionType {
    fromDsToEn,
    fromEnToDs,
}

export enum EnSyncStatusType {
    pending,
    success,
    failure,
}
