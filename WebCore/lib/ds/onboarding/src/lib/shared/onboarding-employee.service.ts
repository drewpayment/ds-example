import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { throwError, Observable, BehaviorSubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import * as saveAs from 'file-saver';
import { UserType } from '@ds/core/shared';
import { Client } from '@ds/core/employee-services/models';
import { IEmployeeOnboardingData, II9DocumentData, IEmployeeOnboardingI9DocumentData, IEmployeeOnboardingI9Data } from '@ajs/onboarding/shared/models';
import { IEmployeeContactInfo } from '@ds/employees/profile/shared/employee-contact-info.model';
import { IFormStatusData } from '@ajs/onboarding/shared/models';

@Injectable({
    providedIn: 'root'
})
export class OnboardingEmployeeService {
    static readonly ONBOARDING_ADMIN_API_BASE = "api/onboarding/admin";    
    static readonly ONBOARDING_EMPLOYEE_API_BASE = "api/employee-onboarding";  

    private activeEmployees$ = new BehaviorSubject<IEmployeeOnboardingData>(null);
    activeEmployees: Observable<IEmployeeOnboardingData> = this.activeEmployees$.asObservable();

    constructor(private httpClient: HttpClient) {
    }
    getRelatedClientsList(clientId):Observable<Client[]> {
        return this.httpClient.get<Client[]>(
            `${OnboardingEmployeeService.ONBOARDING_ADMIN_API_BASE}/related-clients/clients/${clientId}`);
    }

    getActiveOnboardingEmployeeList(clientId):Observable<IEmployeeOnboardingData[]> {
        return this.httpClient.get<IEmployeeOnboardingData[]>(
            `${OnboardingEmployeeService.ONBOARDING_ADMIN_API_BASE}/active-onboarding-employees/clients/${clientId}`);
    }

    getI9DocumentList():Observable<II9DocumentData[]> {
        return this.httpClient.get<II9DocumentData[]>(
            `${OnboardingEmployeeService.ONBOARDING_EMPLOYEE_API_BASE}/i9/documents`);
    }

    getEmployeeOnboardingI9Docs(employeeId):Observable<IEmployeeOnboardingI9DocumentData[]> {
        return this.httpClient.get<IEmployeeOnboardingI9DocumentData[]>(
            `${OnboardingEmployeeService.ONBOARDING_ADMIN_API_BASE}/employees/${employeeId}/i9docs`);
    }

    getEmployeeI9(employeeId):Observable<IEmployeeOnboardingI9Data> {
        return this.httpClient.get<IEmployeeOnboardingI9Data>(
            `${OnboardingEmployeeService.ONBOARDING_EMPLOYEE_API_BASE}/i9/employees/${employeeId}`);
    }

    getEmployeeInfo(employeeId):Observable<IEmployeeContactInfo> {
        return this.httpClient.get<IEmployeeContactInfo>(
            `${OnboardingEmployeeService.ONBOARDING_EMPLOYEE_API_BASE}/employees/${employeeId}`);
    }

    getEmployeeOnboardingForms(employeeId):Observable<IFormStatusData[]> {
        return this.httpClient.get<IFormStatusData[]>(
            `${OnboardingEmployeeService.ONBOARDING_ADMIN_API_BASE}/employees/${employeeId}/forms`);
    }

    setActiveEmployee(val: IEmployeeOnboardingData) {
        this.activeEmployees$.next(val);
    }

    putEmployeeOnboardingFormsDocs(employeeId, formData, docData) {
        var combine = <any>{};
        combine.DocData = docData;
        combine.FormData = formData;

        return this.httpClient.put<any>(
            `${OnboardingEmployeeService.ONBOARDING_ADMIN_API_BASE}/employees/${employeeId}/forms`, combine);
    }
}