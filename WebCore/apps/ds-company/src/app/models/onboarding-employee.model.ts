import { Moment } from 'moment';
import { IEmployee } from '@ajs/core/ds-resource/models';
import { IEmployeeAvatars } from '@ds/core/employees/shared/employee-avatars.model';

import { CompletionStatusType } from "@ds/core/shared/completion-status.enum";

export interface IOnboardingAdminTask {
    employeeOnboardingTaskId:number,
    employeeId:number,
    parentTaskId:number|null,
    isComplete:boolean,
    isEditing:boolean,
    description:string,
    completionStatus:CompletionStatusType|null,
    dateCompleted:Date|string|Moment,
    completedBy:number|null,
    assignedTo:number|null,
    dueDate:Date|string|Moment,
    sequence:number|null,
    modifiedBy: number|null,
    modified: Date|string|Moment,
    taskId: number|null,
    task: any,
}

export interface IOnboardingEmployee { 
    adminPctComplete: number;
    azureSas: string;
    city: string;
    clientDepartmentCode: string;
    clientDepartmentId: number;
    clientId: number;
    clientName: string;
    dateHireDate: Date | string | Moment;
    dateOfHire: Date | string | Moment;
    departmentName: string;
    directSupervisorID: number;
    divisionName: string;
    emailAddress: string;
    employeeClientRates: any[];
    employeeFirstName: string;
    employeeId: number;
    employeeInitial: string;
    employeeLastName: string;
    employeeMiddleName: string;
    employeeName: string;
    employeeNumber: string;
    employeePayInfo: any[];
    employeeSignoff: Date | string | Moment;
    employeeStarted: Date | string | Moment;
    employeeStatus: number;
    employeeStatusDescription: string;
    employeeWorkflow: any[];
    essActivated: any;
    firstName: string;
    fromDashboard: boolean;
    hireDate: Date | string | Moment;
    homePhone: string;
    intEmployeeNumber: number;
    invitationSent: Date | string | Moment;
    isFinalized: boolean;
    isI9AdminComplete: boolean;
    isI9Required: boolean;
    isInOnboarding: boolean;
    isOnboardingCompleted: boolean;
    isSelfOnboardingComplete: boolean;
    isWorkflowComplete: boolean;
    jobCategory: string;
    jobProfileId: number;
    jobTitle: string;
    lastName: string;
    modified: Date | string | Moment;
    modifiedBy: number;
    onboardingEnd: any;
    onboardingInitiated: Date | string | Moment;
    password: string;
    payType: number;
    pctComplete: number;
    profileImage: any;
    rehireDate: Date | string | Moment;
    selfOnboardingTitle: string;
    separationDate: Date | string | Moment;
    shortAdmin: any;
    shortEmployee: any;
    sortAdmin: string;
    sortEmployee: string;
    state: string;
    statusType: string;
    supervisor: string;
    taskList: IOnboardingAdminTask[];
    userAddedDuringOnboarding: boolean;
    userId: number;
    userName: string;
    
    // UI ADDED PROPS
    employeeAvatar: IEmployeeAvatars;
}
