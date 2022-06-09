import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IOnboardingWorkflowTask } from '@models';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class CustomPagesService {
    private onboardingApi = `api/onboarding`;

    constructor(private http: HttpClient){}

    getCompanyTasksByClient(clientId: number): Observable<IOnboardingWorkflowTask[]>{
        const url = `${this.onboardingApi}/companyTasksByClientId/${clientId}`;
        return this.http.get<IOnboardingWorkflowTask[]>(url);
    }

    getOnboardingWorkflowTask(taskId: number): Observable<IOnboardingWorkflowTask>{
        const url = `${this.onboardingApi}/getOnboardingWorkflowTask/${taskId}`;
        return this.http.get<IOnboardingWorkflowTask>(url);
    }

    updateOnboardingWorkflowTask(dto: any) {
        const url = `${this.onboardingApi}/putOnboardingWorkflowTask`;
        return this.http.put<IOnboardingWorkflowTask>(url, dto);
    }

    changeStatusOfWorkflowTask(dto: IOnboardingWorkflowTask): Observable<boolean>{
        const url = `${this.onboardingApi}/workflow-task/update-status`;
        return this.http.post<boolean>(url, dto);
    }

    deleteOnboardingWorkflowTask(dto: IOnboardingWorkflowTask): Observable<boolean>{
        const url = `${this.onboardingApi}/workflowTask/delete`;
        return this.http.put<boolean>(url, dto);
    }
}
