import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IContactSearchOptions, ContactSearchOptions, ContactsProfileImageLoader, IContact } from '@ds/core/contacts';
import { map } from 'rxjs/operators';
import { IScheduledReportFileFormat } from './scheduled-report-file-format.model';
import { IEmployeeChangeList } from './employee-change-info.model';
import { IEmployeeChangeParameters } from './employee-change-parameters.model';
import { DsPopupService } from '@ajs/ui/popup/ds-popup.service';
import { IReportParameter } from '@ds/payroll';
import { IReportDownload } from './report-download-info.model';
import { IEmployeeSearchResultResponseData } from '@ajs/employee/search/shared/models';

@Injectable({
  providedIn: 'root'
})

export class ReportsService {  

  constructor(private http: HttpClient,
    private DsPopup: DsPopupService) { }

  static readonly EMPLOYEECHANGE_API_BASE = 'api/employee-change-history/reports';
  static readonly REPORTS_API_BASE = 'api/reports/changehistory';

  getReportSetupContacts(options: IContactSearchOptions = null) {
    let params = new HttpParams();

    if (options) {
        params = new ContactSearchOptions(options).toHttpParams();             
    }       

    return this.http.get<IEmployeeSearchResultResponseData>(`${ReportsService.REPORTS_API_BASE}/contacts`, { params: params })
        .pipe(map(result => {
          ContactsProfileImageLoader(<IContact[]><any>result.results);
          return result;
        }));
  }

  getFileFormats(): Observable<IScheduledReportFileFormat[]> {
    return this.http.get<IScheduledReportFileFormat[]>(`${ReportsService.REPORTS_API_BASE}/formats`);
  }

  getEmployeeChanges(dtos: IEmployeeChangeParameters): Observable<IEmployeeChangeList[]> {
    let params = new HttpParams();
    params = params.append('employeeId', dtos.employeeId.toString());
    params = params.append('startDate', dtos.startDate.toString());
    params = params.append('endDate', dtos.endDate.toString());
    params = params.append('sequenceId', dtos.sequenceId.toString());
    params = params.append('payType', dtos.payType.toString());
    params = params.append('employeeStatusId', dtos.employeeStatusId.toString());
    params = params.append('returnFilterOnly', dtos.returnFilterOnly.toString());
    params = params.append('csvChangeLogIds', dtos.csvChangeLogIds.toString());
        
    return this.http.get<IEmployeeChangeList[]>(`${ReportsService.EMPLOYEECHANGE_API_BASE}/changes`, {params: params});
    
  }

  generateReports(dtos: IReportParameter) {
    return this.http.post<IReportDownload>(`${ReportsService.EMPLOYEECHANGE_API_BASE}/generate-reports`, dtos);
  }


}
