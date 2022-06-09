import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { TaxDeferral } from '../shared/models/tax-deferral.model';

@Injectable({
    providedIn: 'root'
})
export class TaxDeferralsService {
    
    private api = `api/tax-deferrals`;
    
    constructor(private http: HttpClient) {}
    
    getTaxDeferrals(clientId: number): Observable<TaxDeferral[]> {
        const url = `${this.api}/clients/${clientId}`;
        return this.http.get<TaxDeferral[]>(url);
    }
    
    updateTaxDeferral(dto: TaxDeferral): Observable<TaxDeferral> {
        const url = `${this.api}/clients/${dto.clientId}/deferrals/${dto.clientTaxDeferralId}`;
        return this.http.put<TaxDeferral>(url, dto);
    }
    
    createTaxDeferral(dto: TaxDeferral): Observable<TaxDeferral> {
        const url = `${this.api}/clients/${dto.clientId}/deferrals`;
        return this.http.post<TaxDeferral>(url, dto);
    }
    
    deleteTaxDeferral(clientId: number, clientTaxDeferralId: number): Observable<null> {
        const url = `${this.api}/clients/${clientId}/deferrals/${clientTaxDeferralId}`;
        return this.http.delete<null>(url);
    }
    
}
