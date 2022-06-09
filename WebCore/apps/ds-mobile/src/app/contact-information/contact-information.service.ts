import { Injectable } from '@angular/core';
import { HttpParams, HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Country } from '../../models';
import { State } from '@ds/core/employee-services/models';
import { IEmployeeContactInfo } from '@ds/employees/profile/shared/employee-contact-info.model';
import { AccountService } from "@ds/core/account.service";
import { catchError } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class ContactInformationService {
    private readonly api_employees = 'api/employees';
    constructor(
        private http: HttpClient,
        private account: AccountService
    ) { }

    getEmployeeContactInfo(employeeId: number): Observable<IEmployeeContactInfo> {
        const url = `${this.api_employees}/${employeeId}/contact-info`;
        return this.http.get<IEmployeeContactInfo>(url)
            .pipe(
                catchError(this.httpError('getEmployeeContactInfo', <IEmployeeContactInfo>{}))
            );
    }

    getCountries(): Observable<Country[]> {
        const url = `api/location/countries`;
        return this.http.get<Country[]>(url);
    }
    
    getStatesByCountryId(countryId: number): Observable<State[]> {
        const url = `api/location/countries/${countryId}/states`;
        return this.http.get<State[]>(url);
    }
    
    updateEmployeeContactInfo(model: IEmployeeContactInfo, hasEditPermission: boolean): Observable<IEmployeeContactInfo> {
        const url = `${this.api_employees}/${model.employeeId}/contact-info`;

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

            // let app continue by return empty result
            return of(result as T);
        }
    }
}
