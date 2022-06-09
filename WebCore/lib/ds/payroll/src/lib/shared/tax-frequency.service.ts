import { Injectable }             from '@angular/core';
import * as angular               from "angular";
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable }             from 'rxjs';
import { ITaxFrequency } from './tax-frequency-list.model'

@Injectable({
    providedIn: 'root'
  })
  export class TaxFrequencyService {
  
    constructor(private http: HttpClient) { }
  
      private api = 'api/payroll';
  
      getTaxFrequency(): Observable<ITaxFrequency[]>{
        const url  = `${this.api}/frequency/info`;
        let params = new HttpParams();
        return this.http.get<ITaxFrequency[]>(url);
      }  
  }