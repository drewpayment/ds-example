import { Injectable } from '@angular/core';
import { HttpParams, HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class EmergencyContactsApiService {

    constructor(
        private http: HttpClient
    ) { }

    getEmergencyContactsByEmployeeId(employeeId: number) {
        return this.http.get<any[]>(`api/employee/${employeeId}/emergency-contacts`);
    }
}
