import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { throwError, Observable, BehaviorSubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import * as saveAs from 'file-saver';
import { UserType } from '@ds/core/shared';
import { IEmployeeContactInfo } from '@ds/employees/profile/shared/employee-contact-info.model';
import { IFormStatusData } from '@ajs/onboarding/shared/models';
import { IEmergencyContact } from '../models/contact/emergency-contact.model';

@Injectable({
    providedIn: 'root'
})
export class EmergencyContactService {
    static readonly EMERGENCY_CONTACTS_API_BASE = "api/employee/emergency-contacts";
    static readonly EMPLOYEE_API_BASE = "api/employee";

    private activeContact$ = new BehaviorSubject<IEmergencyContact>(null);
    activeContact: Observable<IEmergencyContact> = this.activeContact$.asObservable();

    constructor(private httpClient: HttpClient) {
    }
    getEmployeeEmergencyContacts(employeeId):Observable<IEmergencyContact[]> {
        return this.httpClient.get<IEmergencyContact[]>(
            `${EmergencyContactService.EMPLOYEE_API_BASE}/${employeeId}/emergency-contacts`);
    }

    setActiveContact(val: IEmergencyContact) {
        this.activeContact$.next(val);
    }

    putEmergencyContact(employeeId, data: IEmergencyContact) {
        return this.httpClient.post<IEmergencyContact>(
            `${EmergencyContactService.EMPLOYEE_API_BASE}/${employeeId}/emergency-contacts/update`, data);
    }
    
    deleteActiveContact(data: IEmergencyContact) {
        return this.httpClient.delete<IEmergencyContact>(
            `${EmergencyContactService.EMPLOYEE_API_BASE}/${data.employeeId}/emergency-contacts/${data.employeeEmergencyContactId}`);
    }
}