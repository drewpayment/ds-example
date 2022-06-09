import { Injectable } from '@angular/core';
import { HttpParams, HttpClient, HttpHeaders } from '@angular/common/http';
import { AccountService } from "@ds/core/account.service";
import { IEmergencyContact } from '@ds/employees/profile/shared/emergency-contact.model';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
​
@Injectable({
    providedIn: 'root'
})
export class EmergencyContactsApiService {
    private readonly api_employees = 'api/employees';​
    constructor(
        private http: HttpClient,
        private account: AccountService
    ) { }
​
    getEmergencyContactsByEmployeeId(employeeId: number): Observable<IEmergencyContact[]> {
        const url = `${this.api_employees}/${employeeId}/emergency-contacts/ess`;
        return this.http.get<IEmergencyContact[]>(url)
            .pipe(
            catchError(this.httpError('getEmergencyContactsByEmployeeId', <IEmergencyContact[]>[]))
            );
    }

    getEmergencyContactByContactId(contactId: number): Observable<IEmergencyContact> {
        return this.http.get<IEmergencyContact>(`${this.api_employees}/emergency-contacts/${contactId}`)
            .pipe(
            catchError(this.httpError('getEmergencyContactByContactId', <IEmergencyContact>{}))
            );
    }

    updateEmergencyContact(model: IEmergencyContact, hasEditPermissionWithoutChangeRequests: boolean): Observable<IEmergencyContact> {
        const url = `${this.api_employees}/${model.employeeId}/emergency-contacts`;

        let headers = new HttpHeaders();
        if (hasEditPermissionWithoutChangeRequests) {
            headers = headers.append("X-No-Change-Request", "true");
        }

        return this.http.put<IEmergencyContact>(url, model, { headers: headers })
            .pipe(
            catchError(this.httpError('updateEmergencyContact', <IEmergencyContact>{}))
            );
    }

    private httpError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            let errorMsg = error.error.errors != null && error.error.errors.length
                ? error.error.errors[0].msg
                : error.message;

            this.account.log(error, `${operation} failed: ${errorMsg}`);

            // let app continue by return empty result
            return of(result as T);
        }
    }

}
