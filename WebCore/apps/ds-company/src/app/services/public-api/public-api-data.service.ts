import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PublicApiDataService {

  constructor(private http: HttpClient) {}

  getEmployeeDemographicReport(clientId: number): Observable<any> {
    const url = `api/internal/reports/clients/${clientId}/employees/demographic`;
    return this.http.get(url, {
      responseType: 'blob',
      headers: {
        'Accept': 'application/pdf'
      },
    });
  }

  getEmployeeDeductionsReport(clientId: number): Observable<any> {
    const url = `api/internal/reports/clients/${clientId}/employees/deductions`;
    return this.http.get(url, {
      responseType: 'blob',
      headers: {
        'Accept': 'application/pdf'
      },
    });
  }

  getClientDeductionReport(clientId: number): Observable<any> {
    const url = `api/internal/reports/clients/${clientId}/deductions`;
    return this.http.get(url, {
      responseType: 'blob',
      headers: {
        'Accept': 'application/pdf'
      },
    });
  }

}
