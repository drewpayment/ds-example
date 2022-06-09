import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { throwError, Observable, BehaviorSubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import * as saveAs from 'file-saver';
import { UserType } from '@ds/core/shared';
import { IEmployeeContactInfo } from '@ds/employees/profile/shared/employee-contact-info.model';
import { IFormStatusData } from '@ajs/onboarding/shared/models';
import { IEmployeeDependent, IEmployeeDependentRelationship } from '../models/employee-dependents/employee-dependent.model';

@Injectable({
    providedIn: 'root'
})
export class EmployeeDependentsService {
    static readonly EMPLOYEE_API_BASE = "api/employees";
    static readonly EMPLOYEE_DEPENDENTS_API_BASE = "api/employee-dependents";

    private activeDependent$ = new BehaviorSubject<IEmployeeDependent>(null);
    activeDependent: Observable<IEmployeeDependent> = this.activeDependent$.asObservable();

    constructor(private httpClient: HttpClient) {
    }

    getEmployeeDependents(employeeId):Observable<IEmployeeDependent[]> {
        return this.httpClient.get<IEmployeeDependent[]>(
            `${EmployeeDependentsService.EMPLOYEE_DEPENDENTS_API_BASE}/${employeeId}/dependents`);
    }

    getEmployeeDependentRelationships():Observable<IEmployeeDependentRelationship[]> {
        return this.httpClient.get<IEmployeeDependentRelationship[]>(
            `${EmployeeDependentsService.EMPLOYEE_API_BASE}/dependent-relationships`);
    }

    setActiveDependent(val: IEmployeeDependent) {
        this.activeDependent$.next(val);
    }

    saveEmployeeDependent(employeeId, data: IEmployeeDependent, hasEditPermissionWithoutChangeRequests: boolean) {
        let headers = new HttpHeaders();
        if (hasEditPermissionWithoutChangeRequests) {
            headers = headers.append("X-No-Change-Request", "true");
        }

        return this.httpClient.put<IEmployeeDependent>(
            `${EmployeeDependentsService.EMPLOYEE_API_BASE}/${employeeId}/dependents`, data, { headers: headers });
    }
    
    deleteActiveDependent(data: IEmployeeDependent) {
        return this.httpClient.delete<IEmployeeDependent>(
            `${EmployeeDependentsService.EMPLOYEE_DEPENDENTS_API_BASE}/${data.employeeId}/dependents/${data.employeeDependentId}`);
    }
}