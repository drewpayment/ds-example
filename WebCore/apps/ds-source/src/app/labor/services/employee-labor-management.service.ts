import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IClockClientTimePolicy } from '@ajs/labor/models/clock-client-time-policy-dtos.model';

@Injectable({
    providedIn: 'root'
})
export class EmployeeLaborManagementService {

    constructor(private http: HttpClient) { }

    private api = 'api/EmployeeLaborManagement';
    
    getClientTimePolicies(clientId: number): Observable<IClockClientTimePolicy[]> {
        const url  = `${this.api}/time-policies/${clientId}`;
        return this.http.get<IClockClientTimePolicy[]>(url);
    }

    updateTimePoliciesGeofence(timePolicies: IClockClientTimePolicy[]): Observable<IClockClientTimePolicy[]> {
        const url = `${this.api}/time-policies/update-geofence`;
        return this.http.post<IClockClientTimePolicy[]>(url, timePolicies);
    }

}
