import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IClientAccrual, LastPayrollDates, EmployeeLeaveManagementInfo, TimeOffRequest, TimeOffRequestResult } from './models';
import { Moment } from 'moment';
import { TimeOffPolicy } from 'apps/ds-ess/ajs/leave/time-off/shared/time-off-policy.model';
import { TimeOffPolicySummary } from 'apps/ds-mobile/src/models';

@Injectable({
    providedIn: 'root'
})
export class LeaveManagementService {

    constructor(private http: HttpClient) {}

    getActiveTimeOffEvents(employeeId: number): Observable<TimeOffPolicy[]> {
        const url = `api/employee/${employeeId}/timeOff/active`;
        return this.http.get<TimeOffPolicy[]>(url);
    }

    getTimeOffPolicies(clientId: number, employeeId: number): Observable<TimeOffPolicySummary[]> {
        const url = `api/leave-management/clients/${clientId}/employees/${employeeId}/timeoff/policies`;
        return this.http.get<TimeOffPolicySummary[]>(url);
    }

    getEmployeeAccruals(clientId: number, employeeId: number): Observable<IClientAccrual[]> {
        const url = `api/leave-management/clients/${clientId}/employees/${employeeId}/accruals`;
        return this.http.get<IClientAccrual[]>(url);
    }

    getEmployeeLastPayrollDates(clientId: number, employeeId: number): Observable<LastPayrollDates> {
            const url = `api/leave-management/clients/${clientId}/employees/${employeeId}/last-payroll-dates`;
            return this.http.get<LastPayrollDates>(url);
        }

    getEmployeeLeaveManagementInfo(planId: number,
        clientId: number,
        userId: number,
        employeeId: number,
        lastPayDate: Moment
    ): Observable<EmployeeLeaveManagementInfo[]> {
        const url = `api/leave-management/clients/${clientId}/info`;
        const params = new HttpParams()
            .append('planId', `${planId}`)
            .append('userId', `${userId}`)
            .append('employeeId', `${employeeId}`);
            // .append('lastPayPeriod', `${lastPayDate.format('YYYY-MM-DD')}`);

        return this.http.get<EmployeeLeaveManagementInfo[]>(url, { params: params });
    }

    getEmployeeLeaveManagementDetail(clientId: number, employeeId: number, clientAccrualId: number): Observable<IClientAccrual> {
        const url = `api/leave-management/clients/${clientId}/employees/${employeeId}/accruals/${clientAccrualId}`;
        return this.http.get<IClientAccrual>(url);
    }

    saveRequestTimeOff(clientId: number, dto: TimeOffRequest): Observable<TimeOffRequest> {
        const url = `api/leave-management/clients/${clientId}/request`;
        return this.http.post<TimeOffRequest>(url, dto);
    }

    saveRequestTimeOffDetail(clientId: number, dto: TimeOffRequest): Observable<TimeOffRequestResult> {
        const url = `api/leave-management/clients/${clientId}/request-detail`;
        return this.http.post<TimeOffRequestResult>(url, dto);
    }

}
