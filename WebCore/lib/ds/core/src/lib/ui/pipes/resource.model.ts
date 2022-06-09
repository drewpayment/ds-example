import { Moment } from 'moment';

export interface ICompanyResourceFolder
{
    companyResourceFolderId : number,
    clientId : number,
    description: string,
    isNew : boolean,
    resourceCount: number,
    resourceList: Array<ICompanyResource>,
    newResourceId: number,
    hovered: boolean,
}

export interface ICompanyResource {
    clientId?: number,
    companyResourceFolderId: number,
    resourceId: number,
    resourceName: string,
    resourceFormat: string,
    securityLevel: CompanyResourceSecurityLevel,
    resourceTypeId: ResourceSourceType,
    isManagerLink: boolean,
    modified: Date,
    modifiedBy: number,
    doesFileExist?: boolean,
    azureAccount?: number,
    cssClass: string,
    source: string,
    currentSource: string,
    isNew: boolean,
    isSelectedResource: boolean,
    previewResourceCssClass: boolean,
    addedDate: Date,
    addedBy: number,
    isDeleted: boolean,
    isFileReselected: boolean,
    isAzure?: boolean,
    isATFile?: boolean,
    name: string,
    sourceType: ResourceSourceType,
    // UI ONLY
    azureUrl: string;
    hovered?: boolean;
}

export enum CompanyResourceSecurityLevel
{
    SystemAdminOnly  = 1,
    CompanyAdminHigher  = 2,
    AllEmployees = 3,
    SupervisorsAndHigher =4    
}

export enum ResourceSourceType 
{
    LocalServerFile = 1,
    Url             = 2,
    Form            = 3,
    Video           = 4,
    AzureProfileImage = 5,
    AzureClientImage = 6,
    AzureClientFile = 7
}