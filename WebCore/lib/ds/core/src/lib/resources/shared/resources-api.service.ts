import { Injectable } from "@angular/core";
import { Subject, ReplaySubject, Observable, of, throwError } from "rxjs";
import { catchError, defaultIfEmpty, tap, map, concat, switchMap, concatMap, share, finalize, take, publishReplay, refCount } from 'rxjs/operators';
import { AccountService } from "@ds/core/account.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
//import { MessageService } from "@ds/core/message.service";
import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";
import { UserInfo } from "@ds/core/shared";
import { saveAs } from "file-saver";
// import * as saveAs from 'file-saver';
import { HttpErrorResponse, HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { IProfileImageDto, ProfileImageDto } from '@ds/core/resources/shared/profile-image.model';
import { IImage, IEmployeeImage } from '@ds/core/resources/shared/employee-image.model';
import { IAzureViewDto } from '@ds/core/resources/shared/azure-view-dto.model';
import { IEmployeeAvatars } from "@ds/core/employees/shared/employee-avatars.model";

@Injectable({
    providedIn: 'root'
})
export class ResourceApiService {
    private readonly api = 'api';
    user: UserInfo;

    constructor(private http: HttpClient, private httpClient: HttpClient, private account: AccountService, private msg: DsMsgService) {
    }

    deleteClientImageResource(
        clientId: number,
        resourceId: number,
        name: string
    ): Observable<boolean> {
        const url = `${this.api}/resources/${resourceId}/clients/${clientId}/files/${name}`;
        return this.httpClient.delete<boolean>(url)
        .pipe(
            catchError(this.httpError('deleteClientImageResource', <boolean>{}))
        );
    }
    //deleteClientImageResource(
    //    clientId: number,
    //    resourceId: number,
    //    name: string
    //): ng.IPromise<boolean> {
    //    return this.rest
    //        .one('resources', resourceId)
    //        .one('clients', clientId)
    //        .one('files', name)
    //        .remove();
    //}

    // /api/resources/clients/{clientId}/employees/{employeeId}/profile-images
    deleteProfileImage(
        clientId: number,
        employeeId: number
    ): Observable<boolean> {
        const url = `${this.api}/resources/clients/${clientId}/employees/${employeeId}/profile-images`;
        return this.httpClient.delete<boolean>(url)
        .pipe(
            catchError(this.httpError('deleteProfileImage', <boolean>{}))
        );
    }
    //deleteProfileImage(
    //    clientId: number,
    //    employeeId: number
    //): ng.IPromise<boolean> {
    //    return this.rest
    //        .all('resources')
    //        .one('clients', clientId)
    //        .one('employees', employeeId)
    //        .one('profile-images')
    //        .remove();
    //}

    saveProfileImage(
        clientId: number,
        employeeId: number,
        dataUrl: string
    ): Observable<ProfileImageDto> {
        const url = `${this.api}/resources/clients/${clientId}/employees/${employeeId}/profile-images`;

        const data = { dataUrl: dataUrl };

        return this.httpClient.post<ProfileImageDto>(url, data)
            .pipe(
                catchError(this.httpError('saveClientImageResource', <ProfileImageDto>{}))
            );
    }

    saveClientImageResource(
        resourceId: number,
        clientId: number,
        name: string,
        dataUrl: string
    ): Observable<IAzureViewDto> {
        const url = `${this.api}/resources/${resourceId || 0}/clients/${clientId}/files/${name}`;
        return this.httpClient.post<IAzureViewDto>(url, { dataUrl:  dataUrl})
        .pipe(
            catchError(this.httpError('saveClientImageResource', <IAzureViewDto>{}))
        );
    }
    //saveClientImageResource(
    //    resourceId: number,
    //    clientId: number,
    //    name: string,
    //    dataUrl: string
    //): ng.IPromise<IAzureViewDto> {
    //    return this.rest
    //        .one('resources', resourceId || 0)
    //        .one('clients', clientId)
    //        .one('files', name)
    //        .customPOST({ dataUrl: dataUrl })
    //        .then(this.dsApi.stripRestangular);
    //}

    getProfileImageSas(clientId: number, employeeId: number): Observable<string> {
        const url = `${this.api}/resources/clients/${clientId}/employees/${employeeId}/blob-sas`;
        var options = {
            headers: new HttpHeaders({
                'Cache-Control': 'private, no-cache'
            })
        };
        return this.httpClient.get<string>(url, options)
        .pipe(
            catchError(this.httpError('getProfileImageSas', <string>{}))
        );
    }

    downloadResource(resourceId: number, ext: string) {
        const acceptHeader = '*/*';

        const url = `${this.api}/resources/${resourceId}`;

        let headers = new HttpHeaders();
        headers = headers.set('Accept', `${acceptHeader}`);

        this.http.get(url, {
            headers: headers,
            responseType: 'blob',
            observe: 'response'
        })
            .subscribe((response: HttpResponse<Blob>) => {
                // const fileUrl = URL.createObjectURL(response.body);
                // window.open(fileUrl);
                // let contentType = response.headers.get('Content-Type');
                const contentDispositions = response.headers.get('Content-Disposition').split(';');
                if (contentDispositions == null || !contentDispositions.length || contentDispositions[1] == null) {
                    saveAs(response.body, `resource_${resourceId + ext}`);
                    return;
                }

                const contentDispositionParts = contentDispositions[1].trim().split('=');
                if (contentDispositionParts == null || !contentDispositionParts.length || contentDispositionParts[1] == null) {
                    saveAs(response.body, `resource_${resourceId + ext}`);
                    return;
                }

                const filename = contentDispositionParts != null && contentDispositionParts.length
                    ? contentDispositionParts[1].replace(/"/g, '')
                    : `resource_${resourceId + ext}`;

                saveAs(response.body, filename);
            });
    }

    getAzureLink(resourceId: number) {
        const url = `${this.api}/resources/${resourceId}/azure-link/true`;
        return this.http.get<HttpResponse<Blob>>(url);
    }

    public openAzureLink(resourceId) {
        const url = `${this.api}/resources/${resourceId}/azure-link/true`;

        this.http.get<HttpResponse<Blob>>(url)
        .subscribe((response) => {
            // let contentType = response.headers.get('Content-Type');
            // let fileName = (response.headers.get('Content-Disposition').split(';')[1].trim().split('=')[1]).replace(/"/g, '');

            // saveAs(response, fileName);
            saveAs(response);
        });

        // this.http.get(url)
        //     .subscribe((res) => {
        //         console.log(res);
        //         saveAs(res);
        //     });
    }

    public getEmployeeProfileImages(clientId: number, employeeId: number): Observable<IEmployeeImage> {
        const url = `${this.api}/resources/clients/${clientId}/employees/${employeeId}`;
        var options = {
            headers: new HttpHeaders({
                'Cache-Control': 'private, no-cache'
            })
        };
        return this.httpClient.get<IEmployeeImage>(url, options)
            .pipe(
                catchError(this.httpError('getEmployeeProfileImages', <IEmployeeImage>{}))
            );
    }

    private httpError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {

         this.showError(error, operation);
            // let app continue by return empty result
         return of(result as T);
        }
    }

    private showError(error, operation){
        let errorMsg = error.error.errors != null && error.error.errors.length
        ? error.error.errors[0].msg
        : error.message;

    this.account.log(error, `${operation} failed: ${errorMsg}`);

    // TODO: better job of transforming error for user consumption
    this.msg.setTemporaryMessage(`Sorry, this operation failed: ${errorMsg}`, MessageTypes.error, 6000);
    //this.msg.open(`Error : Sorry, this operation failed: ${errorMsg}`, 'X');
    }

    getEmployeeAvatar(employeeId: number) {
        return this.http.get<IEmployeeAvatars>(`api/employees/${employeeId}/avatar`)
    }
}
