import { Moment } from 'moment';

export interface EmployeeExitInterviewRequest {
    employeeId: number;
    requestedBy?: number;
    requestedOn?: Moment | Date | string;
    sentOn?: Moment | Date | string;
}
