import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IBankHoliday } from '@models';

@Injectable({
    providedIn: 'root'
})
export class MaintenanceApiService {

    constructor(private http: HttpClient) { }

    // saveClientNotificationPreferences(clientId: number, dtos: INotificationPreferencesProductGroups[]) {
    //     return this.http.post<INotificationPreferencesProductGroups[]>(`api/notifications/clients/${clientId}/preferences`, dtos);
    // }

    getBankHolidays(year: number) {
        let params = new HttpParams().set("year", year.toString());
        return this.http.get<IBankHoliday[]>(`api/banks/bank-holidays`, { params: params });
    }

    deleteBankHoliday(bankHolidayId: number) {
        let params = new HttpParams().set("bankHolidayId", bankHolidayId.toString());
        return this.http.delete<IBankHoliday[]>(`api/banks/bank-holidays/delete`, { params: params });
    }

    saveBankHoliday(dto: IBankHoliday) {
        return this.http.post<IBankHoliday>(`api/banks/bank-holidays`, dto);
    }

    copyBankHolidays(year: number) {
        let params = new HttpParams().set("year", year.toString());
        //return this.http.post<IBankHoliday[]>(`api/banks/bank-holidays/copy`, year);
        return this.http.post<IBankHoliday[]>(`api/banks/bank-holidays/copy/${year}`,{});
    }


}
