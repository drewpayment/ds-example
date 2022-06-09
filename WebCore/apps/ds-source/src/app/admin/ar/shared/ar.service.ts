import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject, throwError } from 'rxjs';
import { IArReport } from './ar-report.model';
import { IFileFormat } from './ar-file-format.model';
import { IArReportParameter } from './ar-report-parameter.model';
import { catchError, map, tap } from 'rxjs/operators';
import { IArManualInvoice } from './ar-manual-invoice.model';
import { IArReportPayroll } from './ar-report-payroll.model';
import { IArBillingItemDesc } from './ar-billing-item-desc.model';
import { saveAs } from "file-saver";
import * as moment from 'moment';
import { MOMENT_FORMATS } from '@ds/core/shared';
import { IOption } from '../shared/option.model';
import { ArDeposit } from './ar-deposit.model';
import { FormControl } from '@angular/forms';
import { IArPayment } from './ar-payment.model';
import { IArDominionCheckPayment } from './ar-dominion-check-payment.model';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { IArClientCheckPayment } from './ar-client-check-payment.model';

@Injectable({
    providedIn: 'root'
})
export class ArService {
    private api = `api/ar`;

    constructor(private http: HttpClient, private msg: DsMsgService) { }

    getReports(): Observable<IArReport[]>{
        var url = `${this.api}/arReports`;
        return this.http.get<IArReport[]>(url);
    }

    getFileFormats(): Observable<IFileFormat[]>{
        var url = `${this.api}/arReports/fileFormats`;
        return this.http.get<IFileFormat[]>(url);
    }

    getClientList(): Observable<IOption[]>{
        var url = `${this.api}/clients`;
        return this.http.get<IOption[]>(url);
    }

    getClientManualInvoices(clientId: number): Observable<IArManualInvoice[]>{
        var url = `${this.api}/clientManualInvoices/${clientId.toString()}`;
        return this.http.get<IArManualInvoice[]>(url);
    }

    getClientPayrolls(clientId: number): Observable<IArReportPayroll[]>{
        var url = `${this.api}/clientPayrolls/${clientId.toString()}`;
        return this.http.get<IArReportPayroll[]>(url);
    }

    getBillingItems(): Observable<IArBillingItemDesc[]>{
        var url = `${this.api}/billingItemDescriptions`;
        return this.http.get<IArBillingItemDesc[]>(url);
    }

    getAllBillingItems(): Observable<IArBillingItemDesc[]>{
        var url = `${this.api}/allBillingItemDescriptions`;
        return this.http.get<IArBillingItemDesc[]>(url);
    }

    generateReport(reportParams: IArReportParameter, saveReport = false){
        var url = `${this.api}/arReports/generateArReport`;
        return this.http.post<any>(url, reportParams, {responseType: 'blob' as any, observe: 'response'})
        .pipe(
            map((response: HttpResponse<Blob>) => {
                if (saveReport || reportParams.fileFormat == 'EXCEL')
                {
                    var filename = response.headers.get('x-filename');
                    saveAs(response.body, filename);
                }
                else
                {
                    var reportUrl = URL.createObjectURL(response.body);
                    window.open(reportUrl, '_blank')
                }

                return response.body;
            })
        )
    }

    generateAchDepositCsv(startDate: Date, endDate: Date) {
        var url = `${this.api}/generateAchDepositCsv`;

        let params = new HttpParams();
        params = params.append('startDate', moment(startDate).format(MOMENT_FORMATS.API));
        params = params.append('endDate', moment(endDate).format(MOMENT_FORMATS.API));

        return this.http.get<any>(url, {params: params, responseType: 'blob' as any, observe: 'response'})
        .pipe(
            map((response: HttpResponse<Blob>) => {
                saveAs(response.body, `ACHDeposits${moment(startDate).format(MOMENT_FORMATS.US_DASHES)}To${moment(endDate).format(MOMENT_FORMATS.US_DASHES)}.csv`);
                return response.body;
            })
        );
    }

    getBillingYears(){
        var url = `${this.api}/manual-invoice/getbillingyears`;
        return this.http.get<number[]>(url);
    }

    saveManualInvoice(manualInvoice: IArManualInvoice) {
        var url = `${this.api}/createManualInvoice`;
        return this.http.post<IArManualInvoice>(url, manualInvoice);
    }

    getArDepositsWithNoPostedDate(){
        var url = `${this.api}/getardepositswithnoposteddate`;
        return this.http.get<ArDeposit[]>(url);
    }

    getPostedArDeposits(startDate: string, endDate: string) {
        var url = `${this.api}/getpostedardeposits/${startDate}/${endDate}`;
        return this.http.get<any>(url);
    }

    getPaymentsByDepositId(depositId: number){
        var url = `${this.api}/getpaymentsbydepositid/${depositId}`;
        return this.http.get<IArPayment[]>(url);
    }

    updateDeposit(deposit: ArDeposit){
        var url = `${this.api}/updatedeposit`;
        return this.http.post<ArDeposit>(url, deposit);
    }

    updatePayment(payment: IArPayment){
        var url = `${this.api}/updatepayment`;
        return this.http.post<IArPayment>(url, payment);
    }

    updatePayments(payment: IArPayment[]){
        var url = `${this.api}/updatepayments`;
        return this.http.post<IArPayment[]>(url, payment);
    }

    getUnpaidDominionCheckPaymentsByDateRange(startDate: string, endDate: string) {
        var url = `${this.api}/getunpaiddominioncheckpaymentsbydaterange?startDate=${startDate}&endDate=${endDate}`;
        return this.http.get<IArDominionCheckPayment[]>(url);
    }

    getUnpaidInvoicesByClientId(clientId: number) {
        var url = `${this.api}/getunpaidinvoicesbyclientid?clientId=${clientId}`;
        return this.http.get<IArClientCheckPayment[]>(url);
    }

    setCheckPaymentToPaid(depositId: number, checkPayment: IArDominionCheckPayment) {
        var url = `${this.api}/saveDominionCheckPaymentAsPaid/${depositId}`;
        return this.http.post<IArDominionCheckPayment>(url, checkPayment);
    }

    setCheckPaymentToUnpaid(depositId: number, checkPayment: IArDominionCheckPayment) {
        var url = `${this.api}/saveDominionCheckPaymentAsUnpaid/${depositId}`;
        return this.http.post<IArDominionCheckPayment>(url, checkPayment);
    }

    getArDepositById$ = new BehaviorSubject<ArDeposit>(null);

    getArDepositById(depositId: number): Observable<ArDeposit>{
        var url = `${this.api}/arDeposit/${depositId}`;
        return this.http.get<ArDeposit>(url).pipe(
            tap(x => {
                this.getArDepositById$.next(x);
            }),
            catchError((error) => {
                this.msg.setTemporaryMessage('Sorry, this operation failed: \'Get Deposit Info\'', MessageTypes.error, 60000);
                return throwError(error);
            })
        );
    }

    createDeposit() {
        var url = `${this.api}/createDeposit`;
        return this.http.get<ArDeposit>(url);
    }

    postDeposit(deposit: ArDeposit){
        var url = `${this.api}/postDeposit`;
        return this.http.post<ArDeposit>(url, deposit);
    }

    reopenDeposit(deposit: ArDeposit){
        var url = `${this.api}/reopenDeposit`;
        return this.http.post<ArDeposit>(url, deposit);
    }

    deletePayment(payment: IArPayment) {
        var url = `${this.api}/deletePayment`;
        return this.http.post<IArPayment>(url, payment);
    }

    saveClientCheckPayments(depositId: string, checkPayments: IArClientCheckPayment[]): Observable<IArClientCheckPayment[]>{
        var url = `${this.api}/saveclientcheckpayments/${depositId}`;
        return this.http.post<IArClientCheckPayment[]>(url, checkPayments);
    }

    deleteArDeposit(arDepositId: number) {
        var url = `${this.api}/deleteArDepositById/${arDepositId}`;
        return this.http.post<ArDeposit>(url, null);
    }
}
