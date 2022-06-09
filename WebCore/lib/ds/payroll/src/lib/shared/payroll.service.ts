import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IPayrollPayCheckList } from './payroll-paycheck-list.model';
import { IPayrollHistoryInfo, IBasicPayrollHistory, IPayrollRunType } from './payroll-history-info.model';
import { IBankDepositInfo } from './bank-deposit-info.model';
import { IBankDepositCountInfo, IPayrollEmailLog } from '.';
import { IStandardReport } from './standard-report.model';
import { IEmailReportOptions } from './email-report-options.model';
import { IEmployeePayType } from 'lib/ds/reports/src/lib/shared';
import { IPaycheckHistory } from '@ajs/payroll/employee/shared';
import { IPaycheckInfo, IPaycheckEarningsDetail, IPaycheckEarningsHours, IPaystubOptions } from './paycheck-info.model';
import * as moment from 'moment';
import { switchMap } from 'rxjs/operators';
import { IOpenPayrollDetail } from '@ajs/payroll/history/models';
import { IPaycheckHistorySaveVoidChecksDto, PaycheckHistorySaveVoidChecksDto } from './paycheck-history-save-void-checks-dto.model';
import { PayrollReportsToEmail } from './payroll-reports-to-email.enum';


@Injectable({
    providedIn: 'root'
})
export class PayrollService {

    constructor(private http: HttpClient) { }

    private api = 'api/payroll';
    private api_employees = 'api/employees'


    getPayrollPaycheckHistory(payrollId: number): Observable<IPayrollPayCheckList[]> {
        const url = `${this.api}/get/payrolls/${payrollId}/paycheckList`;
        let params = new HttpParams();
        params = params.append('payrollId', payrollId.toString());
        // params     = params.append('viewRates'    , viewRates.toString());
        return this.http.get<IPayrollPayCheckList[]>(url, { params: params });
    }

    getPayryollInfoHistoryByPayrollId(payrollId: number): Observable<IPayrollHistoryInfo> {
        const url = `${this.api}/history/${payrollId}/info`;
        let params = new HttpParams();
        params = params.append('payrollId', payrollId.toString());
        return this.http.get<IPayrollHistoryInfo>(url, { params: params });
    }
    getBasicPayryollHistoryByPayrollId(payrollId: number): Observable<IPayrollHistoryInfo> {
        const url = `${this.api}/history/${payrollId}/basic`;
        let params = new HttpParams();
        params = params.append('payrollId', payrollId.toString());
        return this.http.get<IPayrollHistoryInfo>(url, { params: params });
    }

    getPayrollHistoryBankDepositInfoByPayrollId(payrollId: number): Observable<IBankDepositInfo> {
        const url = `${this.api}/history/${payrollId}/bank/info`;
        let params = new HttpParams();
        params = params.append('payrollId', payrollId.toString());
        return this.http.get<IBankDepositInfo>(url, { params: params });
    }

    getPayrollHistoryBankDepositCountInfoByPayrollId(payrollId: number): Observable<IBankDepositCountInfo> {
        const url = `${this.api}/history/${payrollId}/bank/count/info`;
        let params = new HttpParams();
        params = params.append('payrollId', payrollId.toString());
        return this.http.get<IBankDepositCountInfo>(url, { params: params });
    }

    getPayrollHistoryReportList(): Observable<IStandardReport[]> {
        const url = `${this.api}/reports/payroll/list`;
        let params = new HttpParams();
        return this.http.get<IStandardReport[]>(url, { params: params });
    }

    /**
     * Uses Hangfire if `([PayrollReportsToEmail.PayrollExportFile, PayrollReportsToEmail.ComPsychExportFile]).some(x => x === reportTypeId)`
     * @param payrollId
     * @param reportTypeId
     * @param checkDate
     * @param absoluteUrl
     * @returns
     */
    generateReport(payrollId: number, reportTypeId: PayrollReportsToEmail, checkDate: string, absoluteUrl: string): Observable<number> {
        const isHangfireReport = (
               reportTypeId == PayrollReportsToEmail.PayrollExportFile
            || reportTypeId == PayrollReportsToEmail.ComPsychExportFile
        );
        const url = (isHangfireReport)
          ? `${this.api}/generateReportsHangfire`
          : `${this.api}/generateReports`;
        let params = new HttpParams();

        params = params.append('payrollId', payrollId.toString());
        params = params.append('reportTypeId', reportTypeId.toString());
        params = params.append('checkDate', checkDate);
        params = params.append('absoluteUrl', absoluteUrl);

        return this.http.get<number>(url, { params: params });
    }

    getEmailReportOptions(): Observable<IEmailReportOptions> {
        const url = `${this.api}/email/report/options`;
        let params = new HttpParams();

        return this.http.get<IEmailReportOptions>(url, { params: params });
    }

    /**
     * use like:
     * passPayrollIdIntoRequest(payrollId$, (x) => getPayrollEmailLog(x, 2));
     */
    passPayrollIdIntoRequest<T>(payrollId$: Observable<number>, mapper: (payrollId: number) => Observable<T>) {
        return payrollId$.pipe(switchMap(payrollId => mapper(payrollId)));
    }

    getPayrollEmailLog(payrollId: number, logType: PayrollReportsToEmail): Observable<IPayrollEmailLog> {
        const url = `${this.api}/email/status/${payrollId}/type/${logType}`;
        let params = new HttpParams();

        params = params.append('payrollId', payrollId.toString());
        params = params.append('logType', logType.toString());

        return this.http.get<IPayrollEmailLog>(url, { params: params });
    }

    getBasicPayrollHistoryByStatus(clientId: number = null): Observable<IBasicPayrollHistory[]> {
        const url = `${this.api}/history/basic/closed`;
        let params = new HttpParams();
        if (clientId)
            params = params.append('clientId', clientId.toString());
        return this.http.get<IBasicPayrollHistory[]>(url, { params: params });
    }

    getBasicPayrollHistory(clientId: number = null): Observable<IBasicPayrollHistory[]> {
        const url = `${this.api}/history/basic`;
        let params = new HttpParams();
        if (clientId)
            params = params.append('clientId', clientId.toString());
        return this.http.get<IBasicPayrollHistory[]>(url, { params: params });
    }

    getPayType(): Observable<IEmployeePayType[]> {
        const url = `${this.api}/pay/info`;
        return this.http.get<IEmployeePayType[]>(url);
    }

    getPayrollRun(): Observable<IPayrollRunType[]> {
        const url = `${this.api}/pay/runtype`;
        return this.http.get<IPayrollRunType[]>(url);
    }

    /**
     * @see API\GET: /api/payroll/check-history/${employeeId}
     * @param employeeId ID of the employee to get paychecks for.
     * @param clientId ID of the client to get paychecks for.
     * @param startDate Start of date range to search for paychecks. Note that we discard the Time portion of the Date object.
     * @param endDate End of date range to search for paychecks. Note that we discard the Time portion of the Date object.
     */
    getEmployeePayHistoryByEmployeeId(employeeId: number, clientId: number, startDate: Date, endDate: Date): Observable<IPaycheckHistory[]> {
        let params = new HttpParams();
        params = params.append('clientId', clientId.toString());
        params = params.append('startDate', moment(startDate).format('YYYY-MM-DD'));
        params = params.append('endDate', moment(endDate).format('YYYY-MM-DD'));
        return this.http.get<IPaycheckHistory[]>(`${this.api}/check-history/${employeeId}`, {params: params});
    }

    getCurrentPayrollByClientId(clientId: number = null): Observable<IBasicPayrollHistory> {
        const url = `${this.api}/current/payroll`;
        let params = new HttpParams();
        if (clientId)
            params = params.append('clientId', clientId.toString());
        return this.http.get<IBasicPayrollHistory>(url, { params: params });
    }

  getPayrollHistoryByEmployeeId(clientId : number , employeeId : number) : Observable<IBasicPayrollHistory[]> {
    const url  = `${this.api}/employee/payroll-history`;
    let params = new HttpParams();
    params     = params.append('clientId', clientId.toString());
    params     = params.append('employeeId', employeeId.toString());

    return this.http.get<IBasicPayrollHistory[]>(url, { params: params });
  }

    getCurrentPaycheckWithFullDetail(employeeId: number): Observable<IPaycheckInfo[]> {
        return this.http.get<IPaycheckInfo[]>(`${this.api_employees}/${employeeId}/paychecks/current/detail`);
    }

    getCurrentPaycheckEarningsDetail(employeeId: number): Observable<IPaycheckEarningsDetail[]> {
        return this.http.get<IPaycheckEarningsDetail[]>(`${this.api_employees}/${employeeId}/paychecks/current/earnings/detail`);
    }

    getCurrentPaycheckEarningsHours(employeeId: number): Observable<IPaycheckEarningsHours[]> {
        return this.http.get<IPaycheckEarningsHours[]>(`${this.api_employees}/${employeeId}/paychecks/current/earnings/hours`);
    }

    getCurrentPaycheckStubOptions(employeeId: number): Observable<IPaystubOptions> {
        return this.http.get<IPaystubOptions>(`${this.api_employees}/${employeeId}/paychecks/current/stub-options`);
    }

    /**
     * GETs voidable paychecks for an employee.
     * @param employeeId ID of the employee to get voidable paychecks for.
     * @see API\GET: /api/payroll/check-history/voidable-checks/{employeeId:int}
     */
    getGenPaycheckVoidableChecks(employeeId: number): Observable<IPaycheckHistory[]> {
        return this.http.get<IPaycheckHistory[]>(`${this.api}/check-history/voidable-checks/${employeeId}`);
    }

    /**
     * POSTs Array of DTOs, representing the paychecks we wish to void.
     * @param paycheckHistoryDtos Array of DTOs of the checks we wish to void.
     * @see API\POST: /api/payroll/check-history/voidable-checks/
     */
    postGenPaycheckVoidableChecks(paycheckHistoryDtos: Array<PaycheckHistorySaveVoidChecksDto>)
        : Observable<Array<IPaycheckHistorySaveVoidChecksDto>> {
        return this.http.post<Array<IPaycheckHistorySaveVoidChecksDto>>(`${this.api}/check-history/voidable-checks/`, paycheckHistoryDtos);
    }

    /**
     * POSTs IDs of genPaycheckHistory records for which we're attempting to void paychecks.
     * @param employeeId ID of the employee to void paychecks for.
     * @param genPaycheckHistoryIds IDs of the paychecks we're attempting to void.
     * @see API\POST: /api/payroll/check-history/{employeeId:int}/save-void-checks
     */
    saveVoidChecks(employeeId: number, genPaycheckHistoryIds: Array<number>): Observable<boolean> {
        const uri = `${this.api}/check-history/${employeeId}/save-void-checks`;
        // const requestBody = {'genPaycheckHistoryIds': genPaycheckHistoryIds};
        return this.http.post<boolean>(uri, genPaycheckHistoryIds);
    }

    /**
     * GETs dto for the client's currently open payroll,
     * or null if there is no open payroll.
     * @see API\GET: /api/payroll/openPayroll
     */
    getOpenPayrollDetail(): Observable<IOpenPayrollDetail> {
        const uri = `${this.api}/openPayroll`;
        return this.http.get<IOpenPayrollDetail>(uri);
    }

}
