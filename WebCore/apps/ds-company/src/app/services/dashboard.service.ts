import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { IOnboardingEmployee,IOnboardingAdminTask } from "apps/ds-company/src/app/models/onboarding-employee.model";
import { isNotUndefinedOrNull } from "@util/ds-common";
import { IEmployeeStatus } from '@ajs/employee/models';
import { IOnboardingAdminTaskList } from '../company-management/shared/onboarding-admin-task-list.model';

@Injectable({
    providedIn: 'root'
})
export class DashboardService {

    private onboardingAdminApi = "api/onboarding/admin";
    private onboardingApi = "api/onboarding";
    private companyResourcesApi = "api/companyresource";
    private employeeApi = "api/employees";
    private clientApi = "api/clients";
    private eeocApi = "api/eeoc";
    private jobProfileApi = "api/job-profiles";
    private payrollApi = "api/payroll";
    private laborApi = "api/employee-labor-management";
    private accountsApi = "api/account";

    constructor(private http: HttpClient){}

    getEmployeeList(clientId, options:{isOnboardingComplete?:boolean, employeeId?:number, startDate?:string, endDate?:string} = null): Observable<IOnboardingEmployee[]> {
        let params = new HttpParams();
        if(isNotUndefinedOrNull(options.isOnboardingComplete))
	{
            params = params.append('isOnboardingComplete', options.isOnboardingComplete.toString());
            params = params.append('startDate', options.startDate);
            params = params.append('endDate', options.endDate);
        }
        if(isNotUndefinedOrNull(options.employeeId))
            params = params.append('employeeId', options.employeeId.toString());

        const url = `${this.onboardingAdminApi}/employees/${clientId}`;
        return this.http.get<IOnboardingEmployee[]>(url,{ params: params });
    }

    getRelatedClientsList(clientId) {
        const url = `${this.onboardingAdminApi}/relatedClients/${clientId}`;
        return this.http.get<any[]>(url);
    }

    putEmployeeFromESSRemove(employeeId) {
        const url = `${this.onboardingAdminApi}/employee/${employeeId}/removeFromEss`;
        return this.http.put<{data:number}>(url , employeeId);
    }

    putEmployeeOnboardingRemove(employeeId) {
        const url = `${this.onboardingAdminApi}/employee/${employeeId}/remove`;
        return this.http.put<{data:number}>(url , employeeId);
    }

    putRollbackPayrollReady(employeeId) {
        const url = `${this.onboardingAdminApi}/employee/${employeeId}/rollbackPayrollReady`;
        return this.http.put<{data:number}>(url , employeeId);
    }

    completeOnboarding(dto) {
        const url = `${this.onboardingAdminApi}/employees/completeOnboarding`;
        return this.http.put<any>(url , dto);
    }

    importTaskLists(selectedTaskLists:IOnboardingAdminTaskList, employeeId) {
        console.log(selectedTaskLists.onboardingAdminTasks.length);
        const url = `${this.onboardingAdminApi}/onboardingTaskList/importTaskLists/employeeId/${employeeId}`;
        return this.http.put<any>(url , selectedTaskLists);
    }

    deleteEmployeeOnboardingAdminTask(dto) {
        const url = `${this.onboardingAdminApi}/adminTask/delete`;
        return this.http.put<any>(url , dto);
    }

    updateEmployeeOnboardingAdminTask(dto: IOnboardingAdminTask) {
        let url = `${this.onboardingAdminApi}/adminTask/update`;
        if(!dto.employeeOnboardingTaskId)
            url = `${this.onboardingAdminApi}/adminTask/add`;

        return this.http.put<any>(url , dto);
    }

    getEmployeeStatusList(): Observable<IEmployeeStatus[]> {
        return this.http.get<IEmployeeStatus[]>(`${this.employeeApi}/status`);
    }

    getI9Documents():Observable<Array<any>>{
        return this.http.get<any[]>(`api/employee-onboarding/i9/documents`);
    }

    //
    getNextEmployeeNumber(clientId) {
        const url = `${this.clientApi}/${clientId}/next-employee-number`;
        return this.http.get<any>(url);
    }

    getClientOnboardingWorkflows(clientId: number): Observable<any>{
        const url = `${this.onboardingAdminApi}/client/${clientId}/workflow`;
        return this.http.get<any[]>(url);
    }

    getEmployeeWorkflow(employeeId) {
        const url = `${this.onboardingAdminApi}/emp-workflow/employees/${employeeId}`;
        return this.http.get<any>(url);
    }

    getEmployeePayDetails(employeeId) {
        const url = `${this.employeeApi}/${employeeId}/employee-pay-detail`;
        return this.http.get<any>(url);
    }

    addEmployeeWorkflow(employeeWorkflows: any[], hireDate: string): Observable<any> {
        const url = `${this.onboardingAdminApi}/employee-workflow/hire-date/${hireDate}/add`;
        return this.http.post<any>(url, employeeWorkflows);
    }

    // getEmployeeStatusList() {
    //     const url = `${this.employeeApi}/status`;
    //     return this.http.get<any[]>(url);
    // }

    getClientJobTitleList(clientId) {
        const url = `${this.clientApi}/${clientId}/job-titles`;
        return this.http.get<any[]>(url);
    }

    getClientDivisionsList(clientId) {
        const url = `${this.clientApi}/${clientId}/client-divisions`;
        return this.http.get<any[]>(url);
    }

    getClientDepartmentsList(divisionId) {
        const url = `${this.clientApi}/divisions/${divisionId}/client-departments`;
        return this.http.get<any[]>(url);
    }

    getClientCostCentersList(clientId) {
        const url = `${this.clientApi}/${clientId}/all-cost-centers`;
        return this.http.get<any[]>(url);
    }

    getClientGroupsList(clientId) {
        const url = `${this.clientApi}/${clientId}/client-groups`;
        return this.http.get<any[]>(url);
    }

    getClientShiftsList(clientId) {
        const url = `${this.clientApi}/${clientId}/client-shifts`;
        return this.http.get<any[]>(url);
    }

    getClientWorkersCompList(clientId) {
        const url = `${this.clientApi}/${clientId}/workers-comps`;
        return this.http.get<any[]>(url);
    }

    getClientEEOCLocationList(clientId) {
        const url = `${this.clientApi}/${clientId}/eeoc-job-locations`;
        return this.http.get<any[]>(url);
    }

    getEEOCJobCategoriesList() {
        const url = `${this.eeocApi}/eeoc-job-categories`;
        return this.http.get<any[]>(url);
    }

    getDirectSupervisorsForClient(clientId) {
        const url = `${this.jobProfileApi}/clients/${clientId}/direct-supervisors`;
        return this.http.get<any[]>(url);
    }

    getPayFrequencyList() {
        const url = `${this.payrollApi}/pay-frequencies`;
        return this.http.get<any[]>(url);
    }

    getClientSutaStateList(clientId) {
        const url = `${this.clientApi}/${clientId}/suta-states`;
        return this.http.get<any[]>(url);
    }

    getClientRateList(clientId) {
        const url = `${this.clientApi}/${clientId}/client-rates`;
        return this.http.get<any[]>(url);
    }

    getBenefitsDataList(clientId) {
        const url = `${this.jobProfileApi}/clients/${clientId}/benefits-data`;
        return this.http.get<any[]>(url);
    }

    getTimePolicyList(clientId) {
        const url = `${this.laborApi}/clients/${clientId}/time-policies`;
        return this.http.get<any[]>(url);
    }

    getJobProfileData(jobProfileId, clientId) {
        const url = `${this.jobProfileApi}/${jobProfileId}/clients/${clientId}`;
        return this.http.get<any>(url);
    }

    getEmployeeClientRateList(employeeId) {
        const url = `${this.employeeApi}/${employeeId}/employee-client-rates`;
        return this.http.get<any[]>(url);
    }

    getEmailTemplates(clientId) {
        const url = `${this.onboardingAdminApi}/company-correspondences/clients/${clientId}`;
        return this.http.get<any[]>(url);
    }

    isUsernameAvailable(userId, requestedUsername): Observable<any> {
        var dto = {
            userId: userId,
            requestedUserName: requestedUsername,
        };
      
        const url = `${this.accountsApi}/username/availability`;
        return this.http.post<any>(url , dto);
    }

    saveNewEmployee(dto: any): Observable<any> {
        const url = `${this.employeeApi}/new`;
        return this.http.post<any>(url, dto);
    }

    updateEmployeeEmail(dto: any): Observable<any> {
        const url = `${this.employeeApi}/update-employee-email`;
        return this.http.put<any>(url, dto);
    }

    sendEmployeeOnboardingEmail(dto: any): Observable<any> {
        const url = `${this.onboardingAdminApi}/send-employee-onboarding-email`;
        return this.http.put<any>(url, dto);
    }

    updateInvitationEmailSent(employeeId: number) {
        const url = `${this.employeeApi}/${employeeId}/onboarding-invitation-sent`;
        return this.http.put<any>(url, null);
    }
}