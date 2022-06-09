import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import { IAdvanceEnrollmentReportDialogData, IEmployeeData, IPlanData, 
    IProviderData, IAdvanceEnrollmentReportConfigData } from './advance-enrollment-report.model';
import { saveAs } from 'file-saver';

@Injectable({
    providedIn: 'root'
})
export class AdvanceEnrollmentReportService {

    static readonly PLANS_API_BASE = 'api/benefits/plans';
    static readonly CLIENTS_API_BASE = 'api/client';
    static readonly BENEFITS_API_BASE = 'api/benefits';
    constructor(private http: HttpClient) {

    }

    getEmployeeList(clientId: number) {
        return this.http.get<IEmployeeData[]>(`${AdvanceEnrollmentReportService.CLIENTS_API_BASE}/${clientId}/employees`);
    }

    getPlanList(clientId: number) {
        return this.http.get<IPlanData[]>(`${AdvanceEnrollmentReportService.PLANS_API_BASE}/${clientId}/list`);
    }

    getPlanProviderList(clientId: number) {
        return this.http.get<IProviderData[]>(`${AdvanceEnrollmentReportService.PLANS_API_BASE}/providers/${clientId}/list`);
    }

    generateReport(input: IAdvanceEnrollmentReportConfigData) {
        let url = `${AdvanceEnrollmentReportService.BENEFITS_API_BASE}/advance-enrollment-report`;
        this.http.post(url, input, { 
            responseType: 'blob',
            observe: 'response'
        })
        .subscribe((response) => {
            const fileName = (response.headers.get('Content-Disposition').split(';')[1].trim().split('=')[1]).replace(/"/g, '');

            const blob = response.body;
            saveAs(blob, fileName);
        });
    }
    
}
