import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IEmployeeTaxInfo, IEmployeeTaxDetails } from '../../models/tax.model';
import { IFilingStatusWithDisplayOrder } from '@ds/employees/taxes/shared/filing-status.model';
import { FilingStatus } from '@ds/employees/taxes/shared/filing-status';
import { IFilingStatus } from '@ds/employees/taxes/shared/filing-status.model.ts';
import { IEmployeeTax } from '@ds/employees/taxes/shared/employee-tax.model';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { IFormStatusData } from '@ajs/onboarding/shared/models';


@Injectable({
    providedIn: 'root'
})
export class TaxApiService {

    constructor(
        private http: HttpClient
    ) { }

    static readonly EMPLOYEE_API_BASE = 'api/employee';
    static readonly TAXES_API_BASE = 'api/taxes';

    getEmployeeTaxInfo(employeeId: number) {
        return this.http.get<IEmployeeTaxInfo[]>(`${TaxApiService.EMPLOYEE_API_BASE}/${employeeId}/active-taxes`);
    }

    getEmployeeTaxDetails(employeeId: number, taxId: number) {
        return this.http.get<IEmployeeTaxDetails>(`${TaxApiService.EMPLOYEE_API_BASE}/${employeeId}/employee-tax-id/${taxId}`);
    }

    insertEmployeeTaxRequestChange(dto: IEmployeeTaxDetails) {
        return this.http.post<IEmployeeTaxDetails>(`${TaxApiService.EMPLOYEE_API_BASE}/
        ${dto.employeeId}/request-change/${dto.employeeTaxId}`, dto);
    }
    getFilingStatuses(taxId: number) {
        return this.http.get<IFilingStatusWithDisplayOrder[]>(`${TaxApiService.TAXES_API_BASE}/${taxId}/filing-statuses`);
    }

    get2020FederalFilingStatus(selectedFilingStatus: FilingStatus, box2Checked: boolean) {
        const url = `${TaxApiService.TAXES_API_BASE}/filing-status/2020/`
                  + `selected-status/${selectedFilingStatus}/box2-checked/${box2Checked}`;
        return this.http.get<IFilingStatus>(url);
    }

    getDisplayed2020FederalFilingStatus(trueFilingStatus: FilingStatus) {
        const url = `${TaxApiService.TAXES_API_BASE}/filing-status/2020/true-status/${trueFilingStatus}`;
        return this.http.get<IFilingStatus>(url);
    }

    postEmployeeFormUpdatesWithoutFinalize(employeeId: number, forms: IFormStatusData[]) {
        const url = `api/onboarding/employees/${employeeId}/forms/withoutOnboardingFinalize`;
        return this.http.post(url, forms)
        .pipe(
            catchError((error) => {
                return throwError(error);
            })
        );
    }

    updateEmployeeOnboardingW4AndTaxWithNotification(model: IEmployeeTax) {
        console.log("Model: ", model);
        const url = `api/employeeOnboarding/w4/sendNotification`;
        return this.http.put<IEmployeeTax>(url, model)
        .pipe(
            catchError((error) => {
                return throwError(error);
            })
        );
    }

}
