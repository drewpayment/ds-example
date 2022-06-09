import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { INotificationPreferencesProductGroups } from './models/notification-preferences-product-groups.model';
import { IContactPreferenceInfo } from './models/contact-preference-info.model';

@Injectable({
    providedIn: 'root'
})
export class NotificationPreferenceApiService {

    constructor(private http: HttpClient) { }

    getClientNotificationPreferences(clientId:number) {
        return this.http.get<INotificationPreferencesProductGroups[]>(`api/notifications/clients/${clientId}/preferences`);
    }

    saveClientNotificationPreferences(clientId: number, dtos: INotificationPreferencesProductGroups[]) {
        return this.http.post<INotificationPreferencesProductGroups[]>(`api/notifications/clients/${clientId}/preferences`, dtos);
    }

    getContactNotificationPreferences(clientId: number, userId?: number, employeeId?: number) {
        let params = new HttpParams().set("clientId", clientId.toString());

        if(userId)
            params = params.set("userId", userId.toString());

        if(employeeId)
            params = params.set("employeeId", employeeId.toString());

        return this.http.get<IContactPreferenceInfo>(`api/notifications/contacts/preferences`, { params: params });
    }

    saveContactNotificationPreferences(dto: IContactPreferenceInfo) {
        return this.http.post<IContactPreferenceInfo>(`api/notifications/contacts/preferences`, dto);
    }
}
