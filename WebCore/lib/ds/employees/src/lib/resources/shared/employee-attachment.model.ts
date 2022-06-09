import { ResourceSourceType } from './resource-source-type';

export interface IEmployeeAttachment {
    resourceId: number,
    clientId: number,
    employeeId?: number,
    name: string,
    addedDate: Date,
    addedByUsername: string,
    folderId?: number,
    isViewableByEmployee: boolean,
    isAzure: boolean,
    sourceType: ResourceSourceType,
    extension: string,
    source: string,
    cssClass: string;

    // UI ONLY
    azureUrl: string;
}
