import { Injectable } from "@angular/core";
import { Subject, ReplaySubject, Observable, of, throwError } from "rxjs";
import { catchError, defaultIfEmpty, tap, map, concat, switchMap, concatMap, share, finalize, take, publishReplay, refCount } from 'rxjs/operators';
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { AccountService } from "@ds/core/account.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";
import { UserInfo } from "@ds/core/shared";
import { IEmployeePersonalInfo } from './employee-personal-info.model';
import { IEmployeeContactInfo } from './employee-contact-info.model';
import { IEmployeeDependent, IEmployeeDependentRelationship } from './employee-dependent.model';
import { IEmergencyContact } from './emergency-contact.model';

@Injectable({
    providedIn: 'root'
})
export class EmployeeProfileService {
    private readonly api = 'api';
    user: UserInfo;

    constructor(private http: HttpClient, private account: AccountService, private msg: DsMsgService) {
    }

    //Employee Bio related services
    getEmployeePersonalInfo(employeeId: number): Observable<IEmployeePersonalInfo> {
        const url = `${this.api}/employees/${employeeId}/personal-info`;
        return this.http.get<IEmployeePersonalInfo>(url)
            .pipe(
            catchError(this.httpError('getEmployeePersonalInfo', <IEmployeePersonalInfo>{}))
            );
    }

    updateEmployeePersonalInfo(model: IEmployeePersonalInfo): Observable<IEmployeePersonalInfo> {
        const url = `${this.api}/employees/${model.employeeId}/personal-info`;
        return this.http.put<IEmployeePersonalInfo>(url, model)
            .pipe(
            catchError(this.httpError('updateEmployeePersonalInfo', <IEmployeePersonalInfo>{}))
            );
    }

    //Employee Dependents related services
    getEmployeeDependents(employeeId: number, includeInactive: boolean): Observable<IEmployeeDependent[]> {
        const url = `${this.api}/employees/${employeeId}/dependents`;
        return this.http.get<IEmployeeDependent[]>(url)
            .pipe(
                catchError(this.httpError('getEmployeeDependents', <IEmployeeDependent[]>[]))
            );
    }

    updateEmployeeDependent(model: IEmployeeDependent, hasEditPermissionWithoutChangeRequests: boolean): Observable<IEmployeeDependent> {
        const url = `${this.api}/employees/${model.employeeId}/dependents`;

        let headers = new HttpHeaders();
        if (hasEditPermissionWithoutChangeRequests) {
            headers = headers.append("X-No-Change-Request", "true");
        }

        return this.http.put<IEmployeeDependent>(url, model, { headers: headers })
            .pipe(
                catchError(this.httpError('updateEmployeeDependent', <IEmployeeDependent>{}))
            );
    }

    //Emergency Contacts related services
    getEmergencyContacts(employeeId: number): Observable<IEmergencyContact[]> {
        const url = `${this.api}/employees/${employeeId}/emergency-contacts/ess`;
        return this.http.get<IEmergencyContact[]>(url)
            .pipe(
            catchError(this.httpError('getEmergencyContacts', <IEmergencyContact[]>[]))
            );
    }

    updateEmergencyContact(model: IEmergencyContact, hasEditPermission: boolean): Observable<IEmergencyContact> {
        const url = `${this.api}/employees/${model.employeeId}/emergency-contacts`;

        let headers = new HttpHeaders();
        if (hasEditPermission) {
            headers = headers.append("X-No-Change-Request", "true");
        }

        return this.http.put<IEmergencyContact>(url, model, { headers: headers })
            .pipe(
                catchError(this.httpError('updateEmergencyContact', <IEmergencyContact>{}))
            );
    }

    //Employee Contact Info related services
    getEmployeeContactInfo(employeeId: number): Observable<IEmployeeContactInfo> {
        const url = `${this.api}/employees/${employeeId}/contact-info`;
        return this.http.get<IEmployeeContactInfo>(url)
            .pipe(
            catchError(this.httpError('getEmployeeContactInfo', <IEmployeeContactInfo>{}))
            );
    }

    updateEmployeeContactInfo(model: IEmployeeContactInfo, hasEditPermission: boolean): Observable<IEmployeeContactInfo> {
        const url = `${this.api}/employees/${model.employeeId}/contact-info`;

        let headers = new HttpHeaders();
        if (hasEditPermission) {
            headers = headers.append("X-No-Change-Request", "true");
        }

        return this.http.put<IEmployeeContactInfo>(url, model, { headers: headers })
            .pipe(
                catchError(this.httpError('updateEmployeeContactInfo', <IEmployeeContactInfo>{}))
            );
    }

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

    getRelationshipList(): Observable<IEmployeeDependentRelationship[]>{
        return this.http.get<IEmployeeDependentRelationship[]>(`api/dependent-relationships`)
            .pipe(
                catchError(this.httpError('getRelationshipList', <IEmployeeDependentRelationship[]>{}))
            );
    }
}
