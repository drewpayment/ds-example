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
import { IOnboardingAdminTaskList, IOnboardingAdminTask, IJobProfileDetail } from '../shared/onboarding-admin-task-list.model';

@Injectable({
    providedIn: 'root'
})
export class OnboardingAdminService {
    static readonly ONBOARDING_ADMIN_API_BASE = "api/onboarding/admin";    
    static readonly ONBOARDING_CLIENT_API_BASE = "api/client";  

    private activeTaskList$ = new BehaviorSubject<IOnboardingAdminTaskList>(null);
    activeTaskList: Observable<IOnboardingAdminTaskList> = this.activeTaskList$.asObservable();

    constructor(private httpClient: HttpClient) {
    }
    getOnboardingAdminTaskList(clientId):Observable<IOnboardingAdminTaskList[]> {
        return this.httpClient.get<IOnboardingAdminTaskList[]>(
            `${OnboardingAdminService.ONBOARDING_ADMIN_API_BASE}/onboarding-task-list/${clientId}`);
    }
    

    setActiveTaskList(val: IOnboardingAdminTaskList) {
        this.activeTaskList$.next(val);
    }

    putMultipleAdminTaskList(data: IOnboardingAdminTaskList[]) {
        return this.httpClient.put<IOnboardingAdminTaskList[]>(
            `${OnboardingAdminService.ONBOARDING_ADMIN_API_BASE}/onboarding-task-list/update-multiple-task-list`, data);
    }

    getJobProfiles(clientId: number): Observable<IJobProfileDetail[]>{
        return this.httpClient.get<IJobProfileDetail[]>(
            `${OnboardingAdminService.ONBOARDING_CLIENT_API_BASE}/${clientId}/job-profiles`);
    }
    
    deleteActiveTaskList(data: IOnboardingAdminTaskList) {
        return this.httpClient.put<IOnboardingAdminTaskList[]>(
            `${OnboardingAdminService.ONBOARDING_ADMIN_API_BASE}/onboardingTaskList/delete`, data);
    }
}