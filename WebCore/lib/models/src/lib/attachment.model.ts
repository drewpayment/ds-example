import { Moment } from 'moment';

export interface IEmployeeAttachmentFolder
{
    employeeFolderId : number;
    clientId : number;
    description: string;
    isNew : boolean;
    attachmentCount: number;
    attachments: Array<IEmployeeAttachment>;
    newResourceId: number;
    hovered: boolean;
    defaultATFolder: boolean;
    employeeId: number;
    isAdminViewOnly: boolean;
    isCompanyFolder: boolean;
    isDefaultOnboardingFolder: boolean;
    isDefaultPerformanceFolder: boolean;
    isEmployeeView: boolean;
    isSystemFolder: boolean;
    showAttachments: boolean;
}

export interface IEmployeeAttachment
{
    clientId? : number;
    employeeId: number;
    folderId: number;
    resourceId : number;
    name: string;
    extension: string;
    sourceType: AttachmentSourceType;
    isAzure? : boolean;
    cssClass: string;
    source: string;
    addedDate : Date;
    isDeleted : boolean;
    hovered: boolean;
    addedByUsername: string;
    isATFile: boolean;
    isCompanyAttachment: boolean;
    isViewableByEmployee: true
    onboardingWorkflowTaskId: number;
}

export enum AttachmentSourceType 
{
    LocalServerFile = 1,
    Url             = 2,
    Form            = 3,
    Video           = 4,
    AzureProfileImage = 5,
    AzureClientImage = 6,
    AzureClientFile = 7
}