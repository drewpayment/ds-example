import { ResourceType } from '@ds/core/resources/shared/resource-type.model';

export interface IEmployeeAttachment {
    resourceId:           number;
    clientId:             number;
    employeeId:           number;
    name:                 string;
    addedDate:            Date;
    addedByUsername:      string;
    folderId:             number;
    isViewableByEmployee: boolean;
    isAzure:              boolean;
    isATFile:             boolean;
    extension:            string;
    source:               string;
    sourceType:           ResourceType;
    onboardingWorkflowTaskId: number;
    isCompanyAttachment: boolean;    
}