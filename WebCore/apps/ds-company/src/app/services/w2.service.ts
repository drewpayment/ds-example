import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { IClientW2ProcessingNotes } from '../models/w2/client-w2-processing-notes';
import { IW2Client } from '../models/w2/w2-client';
import { ICreatedClient } from '../models/w2/w2-created-client';
import { IW2ProcessingSubmitted } from '../models/w2/w2-processing-submitted';

@Injectable({
    providedIn: 'root'
})

export class W2Service {
    private w2 = `api/w2`;

    constructor(private http: HttpClient){}

    getClientsToProcess(year: number, typeid: number): Observable<IW2Client[]>{
        const url = `${this.w2}/getw2clients?year=${year}&typeid=${typeid}`;
        return this.http.get<IW2Client[]>(url);
    }

    submit(year: number, clients: IW2Client[]){
        const url = `${this.w2}/SaveW2Processing/year/${year}`;
        return this.http.post<IW2ProcessingSubmitted>(url, clients);
    }

    createW2Reports(year: number, clientIds: number[]){
        const url = `${this.w2}/createw2reports/year/${year}`;
        return this.http.post<ICreatedClient[]>(url, clientIds);
    }

    createW2ProcessingOneTimeBilling(year: number, clientId: number, amount: number){
        const url = `${this.w2}/create/w2Processing/oneTimeBilling/year/${year}/client/${clientId}/amount/${amount}`;
        return this.http.post<boolean>(url, clientId);
    }

    createW2ProcessingManualInvoice(year: number, clientId: number, amount: number){
        const url = `${this.w2}/create/w2Processing/manualInvoice/year/${year}/client/${clientId}/amount/${amount}`;
        return this.http.post<any>(url, clientId, {responseType: 'blob' as any, observe: 'response'}).pipe(
            map((response: HttpResponse<Blob>) => {
                var reportUrl = URL.createObjectURL(response.body);
                window.open(reportUrl, '_blank')
                return response.body;
            })
        );
    }

    saveW2Notes(notes: IClientW2ProcessingNotes){
        const url = `${this.w2}/saveW2Notes`;
        return this.http.post<boolean>(url, notes);
    }

    create1099s(year: number, clients: IW2Client[]){
        const url = `${this.w2}/create1099s/year/${year}`;
        return this.http.post<any>(url, clients, {responseType: 'blob' as any, observe: 'response'})
        .pipe(
            map((response: HttpResponse<Blob>) => {
                var reportUrl = URL.createObjectURL(response.body);
                window.open(reportUrl, '_blank');
                return response.body;
            })
        );
    }

    summarize(year: number, clientId: number){
        const url = `${this.w2}/summarize/year/${year}/client/${clientId}`;
        return this.http.get<boolean>(url);
    }

    createManifest(year: number, uniqueId: string){
        const url = `${this.w2}/createManifest/year/${year}/${uniqueId}`;
        return this.http.post<any>(url, uniqueId, {responseType: 'blob' as any, observe: 'response'})
        .pipe(
            map((response: HttpResponse<Blob>) => {
                var reportUrl = URL.createObjectURL(response.body);
                window.open(reportUrl, '_blank');
                return response.body;
            })
        )
    }
}
