import { Moment } from 'moment';

export enum LocalityType {
    federal = 1,
    state = 2,
    local = 3
}

export interface IOnboardingSummaryData {
    companyName?: string;
    submittedDate?: string | Date | Moment;
    submittedByUser?: string;
    employeeName?: string;
    employeeNumber?: string;
    employeeInitial?: string;
    department?: string;
    division?: string;
    supervisor?: string;
    stateW4?: string;
    date?: Date;
    taxWorkflowItems?: IWorkflowItems[];
    workflowItems?: IWorkflowItems[];
}

export interface IWorkflowItems {
    id: number;
    title?: string;
    description?: string;
    workflowTaskId?: number;
    resources?: IWorkflowResources[];
}

export interface IAttachments {
    sourceTypeId: number;
    name?: string;
    source?: string;
}

export interface IWorkflowResources {
    resourceId: number;
    name?: string;
}
