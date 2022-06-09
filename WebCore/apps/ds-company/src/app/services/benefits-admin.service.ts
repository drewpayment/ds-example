import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { throwError, Observable, BehaviorSubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import * as saveAs from 'file-saver';
import { UserType } from '@ds/core/shared';
import { IEmployeeContactInfo } from '@ds/employees/profile/shared/employee-contact-info.model';
import { IFormStatusData } from '@ajs/onboarding/shared/models';
import { IEmployeeAccrualInfo, IClientBenefitSetting, IBenefitPackage, IClientEmploymentClass, ICustomBenefitField, IBenefitDependent, ICustomBenefitFieldValue } from '../models/employee-accruals/employee-accruals.model';
import { Moment } from 'moment';
import { IEmployeeDependent } from '@ds/employees/profile/shared/employee-dependent.model';
import { IContact } from '@ds/core/contacts';

@Injectable({
    providedIn: 'root'
})
export class BenefitsAdminService {
    static readonly BENEFITS_API_BASE = "api/benefits";

    private activeAccrual$ = new BehaviorSubject<IEmployeeAccrualInfo>(null);
    activeAccrual: Observable<IEmployeeAccrualInfo> = this.activeAccrual$.asObservable();

    private activeDependent$ = new BehaviorSubject<IBenefitDependent>(null);
    activeDependent: Observable<IBenefitDependent> = this.activeDependent$.asObservable();

    constructor(private httpClient: HttpClient) {
    }

    getClientBenefitSetup(clientId):Observable<IClientBenefitSetting> {
        return this.httpClient.get<IClientBenefitSetting>(
            `${BenefitsAdminService.BENEFITS_API_BASE}/clients/${clientId}/client-benefit-setup`);
    }
    
    getBenefitPackages(clientId):Observable<IBenefitPackage[]> {
        return this.httpClient.get<IBenefitPackage[]>(
            `${BenefitsAdminService.BENEFITS_API_BASE}/clients/${clientId}/benefit-packages`);
    }

    getClientEmploymentClasses(clientId):Observable<IClientEmploymentClass[]> {
        return this.httpClient.get<IClientEmploymentClass[]>(
            `${BenefitsAdminService.BENEFITS_API_BASE}/clients/${clientId}/employment-classes`);
    }
    saveClientEmploymentClass(clientId, dto: IClientEmploymentClass):Observable<IClientEmploymentClass> {
        return this.httpClient.post<IClientEmploymentClass>(
            `${BenefitsAdminService.BENEFITS_API_BASE}/clients/${clientId}/employment-class`, dto);
    }
    getClientCustomFields(clientId):Observable<ICustomBenefitField[]> {
        return this.httpClient.get<ICustomBenefitField[]>(
            `${BenefitsAdminService.BENEFITS_API_BASE}/clients/${clientId}/custom-fields/false`);
    }
}