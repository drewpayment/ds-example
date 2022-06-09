import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, Observer, forkJoin, BehaviorSubject } from 'rxjs';
import { ClientAlert } from '@ds/core/clients/shared/client-alert.model';
import { Client } from '../../../../../../apps/ds-source/src/app/employee/shared/models/client.model';
import { ClientAccountFeature } from '@ds/core/clients/shared/client-account-feature.model';
import { DefaultPageSettings } from '../../../../../../apps/ds-company/src/app/models/default-page-settings.model';
import { ClockTimeCard } from '../../../../../../apps/ds-source/src/app/employee/shared/models/clock-client-time-card.model';
import { EmployeeExitInterviewRequest } from '../../../../../../apps/ds-source/src/app/employee/shared/models/employee-exit-interview-request.model';
import { tap, map } from 'rxjs/operators';
import { MOMENT_FORMATS } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import * as moment from 'moment';
import { WorkNumberService } from '@ds/core/popup/shared/work-number.service';
import { IEmployeeWorkInfo } from 'lib/ds/employees/src/lib/profile/shared/employee-work-info.model';
import { date } from '@ajs/applicantTracking/application/inputComponents';
import { IAlert } from '../../../../../models/src/lib/alert.model';
import * as saveAs from 'file-saver';
import { AlertService } from '../../../../../../apps/ds-source/src/app/admin/company-alerts/shared/alert.service';
import { DominionShortcut } from 'apps/ds-company/src/app/models/dominion-shortcut.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  clockTimeCardItems$ = new BehaviorSubject<ClockTimeCard[]>(null);

  constructor(private http: HttpClient, private accountService: AccountService, private workNumberService: WorkNumberService) { }

  private clientAPI = `api/client`;
  private clientsAPI = `api/clients`;
  data$ = new BehaviorSubject<any>(null);

  getClientAlerts() : Observable<ClientAlert[]> {
    const url  = `${this.clientAPI}/get/client/alerts`;
    return this.http.get<ClientAlert[]>(url);
  }

  getDominionShortcutInfo(employeeId: number) : Observable<DominionShortcut> {
    if(employeeId == null)
      employeeId = 0;

    const url = `${this.clientAPI}/dominion/shorcut`;
    let params = new HttpParams();
    params = params.append('employeeId', employeeId.toString());

    return this.http.get<DominionShortcut>(url, {params: params});
  }

  getClient(clientId: number) : Observable<Client> {
    const url  = `${this.clientAPI}/get/client`;
    let params = new HttpParams();
    params     = params.append('clientId', clientId.toString());

    return this.http.get<Client>(url, {params: params});
  }

  getDefaultPageSetting(clientId: number, employeeId: number): Observable<DefaultPageSettings> {
    if(employeeId == null)
      employeeId = 0;

    const url  = `${this.clientAPI}/default/setup`;
    let params = new HttpParams();
    params     = params.append('clientId', clientId.toString());
    params     = params.append('employeeId', employeeId.toString());

    return this.http.get<DefaultPageSettings>(url, {params: params});
  }

  getNextPunchDetail(employeeId) : Observable<any> {
    const url  = `api/clock/punch-detail/${employeeId}`;
    let params = new HttpParams();
    //params     = params.append('employeeId', employeeId.toString());

    return this.http.get<any>(url, {params: params});
  }

  getEmployeePunches(employeeId: number, startDate: string, endDate: string): Observable<object[]> {
    const url  = `api/clock/get/employee/punches`;
    let params = new HttpParams();
    params     = params.append('id', employeeId.toString());
    params     = params.append('startDate', startDate.toString());
    params     = params.append('endDate', endDate.toString());

    return this.http.get<Object[]>(url, {params: params});
  }

  getWeeklyHoursWorked(clientId: number, employeeId: number, startDate: number, endDate: number, punchOption: number): Observable<object[]> {
    const url  = `api/clock/weekly-hours-worked/clients/${clientId}/employees/${employeeId}`;
    let params = new HttpParams();
    params     = params.append('clientId', clientId.toString());
    params     = params.append('employeeId', employeeId.toString());
    params     = params.append('punchOptionType', punchOption.toString());
    // params     = params.append('start', startDate.toString());
    // params     = params.append('end', endDate.toString());

    return this.http.get<Object[]>(url, {params: params});
  }

  getWeeklyHoursWorkedTest(clientId: number, employeeId: number): Observable<ClockTimeCard[]> {
    const url  = `api/clock/weekly-hours-worked/clients/${clientId}/employees/${employeeId}/full`;
    let params = new HttpParams();
    params     = params.append('clientId', clientId.toString());
    params     = params.append('employeeId', employeeId.toString());

    return this.http.get<ClockTimeCard[]>(url, {params: params});

  }

  getHoursWorked(clientId: number, employeeId: number, payrollId?: number, startDate?: Date, endDate?: Date): Observable<ClockTimeCard[]> {
    let payroll =  payrollId ? payrollId.toString() : "-1";
    let start   = startDate ? moment(startDate).format('YYYY-MM-DD') : '0001-01-01';
    let end     = endDate? moment(endDate).format('YYYY-MM-DD') : '0001-01-01';

    const url  = `api/clock/hours-worked/clients/${clientId}/employees/${employeeId}/payroll/${payroll}/start/${start}/end/${end}`;
    let params = new HttpParams();

    return this.http.get<ClockTimeCard[]>(url, {params: params});
  }

  getAvailableTimePolicies(employeeId: number): Observable<any[]> {
    const url  = `api/labor/setup/employees/${employeeId}/time-policies/available`;
    let params = new HttpParams();
    params     = params.append('employeeId', employeeId.toString());
    // params     = params.append('start', startDate.toString());
    // params     = params.append('end', endDate.toString());

    return this.http.get<any[]>(url, {params: params});
  }

  getAvailableOneTimeEarningSettingsList(employeeId: number): Observable<any[]> {
    const url  = `api/employees/${employeeId}/one-time-earning-settings`;
    let params = new HttpParams();
    params     = params.append('employeeId', employeeId.toString());

    return this.http.get<any[]>(url, {params: params});
  }

  updateAllowTurboTax(optIn: number) {
    const url  = `${this.clientAPI}/update/turbotax/optin`;
    let op     = { optIn: optIn }
    return this.http.post(url, op);

  }

  getEmployeeWorkInfo(employeeId: number): Observable<IEmployeeWorkInfo> {
    const url  = `api/employee/${employeeId}/work-info`;
    return this.http.get<IEmployeeWorkInfo>(url);
  }

  getEmployeeExitInterviewRequest(employeeId: number): Observable<EmployeeExitInterviewRequest> {
    const url  = `api/employees/${employeeId}/employee-exit-interview-request`;
    return this.http.get<EmployeeExitInterviewRequest>(url);
  }

  updateEmployeeExitInterviewRequest(employeeId: number, requestedBy: number): Observable<EmployeeExitInterviewRequest> {
    const url  = `api/employee/update-employee-exit-interview-request`;
    const params: EmployeeExitInterviewRequest = {
        employeeId: employeeId,
        requestedBy: requestedBy,
        requestedOn: moment().format(MOMENT_FORMATS.API),
        sentOn: null
    };
    return this.http.post<EmployeeExitInterviewRequest>(url, params);
  }

  createEmployeeExitInterviewRequest(employeeId: number, requestedBy: number): Observable<EmployeeExitInterviewRequest> {
    const url  = `api/employee/create-employee-exit-interview-request`;
    const params: EmployeeExitInterviewRequest = {
        employeeId: employeeId,
        requestedBy: requestedBy,
        requestedOn: moment().format(MOMENT_FORMATS.API),
        sentOn: null
    };
    return this.http.post<EmployeeExitInterviewRequest>(url, params);
  }

  fetchFakeResolver() {
    this.accountService.getUserInfo()
      .subscribe(u =>
        forkJoin(
            this.getDefaultPageSetting(u.clientId, u.employeeId),
            this.getClientAlerts(),
            this.getDominionShortcutInfo(u.employeeId),
            this.workNumberService.getWorkNumberInfo(u.clientId),
          ).subscribe(res => {
            this.data$.next(res);
          }, err => {
            console.dir(err);
          }));
  }

  fetchTimeCard(clientId: number, employeeId: number, payrollId?: number, startDate?: Date, endDate?: Date ) {
    this.getHoursWorked(clientId, employeeId, payrollId, startDate, endDate).subscribe(res => {
      this.clockTimeCardItems$.next(res);
    })
  }

  getFileAlertToDownload(item:IAlert) {
    let extn = item.alertLink.substring(item.alertLink.lastIndexOf('.') )
    let url = `${AlertService.CLIENT_API_BASE}/alert/${item.alertId}/download`;
    return this.http.post<Blob>(url,{},{responseType: 'blob' as any, observe: 'response'})
    .pipe(
        map((response: HttpResponse<Blob>) => {
            var downloadUrl = URL.createObjectURL(response.body);
            window.open(downloadUrl, '_blank');
            return response.body;
        })
    );
}
}
