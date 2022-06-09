import { Moment } from 'moment';


export interface RealTimePunchRequest {
    clientId: number;
    employeeId: number;
    costCenterId?: number;
    departmentId?: number;
    divisionId?: number;
    isOutPunch: boolean;
    punchTypeId?: number;
    overridePunchTime?: Moment | Date | string;
    overrideLunchBreak?: number;
    inputHours?: number;
    inputHoursDate?: Moment | Date | string;
    comment?: string;
    employeeComment?: string;
    clockName?: string;
    clockHardwareId?: number;
    jobCostingAssignmentId1?: number;
    jobCostingAssignmentId2?: number;
    jobCostingAssignmentId3?: number;
    jobCostingAssignmentId4?: number;
    jobCostingAssignmentId5?: number;
    jobCostingAssignmentId6?: number;
}

export interface RealTimePunchPairRequest {
    first: RealTimePunchRequest;
    second: RealTimePunchRequest;
}


export interface RealTimePunchResult {
    succeeded: boolean;
    message: string;
    punchTime: Moment | Date | string;
    punchId: number | null;
    transerPunchId: number | null;
    isDuplicatePunch: boolean;
}

export interface RealTimePunchPairResult {
    succeeded: boolean;
    message: string;
    first: RealTimePunchResult;
    second: RealTimePunchResult;
}
