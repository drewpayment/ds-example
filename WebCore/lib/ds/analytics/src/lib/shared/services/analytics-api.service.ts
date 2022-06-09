import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { EmployeePointTotal } from '../models/EmployeePointTotal.model';
import { Observable } from 'rxjs';
import { map, switchMap, tap } from "rxjs/operators";
import { IPayrollPayCheckList } from '@ds/payroll';
import { EarningsBreakdown } from '../models/EarningsBreakdown.model';
import { UserPerformanceDashboardEmployee, UserPerformanceDashboardResult } from '@ds/analytics/models';
import { TerminationData } from '../models/TerminationData.model';

@Injectable({
    providedIn: 'root'
})
export class AnalyticsApiService {

    constructor(private http: HttpClient) { }

    GetTerminationInformation(clientId, startDate, endDate, employeeIds, userId, userTypeId) {
        return this.http.post(`api/ApplicantTracking/getTerminationInformation`,
            { clientId, startDate, endDate, employeeIds, userId, userTypeId });
    }

    GetActiveEmployees(clientId, startDate, endDate, employeeIds, userId, userTypeId) {
        return this.http.post(`api/employee/getActiveEmployees`, { clientId, startDate, endDate, employeeIds, userId, userTypeId });
    }

    GetPunchTypes(clientId, startDate, endDate, employeeIds) {
        return this.http.post(`api/employee/employeepunches`, { clientId, startDate, endDate, employeeIds });
    }

    GetEmployeeDemographicInformation(clientId, startDate, endDate, employeeIds) {
        return this.http.post(`api/employee/GetEmployeeDemographicInformation`, { clientId, startDate, endDate, employeeIds });
    }

    GetHistoryInformation(clientId) {
        return this.http.get(`api/ApplicantTracking/getHistoryInformation/clientId/${clientId}`);
    }

    GetTimeCardApproval(clientId, currentPayrollId, startDate, endDate, userId) {
        return this.http.post(`api/clients/getTimeCardApprovals`, { clientId, currentPayrollId, startDate, endDate, userId });
    }

    GetSupervisors(clientId) {
        return this.http.get(`api/time-card-authorization/get-dropdown-data-dynamic/filterCategory/6/clientId/${clientId}`);
    }

    GetOvertimeSetup(clientId, startDate, endDate, employeeIds) {
        return this.http.post(`api/clock/overtime-policy-by-employeeIds`, { clientId, startDate, endDate, employeeIds });
    }

    GetClockExceptionsByRange(clientId, startDate, endDate, employeeIds) {
        return this.http.post(`api/labor-management/clients/standard-exception-details-by-range`,
            { clientId, startDate, endDate, employeeIds });
    }

    GetEmployeePointTotals(clientId, startDate, endDate, employeeIds) {
        return this.http.post<EmployeePointTotal[]>(`api/employee/GetEmployeePoints`, { clientId, startDate, endDate, employeeIds });
    }

    GetClockedInEmployees(clientId, employeeIds) {
        return this.http.post(`api/labor-management/clients/ClockedInEmployee`, { clientId, employeeIds });
    }

    GetGetClockEmployeeHoursComparison(clientId, startDate, endDate, employeeIds) {
        return this.http.post(`api/labor-management/clients/employee-scheduled-vs-worked-hours`,
            { clientId, startDate, endDate, employeeIds });
    }

    GetPayrollHistory(startDate, endDate, employeeIds) {
        return this.http.post(`api/payroll/payrollHistory`, { startDate, endDate, employeeIds });
    }

    GetEarningBreakdownByYear(yearId) {
        return this.http.get(`api/payroll/earningBreakdown/${yearId}`);
    }

    GetPTOEvents(clientId, startDate, endDate, employeeIds, userId, userTypeId) {
        return this.http.post(`api/pto`, { clientId, startDate, endDate, employeeIds, userId, userTypeId });
    }

    getClientCalendarTaxFileID(clientId: number) {
        return this.http.post<number>(`api/payroll/clientCalendarTaxFileID`, { clientId });
    }

    GetEarningBreakdownByDateRange(clientId, startDate, endDate, employeeIds): Observable<EarningsBreakdown[]> {
        return this.http.post<EarningsBreakdown[]>(`api/payroll/earningBreakdownInDateRange`, { clientId, startDate, endDate, employeeIds });
    }

    GetQuarterStartDates() {
        return this.http.get('api/payroll/fiscalYearQuarters');
    }

    getPayrollPaycheckHistory(payrollId: number): Observable<IPayrollPayCheckList[]> {
        const url = `api/payroll/get/payrolls/${payrollId}/paycheckListByPayrollID`;
        let params = new HttpParams();
        params = params.append('payrollId', payrollId.toString());
        return this.http.get<IPayrollPayCheckList[]>(url, { params: params });
    }

    getPayrollTaxHistory(payrollId: number): Observable<IPayrollPayCheckList[]> {
        const url = `api/payroll/get/payrolls/${payrollId}/paycheckTaxTotals`;
        let params = new HttpParams();
        params = params.append('payrollId', payrollId.toString());
        return this.http.get<IPayrollPayCheckList[]>(url, { params: params });
    }

    getYTDQTDMTDTaxTotals(payrollId: number): Observable<IPayrollPayCheckList[]> {
        const url = `api/payroll/get/payrolls/${payrollId}/YTDQTDMTDTaxTotals`;
        let params = new HttpParams();
        params = params.append('payrollId', payrollId.toString());
        return this.http.get<IPayrollPayCheckList[]>(url, { params: params });
    }

    getPaycheckYTDQTDMTDTotals(payrollId: number): Observable<IPayrollPayCheckList[]> {
        const url = `api/payroll/get/payrolls/${payrollId}/paycheckYTDQTDMTDTotals`;
        let params = new HttpParams();
        params = params.append('payrollId', payrollId.toString());
        return this.http.get<IPayrollPayCheckList[]>(url, { params: params });
    }
    getAllSupers(clientId, startDate, endDate, employeeIds) {
        return this.http.post('api/employees/supervisorForDashboard', { clientId, startDate, endDate, employeeIds });
    }

    getDepartments(clientId) {
        return this.http.get(`api/client/${clientId}/client-departments`);
    }

    getEmployeeSchedule(clientId, employeeIds, startDate, endDate) {
        return this.http.post(`api/Clock/employees/get-employee-schedule`, { clientId, employeeIds, startDate, endDate });
    }

    GetTurnoverRate(clientId, startDate, endDate, employeeIds, userId, userTypeId) {
        return this.http.post(`api/ApplicantTracking/GetGrowthRate`, { clientId, startDate, endDate, employeeIds, userId, userTypeId });
    }

    GetTerminatedEmployees(clientId, startDate, endDate, employeeIds, userId, userTypeId) {
        return this.http.post(`api/ApplicantTracking/GetTerminatedEmployees`,
            { clientId, startDate, endDate, employeeIds, userId, userTypeId });
    }

    GetNewHiredEmployeesDetailFn(clientId, startDate, endDate, employeeIds, userId, userTypeId) {
        return this.http.post(`api/ApplicantTracking/GetNewHiredEmployeesDetailFn`,
            { clientId, startDate, endDate, employeeIds, userId, userTypeId });
    }

    GetRetentionRate(clientId, startDate, endDate, employeeIds, userId, userTypeId) {
        return this.http.post(`api/ApplicantTracking/GetGrowthRate`, { clientId, startDate, endDate, employeeIds, userId, userTypeId });
    }

    getEmployeeInfo(clientId: number, startDate: string, endDate: string, employeeIds: number[]): Observable<UserPerformanceDashboardEmployee[]> {
        return this.http.post<UserPerformanceDashboardEmployee[]>('api/employees/employeeInfo', {
            clientId, startDate, endDate, employeeIds
        });
    }

    getUserInfo(clientId: number, startDate: string, endDate: string, employeeIds: number[]): Observable<UserPerformanceDashboardEmployee[]> {
        return this.http.post<UserPerformanceDashboardEmployee[]>('api/employees/dashboardUserInfo', { clientId, startDate, endDate, employeeIds });
    }

    getUserPerformanceDashboard(clientId: number, startDate: string, endDate: string, employeeIds: number[]): Observable<UserPerformanceDashboardResult> {
        return this.http.post<UserPerformanceDashboardResult>('api/dashboard/user-performance', { clientId, startDate, endDate, employeeIds });
    }

    getUserTypeInfo(clientId, startDate, endDate, employeeIds) {
        return this.http.post('api/employees/userTypes', { clientId, startDate, endDate, employeeIds });
    }

    GetGrowthRate(clientId, startDate, endDate, employeeIds, userId, userTypeId) {
        return this.http.post(`api/ApplicantTracking/GetGrowthRate`, { clientId, startDate, endDate, employeeIds, userId, userTypeId });
    }
}

