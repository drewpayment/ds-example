import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { map, tap } from 'rxjs/operators';
import * as saveAs from 'file-saver';
import { IEmployeeAttachmentFolder, IEmployeeAttachment } from '@models/attachment.model';

@Injectable({
    providedIn: 'root'
})
export class AttachmentService {

    static readonly CLIENT_API_BASE = "api/employeeAttachment";

    constructor(private httpClient: HttpClient) {

    }

    getEmployeeFolderList(employeeId: number, clientId: number, employeeVisible: boolean) {
        return this.httpClient.get<IEmployeeAttachmentFolder[]>(
            `${AttachmentService.CLIENT_API_BASE}/folderList/employeeId/${employeeId || 0}/clientId/${clientId}/employeeVisible/${employeeVisible}`);
    }

    updateEmployeeAttachmentFolder(dto) {
        return this.httpClient.put<IEmployeeAttachmentFolder>(
            `${AttachmentService.CLIENT_API_BASE}/updateEmployeeAttachmentFolder` , dto);
    }

    getFileResourceToDownload(resource:IEmployeeAttachment, isFromESS?: boolean) {
        let url="";
        if (resource.isAzure) {
            let url = `api/resources/${resource.resourceId}/azure-link/${isFromESS ? isFromESS : false}`;
            return this.httpClient.get<string>(url).subscribe(x=> {
                window.open(x,'_blank');
            });
        }
        else
        {
            let url = `api/resources/${resource.resourceId}`;
            return this.httpClient.post<Blob>(url,{},{responseType: 'blob' as any, observe: 'response'})
            .pipe(
                map((response: HttpResponse<Blob>) => {
                    saveAs(response.body, resource.name + resource.extension );
                    return response.body;
                })
            ).subscribe();
        }
    }

    uploadEmployeeAttachment(dto:IEmployeeAttachment, resource: File){
        // Create form data
        const formData = new FormData();

        // Store form name as "file" with file data
        formData.append("file", resource, resource.name);
        formData.append("folderId", dto.folderId.toString());
        formData.append("objectData", JSON.stringify(dto));

        return this.httpClient.post<IEmployeeAttachment>(
            `${AttachmentService.CLIENT_API_BASE}/uploadEmployeeAttachment` , formData);
    }

    uploadEmployeeAttachmentInfo(dto:IEmployeeAttachment){
        return this.httpClient.post<IEmployeeAttachment>(
            `${AttachmentService.CLIENT_API_BASE}/updateEmployeeAttachmentInfo` , dto);
    }

    isAttachmentForeignKey(resourceId: number) {
        return this.httpClient.get<any>(
            `${AttachmentService.CLIENT_API_BASE}/deleteEmployeeAttachment/checkFKDependency/resourceId/${resourceId}`);
    }

    removeEmployeeAttachmentForeignKeyRecord(dto:any) {
        return this.httpClient.post<any>(
            `${AttachmentService.CLIENT_API_BASE}/deleteEmployeeAttachment/delete-attachment-FK/${dto.attachmentId}` , dto);
    }

    deleteEmployeeAttachment(resourceId: number, isCompanyAttachment: Boolean) {
        return this.httpClient.post<any>(
            `${AttachmentService.CLIENT_API_BASE}/deleteEmployeeAttachment/resourceId/${resourceId}/${isCompanyAttachment}`,null);
    }

    deleteEmployeeFolder(folderId: number) {
        return this.httpClient.post<any>(
            `${AttachmentService.CLIENT_API_BASE}/deleteEmployeeFolder/folderId/${folderId}`,null);
    }

    getUrlToDownload(url:string, fileName: string){
        
        return this.httpClient.post<Blob>(url,{},{responseType: 'blob' as any, observe: 'response'})
        .pipe(
            map((response: HttpResponse<Blob>) => {
                saveAs(response.body, fileName );
                return response.body;
            })
        );
      }
}
