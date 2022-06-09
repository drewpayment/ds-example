import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs";
import { catchError } from 'rxjs/operators';
import { HttpClient } from "@angular/common/http";
import { AccountService } from "@ds/core/account.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";
import { UserInfo } from "@ds/core/shared";
import { saveAs } from "file-saver"
import { IJobProfileBasicInfo } from './job-profile-basic-info.model';

@Injectable({
    providedIn: 'root'
})
export class JobProfileService {
    private readonly api = 'api';
    user: UserInfo;

    constructor(private http: HttpClient, private account: AccountService, private msg: DsMsgService) {
    }

    //Job Profile related services
    getJobProfileBasicInfo(jobProfileId: number): Observable<IJobProfileBasicInfo> {
        const url = `${this.api}/job-profiles/${jobProfileId}/basic-info`;
        return this.http.get<IJobProfileBasicInfo>(url)
            .pipe(
                catchError(this.httpError('getJobProfileBasicInfo', <IJobProfileBasicInfo>{}))
            );
    }

    public convertHtmlPageToPdf(dto: any) {
        const url = `${this.api}/resources/convert-to-pdf`;

        this.http.post<Blob>(url, dto, { responseType: 'blob' as 'json', observe: 'response'})
            .subscribe(response => {
                let contentType = response.headers.get('Content-Type');
                let fileName = (response.headers.get('Content-Disposition').split(';')[1].trim().split('=')[1]).replace(/"/g, '');

                const blob = new Blob([response.body], { type: contentType });
                saveAs(blob, fileName);
            });
    }

    //convertHtmlPageToPdf(dto: any) {
    //    var preview = this.restFull
    //        .one("resources")
    //        .one("convert-to-pdf");
    //    return this.downloader.downloadFile(preview, false, dto);

    //}

    private httpError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            let errorMsg = error.error.errors != null && error.error.errors.length
                ? error.error.errors[0].msg
                : error.message;

            this.account.log(error, `${operation} failed: ${errorMsg}`);

            // TODO: better job of transforming error for user consumption
            this.msg.setTemporaryMessage(`Sorry, this operation failed: ${errorMsg}`, MessageTypes.error, 6000);

            // let app continue by return empty result
            return of(result as T);
        }
    }
}
