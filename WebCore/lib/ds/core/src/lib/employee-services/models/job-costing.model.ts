import { Client } from '.';
import { Moment } from 'moment';
import { Observable } from 'rxjs';

export interface JobCosting {
    clientJobCostingId: number;
    clientId: number;
    jobCostingTypeId: number;
    jobCostingTypeDescription: string;
    hideOnScreen: boolean;
    isRequired: boolean;
    sequence: number;
    level: number;
    parentJobCostingIds?: number[];
    parentJobCostingAssignmentIds?: number[];
    description: string;

    // UI ONLY
    availableAssignments?: JobCostingAssignment[];
    availableAssignments$?: Observable<JobCostingAssignment[]>;
    selectedAvailableAssignmentId?: number;
    jobCostingAssignmentSelection?: JobCostingAssignment;
    parentJobCosting?: JobCosting;
    formName: string;
}

export interface JobCostingAssignment {
    clientJobCostingAssignmentId: number;
    description: string;
    code: string;
    clientId: number;
    clientJobCostingId: number;
    foreignKeyId: number | null;
    isEnabled: boolean;
}

export interface JobCostingWithAssociations {
    clientJobCostingId: number;
    parentJobCostingIds?: number[];
    parentJobCostingAssignmentIds?: number[];
}

export interface JobCostingAssignmentSelected {
    clientJobCostingAssignmentSelectedId: number;
    clientJobCostingAssignmentId: number;
    clientJobCostingAssignmentId_Selected?: number;
    clientId: number;
    clientJobCostingId_Selected?: number;
    foreignKeyId_Selected?: number;
    isEnabled: boolean;
    clientJobCostingAssignment?: JobCostingAssignment;
    clientJobCostingAssignment_Selected?: JobCostingAssignment;
    clientJobCosting_Selected?: JobCosting;
    client?: Client;
}

export interface ClientCostCenter {
    clientCostCenterId: number;
    clientId: number;
    code: string;
    description: string;
    isDefaultGlCostCenter?: boolean;
    modified?: Moment | Date | string;
    isActive: boolean;
}

