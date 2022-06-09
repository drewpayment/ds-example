import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import * as saveAs from 'file-saver';
import { ICompanyResourceFolder, ICompanyResource } from '@models';

@Injectable({
    providedIn: 'root'
})
export class ResourceService {

    private resourceApi = "api/resources";
    private onboardingApi = "api/onboarding";
    private companyResourcesApi = "api/companyresource";

    constructor(private http: HttpClient){}

    getCompanyResourceFoldersByClient(clientId: number): Observable<ICompanyResourceFolder[]> {
        const url = `${this.companyResourcesApi}/companyResourceFolders/${clientId}`;
        return this.http.get<ICompanyResourceFolder[]>(url);
    }

    getCompanyResourcesByClient(clientId: number): Observable<ICompanyResource[]> {
        const url = `${this.companyResourcesApi}/companyResourcesByClientId/${clientId}`;
        return this.http.get<ICompanyResource[]>(url);
    }

    getOnboardingWorkflowResourceByResourceId(resourceId: number): Observable<ICompanyResource> {
        const url = `${this.onboardingApi}/getOnboardingWorkflowResource/${resourceId}`;
        return this.http.get<ICompanyResource>(url);
    }

    downloadResource(resource:ICompanyResource) {
        let url = `${this.resourceApi}/download-resource/${resource.resourceId}/`;
        return this.http.post<Blob>(url,{},{responseType: 'blob' as any, observe: 'response'})
        .pipe(
            map((response: HttpResponse<Blob>) => {
                saveAs(response.body, resource.resourceName + resource.resourceFormat );
                return response.body;
            })
        );
    }

    updateCompanyResource(dto): Observable<any> {
        const url = `${this.companyResourcesApi}/putCompanyResource`;
        return this.http.put<ICompanyResource>(url , dto);
    }

    uploadCompanyResource(dto:ICompanyResource, resource: File): Observable<any> {
        // Create form data
        const formData = new FormData();

        // Store form name as "file" with file data
        formData.append("file", resource, resource.name);
        formData.append("folderId", dto.companyResourceFolderId.toString());
        formData.append("objectData", JSON.stringify(dto));

        const url = `${this.companyResourcesApi}/uploadCompanyResource`;
        return this.http.post<ICompanyResource>(url, formData);
    }
}
