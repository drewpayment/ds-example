import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { throwError, Observable, BehaviorSubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import * as saveAs from 'file-saver';
import { UserType } from '@ds/core/shared';
import { IEmployeeContactInfo } from '@ds/employees/profile/shared/employee-contact-info.model';
import { IFormStatusData } from '@ajs/onboarding/shared/models';
import { IEmployeeAccrualInfo, IEmployeeBenefitSettings, IBenefitPackage, IClientEmploymentClass, ICustomBenefitField, IBenefitDependent, ICustomBenefitFieldValue } from '../models/employee-accruals/employee-accruals.model';
import { Moment } from 'moment';
import { IEmployeeDependent } from '@ds/employees/profile/shared/employee-dependent.model';
import { IContact } from '@ds/core/contacts';

@Injectable({
    providedIn: 'root'
})
export class EmployeeAccrualsService {
    static readonly EMPLOYEE_API_BASE = "api/employees";
    static readonly BENEFITS_API_BASE = "api/benefits";

    constructor(private httpClient: HttpClient) {
    }

    mapCustomValuesToDependents = (emp:IContact,dependents:IEmployeeDependent[],c:ICustomBenefitField[], values:ICustomBenefitFieldValue[]) => {
        let result:IBenefitDependent[] = [];

        let empData:IBenefitDependent = { dependentId:-1, firstName:emp.firstName,lastName:emp.lastName,birthDate:emp.birthDate, customFields:[] };
        c.filter(x=>x.isEmployeeField).forEach(x => {
            let v = values.find(y => y.customFieldId == x.customFieldId && y.employeeId > 0 && !y.dependentId);
            if(!v) v = {
                customFieldId: x.customFieldId,
                clientId: x.clientId,
                dependentId: 0,
                employeeId: emp.employeeId,
                name: x.name,
                textValue: '',
                customFieldValueId: 0,
              };
              
            v.name = x.name;
            empData.customFields.push( v );
        });
        result.push( empData );

        dependents.forEach(de => {
            let depData:IBenefitDependent = { dependentId:de.employeeDependentId, firstName: de.firstName, lastName: de.lastName,  birthDate: de.birthDate,
                customFields:[] };
            c.forEach(x => {
                let v = values.find(y => y.customFieldId == x.customFieldId && y.employeeId > 0 && y.dependentId == de.employeeDependentId);
                if(!v) v = {
                    customFieldId: x.customFieldId,
                    clientId: x.clientId,
                    dependentId: de.employeeDependentId,
                    employeeId: emp.employeeId,
                    name: x.name,
                    textValue: '',
                    customFieldValueId: 0,
                  };

                v.name = x.name;
                depData.customFields.push( v );
            });
            result.push( depData );
        });

        return result;
    }

    getEmployeeProfileCard(employeeId: number): Observable<IContact> {
        return this.httpClient
          .get<IContact>(`${EmployeeAccrualsService.EMPLOYEE_API_BASE}/${employeeId}/profile-card`);
    }

    getEmployeeAccrualList(clientId, employeeId):Observable<IEmployeeAccrualInfo[]> {
        return this.httpClient.get<IEmployeeAccrualInfo[]>(
            `${EmployeeAccrualsService.EMPLOYEE_API_BASE}/${employeeId}/clients/${clientId}/employee-accrual-list`);
    }

    getEmployeeDependents( employeeId):Observable<IEmployeeDependent[]> {
        return this.httpClient.get<IEmployeeDependent[]>(
            `${EmployeeAccrualsService.EMPLOYEE_API_BASE}/${employeeId}/dependents`);
    }

    getEmployeeBenefitSettings(clientId, employeeId):Observable<IEmployeeBenefitSettings> {
        return this.httpClient.get<IEmployeeBenefitSettings>(
            `${EmployeeAccrualsService.EMPLOYEE_API_BASE}/${employeeId}/benefit-settings`);
    }

    getEmployeeClientCustomFields(clientId, employeeId):Observable<ICustomBenefitFieldValue[]> {
        return this.httpClient.get<ICustomBenefitFieldValue[]>(
            `${EmployeeAccrualsService.BENEFITS_API_BASE}/clients/${clientId}/employees/${employeeId}/custom-fields`);
    }

    saveEmployeeClientCustomFields(clientId, dtos : ICustomBenefitFieldValue[] ):Observable<ICustomBenefitFieldValue[]> {
        return this.httpClient.post<ICustomBenefitFieldValue[]>(
            `${EmployeeAccrualsService.BENEFITS_API_BASE}/clients/${clientId}/custom-field-values`, dtos);
    }

    saveEmployeeAccrual(accr: IEmployeeAccrualInfo,employeeId:number, reminderActive: boolean, reminderDate: Moment){
        return this.httpClient.post<boolean>(
            `${EmployeeAccrualsService.EMPLOYEE_API_BASE}/${employeeId}/employee-accrual/reminder-active/${reminderActive.toString()}/${reminderDate.format("YYYY-MM-DD")}`,accr);
    }

    saveEmployeeBenefitSettings(setting:IEmployeeBenefitSettings,employeeId:number, reminderActive: boolean, reminderDate: Moment):Observable<IEmployeeBenefitSettings> {
        return this.httpClient.post<IEmployeeBenefitSettings>(
            `${EmployeeAccrualsService.EMPLOYEE_API_BASE}/${employeeId}/benefit-settings/reminder-active/${reminderActive.toString()}/${reminderDate.format("YYYY-MM-DD")}`, setting);
    }
}