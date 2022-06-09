import { Injectable }             from '@angular/core';
import * as angular               from "angular";
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable }             from 'rxjs';
import { IVendorMaintenanceInfo } from './vendor-maintenance-info.model'

@Injectable({
    providedIn: 'root'
  })
  export class VendorMaintenanceService {   
  
    constructor(private http: HttpClient) { }
  
      private api = 'api/payroll';
  
      getVendors(): Observable<IVendorMaintenanceInfo[]>{
        const url  = `${this.api}/vendor/info`;
        let params = new HttpParams();
        return this.http.get<IVendorMaintenanceInfo[]>(url);
      }
       
      saveVendor(vendor: IVendorMaintenanceInfo) {
        let url = `${this.api}/vendor/update`;
        if (vendor.vendorId != null) 
            url += `/${vendor.vendorId}`;
        else
          url +=`/0`
        return this.http.post<IVendorMaintenanceInfo>(url, vendor);
        
      }

      deleteVendor(vendorId: number) {
        let url = `${this.api}/vendor/delete/${vendorId}`;
        return this.http.delete(url);
      }
  }
  