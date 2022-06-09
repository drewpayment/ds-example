import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { AccountService } from "@ds/core/account.service";
import { IEmployeeDependent } from '@ds/employees/profile/shared/employee-dependent.model';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

​
@Injectable({
    providedIn: 'root'
})
export class EmployeeDependentsApiService {
    constructor(
        private http: HttpClient,
        private account: AccountService,
    ) { }

    
    private api_employees = 'api/employees';
​
    getDependents(employeeId: number): Observable<IEmployeeDependent[]>{
        return this.http.get<IEmployeeDependent[]>(`${this.api_employees}/${employeeId}/dependents`)
            .pipe(
                catchError(this.httpError('getEmployeeDependents', <IEmployeeDependent[]>[]))
            );
    }

    getDependentByDependentId(dependentId: number): Observable<IEmployeeDependent> {
        return this.http.get<IEmployeeDependent>(`${this.api_employees}/dependent-info/${dependentId}`)
            .pipe(
            catchError(this.httpError('getDependentByDependentId', <IEmployeeDependent>{}))
            );
    }

    updateEmployeeDependent(model: IEmployeeDependent, hasEditPermissionWithoutChangeRequests: boolean): Observable<IEmployeeDependent> {
        const url = `${this.api_employees}/${model.employeeId}/dependents`;

        let headers = new HttpHeaders();
        if (hasEditPermissionWithoutChangeRequests) {
            headers = headers.append("X-No-Change-Request", "true");
        }

        return this.http.put<IEmployeeDependent>(url, model, { headers: headers })
            .pipe(
                catchError(this.httpError('updateEmployeeDependent', <IEmployeeDependent>{}))
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
