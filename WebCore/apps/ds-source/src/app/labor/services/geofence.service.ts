import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Geofence } from '../../models/geofence.model';
import { IGeofenceOptIn, IClientAgreement } from '../../models';
import { Features } from '@ds/admin/client-statistics/shared/models/featureEnum';
import * as moment from 'moment';

@Injectable({
    providedIn: 'root'
})
export class GeofenceService {

    constructor(private http: HttpClient) { }

    private api = 'api/geofences';

    getClientGeofenceList(clientId: number): Observable<Geofence[]> {
        // let params = new HttpParams();
        // params = params.append('clientId', clientId.toString());
        // return this.http.get<Geofence[]>(`${this.api}/client/${clientId}`, { params: params });
        return this.http.get<Geofence[]>(`${this.api}/client/${clientId}/`);
    }

    getClientLastModified(clientId: number): Observable<moment.Moment> {
        // let params = new HttpParams();
        // params = params.append('clientId', clientId.toString());
        // return this.http.get<Geofence[]>(`${this.api}/client/${clientId}`, { params: params });
        return this.http.get<moment.Moment>(`${this.api}/client/${clientId}/lastmodified`);
    }

    addClientGeofence(dto: Geofence): Observable<Geofence> {
        return this.http.post<Geofence>(this.api, dto);
    }

    updateClientGeofence(dto: Geofence): Observable<Geofence> {
        const url = `${this.api}/${dto.clockClientGeofenceID}`;
        return this.http.put<Geofence>(url, dto);
    }

    clientGeofenceOptIn(optIn: IGeofenceOptIn) {
        const url = `${this.api}/set-client-geofence`;
        return this.http.post<any>(url, optIn);
    }

    getClientGeofenceOptIn(): Observable<any> {
        const url = `${this.api}/get-client-geofence`;
        return this.http.get<any>(url);
    }

    getClientAgreement(featureId: Features): Observable<IClientAgreement> {
        const url = `${this.api}/get-client-agreement/${featureId}`;
        return this.http.get<IClientAgreement>(url);
    }

    getPunchesByPunchIds(punchIds: number[], employeeId: number): Observable<any> {
        const url = `${this.api}/get-punches/${employeeId}`;
        return this.http.post<any>(url, punchIds);
    }

}

