import { ResourceSourceType } from './resource-source-type';
import { CompanyResourceSecurityLevel } from './company-resource-security-level';

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
}