import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpParams, HttpHeaders, HttpRequest } from '@angular/common/http';
import { ApplicantDto } from "@ajs/applicantTracking/newuser/applicant-newUser-dto.model";
import { UsernameResult,JobBoardInfo } from "./shared/usernameResult.model";
import { catchError } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})

export class SignUpApiService {

    constructor(private http: HttpClient) {}

    signIn(token: string): Observable<ApplicantDto>{ 
        return this.http.post<ApplicantDto>(`api/Auth/signin`,token);
    }

    saveNewUser(applicantDto : ApplicantDto):Observable<ApplicantDto>{
        return this.http.post<ApplicantDto>(`api/ApplicantTracking/applicantNew/clients/${applicantDto.clientId}/users`, applicantDto);
    }

    validateUsername(username:string):Observable<UsernameResult>{
        let encodedUName = window.btoa( username );
        return this.http.get<UsernameResult>(`api/ApplicantTracking/applicantNew/username/${encodedUName}`)
    }

    getCompanyJobBoardInfo(clientId: number):Observable<JobBoardInfo> {
        return this.http.get<JobBoardInfo>(`api/ApplicantTracking/companyJobBoard/${clientId}`)
    }

    getCompanyLegacySetting(clientId: number):Observable<boolean> {
        return this.http.get<boolean>(`api/ApplicantTracking/legacy-security-setting/${clientId}`)
            .pipe(catchError(this.httpError('getCompanyLegacySetting', <boolean>{})))
    }

    httpError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            let errorMsg = error.error.errors != null && error.error.errors.length
                ? error.error.errors[0].msg
                : error.message;

            console.log(error, `${operation} failed: ${errorMsg}`);
            // let app continue by return empty result
            return of(result as T);
        }
    }
}
