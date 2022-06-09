import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { IEmployeeBasicStatusData } from '@ajs/employee/hiring/shared/models';
import { IEEOCJobCategoryData } from '@ajs/job-profiles/shared/models/eeoc-job-category-data.interface';
import { IClientPayroll, IClientPay } from '@ajs/job-profiles/shared/models/client-payroll.interface';
import { IEEOCRaceEthnicCategory } from './eeoc-race-ethnic-categories.model';
import { IEEOCEmployeeInfo } from './employee-eeoc.model';
import { IEEOCLocationDataPerMultiClient } from '@ajs/job-profiles/shared/models/eeoc-location-data-per-client.interface';
import { IClientRelationData } from '@ajs/onboarding/shared/models';
import { IEEOCOrganizationData } from '@ajs/job-profiles/shared/models/eeoc-organization-data.interface';
import { map } from 'rxjs/operators';
import { ProfileImageLoaderPipe } from '@ds/core/contacts';
import { saveAs } from "file-saver";

@Injectable({
    providedIn: 'root'
})
export class EmployeeEEOCApiService {
    static SERVICE_NAME = 'EmployeeEEOCApiService';

    private api = 'api/eeoc';

    constructor(
        private http: HttpClient,
        @Inject('window') public window: Window
    ) { }

    getEeocW2YearList(clientIds: number[]) {
        const url = `${this.api}/eeocW2Years`;
        return this.http.post<number[]>(url, clientIds);
    }

    getEeocJobCategoriesList() {
        return this.http.get<IEEOCJobCategoryData[]>(`${this.api}/eeocJobCategories`);
    }

    getClientRelatedClientIds(isActive: boolean) {
        return this.http.get<IClientRelationData>(`api/clients/client/relations/clients`);
    }

    getClientPayrollsByYear(year: number) {
        return this.http.get<IClientPayroll[]>(`${this.api}/getClientPayrollsByYear/${year}`);
    }

    getEeocRaceEthnicCategoriesList() {
        return this.http.get<IEEOCRaceEthnicCategory[]>(`${this.api}/eeocRaceEthnicCategories`);
    }

    getEeocLocationsListMultipleClients(clientIds: number[], returnUnique: boolean, returnActive: boolean) {
        const url = `${this.api}/eeocLocationsMultiClient/${returnUnique}/${returnActive}`;
        return this.http.post<IEEOCLocationDataPerMultiClient[]>(url, clientIds);
    }

    getEeocEmployeesToValidate(payrollIds: number[]) {
        const url = `${this.api}/eeocEmployeesToValidate`;
        return this.http.post<IEEOCEmployeeInfo[]>(url, payrollIds).pipe(
            map(response => {
                response.forEach(employee => {
                    ProfileImageLoaderPipe.EEOCEmployeeProfileImageLoader(employee);
                });

                return response;
            }),
        );
    }

    saveEmployeeStatus(dtos: IEmployeeBasicStatusData) {
        const url = `${this.api}/saveEmployeeStatus`;
        return this.http.post<IEmployeeBasicStatusData>(url, dtos);
    }

    getEeocOrganizationInfo(FEIN: number) {
        const url = `${this.api}/eeocOrganizationInfo/${FEIN}`;
        return this.http.get<IEEOCOrganizationData>(url);
    }

    saveEeocOrganizationInfo(organizationInfo: IEEOCOrganizationData) {
        const url = `${this.api}/eeocOrganizationInfo/save`;
        return this.http.post<IEEOCOrganizationData>(url, organizationInfo);
    }

    saveEEOCLocation(dtos: IEEOCLocationDataPerMultiClient) {
        const url = `${this.api}/eeocLocation/save`;
        return this.http.post<IEEOCLocationDataPerMultiClient>(url, dtos);
    }

    saveEmployeesEeocInfo(employeeRows: IEEOCEmployeeInfo[]) {
        const url = `${this.api}/eeocEmployees/save`;
        return this.http.post<IEEOCEmployeeInfo>(url, employeeRows);
    }

    saveEmployeeEeocInfo(employeeRow: IEEOCEmployeeInfo) {
        const url = `${this.api}/eeocEmployee/save`;
        return this.http.post<IEEOCEmployeeInfo>(url, employeeRow);
    }

    getEeocCSV(dtos: IClientPay[], fein) {
        const url = `${this.api}/GetComp1CsvByClientPayrolls/${fein}`;
        return this.http.post<any>(url, dtos, {responseType: 'blob' as any, observe: 'response'})
        .pipe(
            map((response: HttpResponse<Blob>) => {
                saveAs(response.body, `EEO1_Comp1_Export.csv`);
                return response.body;
            })
        );
    }

    getEeeocPDF(dtos: IClientPay[]) {
        const url = `${this.api}/GetComp1ReportByClientPayrolls`;
        return this.http.post<any>(url, dtos, {responseType: 'blob' as any, observe: 'response'})
        .pipe(
            map((response: HttpResponse<Blob>) => {
                var reportUrl = URL.createObjectURL(response.body);
                window.open(reportUrl, '_blank')
                return response.body;
            })
        );
    }

    disableHeadquartersForLocationIds(locationIds: number[]) {
        const url = `${this.api}/location/disableheadquartersforlocationids`;
        return this.http.post<number[]>(url, locationIds);
    }

    updateLocationHeadquarters(clientIds: number[], eeocLocationDesc: string) {
        const url = `${this.api}/location/updateHeadquarters/${eeocLocationDesc}`;
        return this.http.post(url, clientIds);
    }
}
