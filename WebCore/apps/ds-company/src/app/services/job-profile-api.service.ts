import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IClientDepartmentData, IJobDetailData, IJobSkillsData } from '../models/job-profile.model'
import { map } from 'rxjs/operators';
import * as saveAs from 'file-saver';

@Injectable({
    providedIn: 'root'
})

export class JobProfileApiService {
    private clientApi = `api/clients`;
    private jobProfileApi = `api/job-profiles`;
    private onboardingAdminApi = `api/onboarding/admin`;

    constructor(private http: HttpClient){}

    getJobProfileListByClient(clientId: number): Observable<IJobDetailData[]>{
        const url = `${this.jobProfileApi}/job-details/client/${clientId}/isActive`;
        return this.http.get<IJobDetailData[]>(url);
    }

    getJobProfileDetails(jobProfileId: number, clientId: number): Observable<any>{
        const url = `${this.jobProfileApi}/${jobProfileId}/clients/${clientId}`;
        return this.http.get<any>(url);
    }

    //Responsibilities
    updateJobProfileResponsibility(dto: any): Observable<any> {
        const url = `${this.jobProfileApi}/job-profile-responsibility`;
        return this.http.post<any>(url, dto);
    }

    updateJobResponsibility(dto: any): Observable<any> {
        const url = `${this.jobProfileApi}/job-responsibility`;
        return this.http.post<any>(url, dto);
    }

    deleteJobProfileResponsibility(dto: any): Observable<any>{
        const url = `${this.jobProfileApi}/job-profile-responsibility/delete`;
        return this.http.put<any>(url, dto);
    }

    //Skills
    updateJobProfileSkill(dto: any): Observable<any> {
        const url = `${this.jobProfileApi}/job-profile-skill`;
        return this.http.post<any>(url, dto);
    }

    updateJobSkill(dto: any): Observable<any> {
        const url = `${this.jobProfileApi}/job-skill`;
        return this.http.post<any>(url, dto);
    }

    ///

    deleteJobProfileSkill(dto: any): Observable<any>{
        const url = `${this.jobProfileApi}/job-profile-skill/delete`;
        return this.http.put<any>(url, dto);
    }

    getClientDepartmentsList(clientDivisionId: number): Observable<IClientDepartmentData[]>{
        const url = `${this.clientApi}/divisions/${clientDivisionId}/client-departments`;
        return this.http.get<IClientDepartmentData[]>(url);
    }

    getemployeeTasks(clientId: number): Observable<any>{
        const url = `${this.onboardingAdminApi}/client/${clientId}/workflow`;
        return this.http.get<any>(url);
    }

    getW4StateList(clientId: number): Observable<any>{
        const url = `${this.onboardingAdminApi}/client/${clientId}/w4StateList`;
        return this.http.get<any>(url);
    }

    saveJobProfile(dto: any): Observable<any> {
        const url = `${this.jobProfileApi}/save`;
        return this.http.post<any>(url, dto);
    }

    updateJobProfileStatus(dto: any): Observable<any> {
        const url = `${this.jobProfileApi}/job-profile-status`;
        return this.http.post<any>(url, dto);
    }

    updateJobProfileTitle(dto: any): Observable<any> {
        const url = `${this.jobProfileApi}/job-profile-title`;
        return this.http.post<any>(url, dto);
    }

    copyJobProfile(jobProfileId: number): Observable<any>{
        const url = `${this.jobProfileApi}/${jobProfileId}/copy`;
        return this.http.post<any>(url, jobProfileId);
    }

    public convertHtmlPageToPdf(dto: any) {
        var url = `api/resources/convert-to-pdf`;
        
        return this.http.post<Blob>(url,dto,{responseType: 'blob' as any, observe: 'response'})
        .pipe(
            map((response: HttpResponse<Blob>) => {
                saveAs(response.body, dto.fileName + '.pdf' );
                return response.body;
            })
        );
    }
}