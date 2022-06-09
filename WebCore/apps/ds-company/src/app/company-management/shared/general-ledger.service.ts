import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ClientDivisionDto } from '@ajs/ds-external-api/models';
import { GeneralLedgerAccount } from './models/general-ledger-account.model';

@Injectable({
  providedIn: 'root'
})
export class GeneralLedgerService {

  constructor(private http: HttpClient) { }

  private api = `api/general-ledger`;
  private clientAPI = `api/client`;
  private payrollAPI = `api/payroll`;

  getClientDivisions(clientId: number): Observable<ClientDivisionDto[]> {
    const url = `${this.clientAPI}/${clientId}/client-divisions`;
    let params = new HttpParams();
    params = params.append('clientId', clientId.toString());

    return this.http.get<ClientDivisionDto[]>(url, { params: params });
  }

  getClientDivisionsWithDepartments(): Observable<ClientDivisionDto[]> {
    const url = `${this.api}/get/client/gl/divisions`;
    let params = new HttpParams();

    return this.http.get<ClientDivisionDto[]>(url, { params: params });
  }

  

  getClientGeneralLedgerAccounts() : Observable<GeneralLedgerAccount[]> {
    const url = `${this.api}/get/client/gl/accounts`;
    let params = new HttpParams();

    return this.http.get<GeneralLedgerAccount[]>(url, {params: params});
  }

  

  updateCompanyLedger(item:GeneralLedgerAccount): Observable<GeneralLedgerAccount> {
    const url = `${this.api}/put/client/gl/account`;
    return this.http.put<GeneralLedgerAccount>(url,item);
  }

  deleteCompanyLedger(item:GeneralLedgerAccount): Observable<GeneralLedgerAccount> {
    const url = `${this.api}/delete/client/gl/account`;
    return this.http.put<GeneralLedgerAccount>(url,item);
  }

  uploadCompanyLedgers(resource: File): Observable<{ totalRecords:number, errorRecords :GeneralLedgerAccount[]}>{
      // Create form data 
      const formData = new FormData();  
      
      // Store form name as "file" with file data 
      formData.append("file", resource, resource.name);
      
      return this.http.post<any>(
          `${this.api}/uploadGLAccounts` , formData);
  }
}
