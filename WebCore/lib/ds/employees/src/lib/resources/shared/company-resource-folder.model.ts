import { ICompanyResource } from './company-resource.model';

export interface ICompanyResourceFolder {
    companyResourceFolderId: number,
    clientId: number,
    description: string,
    isNew: boolean,
    resourceCount: number,
    newResourceId: number,
    resourceList: ICompanyResource[]
}
