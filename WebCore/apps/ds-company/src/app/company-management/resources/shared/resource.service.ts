import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { map, tap } from 'rxjs/operators';
import * as saveAs from 'file-saver';
import { ICompanyResourceFolder, ICompanyResource } from '@models';

@Injectable({
    providedIn: 'root'
})
export class ResourceService {

    static readonly CLIENT_API_BASE = "api/companyresource";

    constructor(private httpClient: HttpClient) {

    }

    getCompanyResourceFolderByClient(clientId: number) {
        return this.httpClient.get<ICompanyResourceFolder[]>(
            `${ResourceService.CLIENT_API_BASE}/companyResourceFolders/${clientId}`);
    }

    getCompanyResourceFolderByFolder(folderId: number) {
        return this.httpClient.get<ICompanyResourceFolder[]>(
            `${ResourceService.CLIENT_API_BASE}/companyResourceFolder/${folderId}`);
    }

    updateCompanyResourceFolder(dto) {
        return this.httpClient.put<ICompanyResourceFolder>(
            `${ResourceService.CLIENT_API_BASE}/putCompanyResourceFolder` , dto);
    }

    deleteCompanyResourceFolder(sourceFolderId, destFolderId) {
        let destFolderIdStr: string = destFolderId ? destFolderId.toString() : 'null';

        return this.httpClient.put<ICompanyResourceFolder>(
            `${ResourceService.CLIENT_API_BASE}/deleteCompanyResourceFolder/sourceFolderId/${sourceFolderId}` +
            `/destFolderId/${destFolderIdStr}` , {});
    }

    getCompanyResourcesByClient(clientId) {
        return this.httpClient.get<ICompanyResource[]>(
            `${ResourceService.CLIENT_API_BASE}/companyResourcesByClientId/${clientId}`);
    }

    deleteCompanyResource(dto) {
        return this.httpClient.put<ICompanyResource>(
            `${ResourceService.CLIENT_API_BASE}/deleteCompanyResource` , dto);
    }

    getCompanyResourcesByEmployee(employeeId, clientId) {
        return this.httpClient.get<ICompanyResource[]>(`api/employees/${employeeId}/resources`);
    }


    doesFileResourceExist(resourceId) {
        return this.httpClient.get<ICompanyResource>(
            `${ResourceService.CLIENT_API_BASE}/resource/${resourceId}/info`);
    }

    getFileResourceToDownload(resource:ICompanyResource) {
        let url = `${ResourceService.CLIENT_API_BASE}/resource/${resource.resourceId}/download`;
        return this.httpClient.post<Blob>(url,{},{responseType: 'blob' as any, observe: 'response'})
        .pipe(
            map((response: HttpResponse<Blob>) => {
                saveAs(response.body, resource.resourceName + resource.resourceFormat );
                return response.body;
            })
        );
    }

    updateCompanyResource(dto) {
        return this.httpClient.put<ICompanyResource>(
            `${ResourceService.CLIENT_API_BASE}/putCompanyResource` , dto);
    }

    isCompanyResourceExistsInTask(resourceId) {
        return this.httpClient.get<boolean>(
            `${ResourceService.CLIENT_API_BASE}/isCompanyResourceExistsInTask/${resourceId}`);
    }

    deleteCompanyResourceWithTask(dto) {
        return this.httpClient.put<ICompanyResource>(
            `${ResourceService.CLIENT_API_BASE}/deleteCompanyResourceWithTask` , dto);
    }

    uploadCompanyResource(dto:ICompanyResource, resource: File){
        // Create form data
        const formData = new FormData();

        // Store form name as "file" with file data
        formData.append("file", resource, resource.name);
        formData.append("folderId", dto.companyResourceFolderId.toString());
        formData.append("objectData", JSON.stringify(dto));

        return this.httpClient.post<ICompanyResource>(
            `${ResourceService.CLIENT_API_BASE}/uploadCompanyResource` , formData);
    }
}
