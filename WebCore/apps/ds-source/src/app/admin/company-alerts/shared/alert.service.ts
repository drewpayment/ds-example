import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import { IAlert, AlertType } from '../../../../../../../lib/models/src/lib/alert.model';
import { map, tap } from 'rxjs/operators';
import * as saveAs from 'file-saver';
import { UserType } from '@ds/core/shared';

@Injectable({
    providedIn: 'root'
})
export class AlertService {
    static readonly CLIENT_API_BASE = "api/companyalert";    
    constructor(private httpClient: HttpClient) {
    }
    getCompanyAlertsByClient(clientId: number, includeExpired: boolean = false) {
        return this.httpClient.get<IAlert[]>(
            `${AlertService.CLIENT_API_BASE}/clientId/${clientId}/include-expired/${includeExpired}`);
    }
    getCompanyAlertByAlert(alertId: number) {
        return this.httpClient.get<IAlert[]>(
            `${AlertService.CLIENT_API_BASE}/alert/${alertId}`);
    }

    test() {
        return this.httpClient.put<IAlert>(
            `${AlertService.CLIENT_API_BASE}/putTest` , null);
    }

    updateAlert(dto) {
        return this.httpClient.put<IAlert>(
            `${AlertService.CLIENT_API_BASE}/putAlert` , dto);
    }

    uploadAlert(dto:IAlert, resource: File){
        // Create form data 
        const formData = new FormData();  

        // Store form name as "file" with file data 
        formData.append("file", resource, resource.name);
        formData.append("objectData", JSON.stringify(dto));

        return this.httpClient.post<IAlert>(
            `${AlertService.CLIENT_API_BASE}/uploadAlert` , formData);
    }

    getFileAlertToDownload(item:IAlert) {
        let extn = item.alertLink.substring(item.alertLink.lastIndexOf('.') )
        let url = `${AlertService.CLIENT_API_BASE}/alert/${item.alertId}/download`;
        return this.httpClient.post<Blob>(url,{},{responseType: 'blob' as any, observe: 'response'})
        .pipe(
            map((response: HttpResponse<Blob>) => {
                saveAs(response.body, item.title + extn );
                return response.body;
            })
        );
    }
}