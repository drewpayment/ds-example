import { Injectable } from '@angular/core';
import { HttpParams, HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { IEmployeeAttachment } from './employee-attachment.model';
import { map } from 'rxjs/operators';
import * as saveAs from 'file-saver';

@Injectable({
  providedIn: 'root'
})
export class EmployeeAttachmentApiService {

  constructor(
    private http: HttpClient
  ) { }

  getEmployeeAttachmentFolderList(employeeId: number, clientId: number, employeeVisible: boolean) {
    return this.http.get<any[]>(`api/employeeAttachment/folderList/employeeId/${employeeId}/clientId/${clientId}/employeeVisible/${employeeVisible}`);
  }

  getEmployeeAttachmentsByResourceIds(attachmentIds: number[], employeeId: number, clientId: number, employeeVisible: boolean) {
    return this.http.post<IEmployeeAttachment[]>(`api/employeeAttachment/folderList/employeeId/${employeeId}/clientId/${clientId}/employeeVisible/${employeeVisible}/ResourceIds`, attachmentIds);
  }

  getFileResourceToDownload(resource:IEmployeeAttachment) {
    let url = `api/resources/download-resource/${resource.resourceId}`;
    return this.http.post<Blob>(url,{},{responseType: 'blob' as any, observe: 'response'})
    .pipe(
        map((response: HttpResponse<Blob>) => {
            saveAs(response.body, resource.name + resource.extension );
            return response.body;
        })
    );
  }

  getUrlToDownload(url:string, fileName: string){
    console.log(url);
    
    return this.http.post<Blob>(url,{},{responseType: 'blob' as any, observe: 'response'})
    .pipe(
        map((response: HttpResponse<Blob>) => {
            saveAs(response.body, fileName );
            return response.body;
        })
    );
  }

  updateEmployeeAttachment(dto:IEmployeeAttachment) {
    return this.http.post<IEmployeeAttachment>(
        `api/employeeAttachment/updateEmployeeAttachmentInfo` , dto);
  }

  uploadEmployeeAttachment(dto:IEmployeeAttachment, resource: File){
      // Create form data 
      const formData = new FormData();  
      
      // Store form name as "file" with file data 
      formData.append("file", resource, resource.name);
      formData.append("objectData", JSON.stringify(dto));

      return this.http.post<IEmployeeAttachment>(
          `api/employeeAttachment/uploadEmployeeAttachment` , formData);
  }
}
