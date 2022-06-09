import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICheckStockBilling, ICheckStockOrder } from '.';

@Injectable({
  providedIn: 'root'
})
export class CheckStockService {

  constructor(private http: HttpClient) { }

  private api = 'api/checkstockorder';

    getCheckStockPrice(): Observable<ICheckStockBilling[]> {
        const url = `${this.api}/billing`;
        let params = new HttpParams();
        //params     = params.append('payrollId'    , payrollId.toString());
        return this.http.get<ICheckStockBilling[]>(url, { params: params });
    }

    getCheckNumber() : Observable<{nextCheckNumber: number}> {
        const url  = `${this.api}/nextcheck`;
        let params = new HttpParams();
        return this.http.get<{nextCheckNumber: number}>(url, { params: params });
    }

    createCheckStockOrder(dto : ICheckStockOrder) : Observable<ICheckStockOrder> {
        const url = `${this.api}/createOrder`;
        return this.http.post<ICheckStockOrder>(url, dto);
    }
}
