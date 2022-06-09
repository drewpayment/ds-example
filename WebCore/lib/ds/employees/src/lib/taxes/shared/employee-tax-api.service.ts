import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpClient, HttpResponse, HttpHeaders} from '@angular/common/http';
import { AccountService } from '@ds/core/account.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { UserInfo } from '@ds/core/shared';
import { IFilingStatus } from './filing-status.model';
import { IEmployeeTax, IFormSignatureDefinition } from './employee-tax.model';
import { IEmployeeOnboardingTax } from './employee-onboarding-tax.model';
import * as saveAs from 'file-saver';

import { IFormStatusData } from '@ajs/onboarding/shared/models';

@Injectable({
    providedIn: 'root'
})
export class EmployeeTaxExemptionService {
    private readonly api = 'api';
    user: UserInfo;

    constructor(private http: HttpClient, private account: AccountService, private msg: DsMsgService) {
    }

    getTaxes(employeeId: number): Observable<IEmployeeTax[]> {
        const url = `${this.api}/employees/${employeeId}/tax-exemptions`;
        return this.http.get<IEmployeeTax[]>(url)
            .pipe(
            catchError(this.httpError('getTaxes', <IEmployeeTax[]>[]))
            );
    }

    getFilingStatuses(): Observable<IFilingStatus[]> {
        const url = `${this.api}/taxes/filing-statuses`;
        return this.http.get<IFilingStatus[]>(url)
            .pipe(
            catchError(this.httpError('getFilingStatuses', <IFilingStatus[]>[]))
            );
    }

    updateTax(model: IEmployeeTax): Observable<IEmployeeTax> {
        const url = `${this.api}/employee/${model.employeeId}/tax-exemptions`;
        return this.http.put<IEmployeeTax>(url, model)
            .pipe(
                catchError((error) => {
                    this.msg.setTemporaryMessage('Sorry, this operation failed: \'Update Tax Model\'', MessageTypes.error, 6000);
                    return throwError(error);
                })
            );
    }

    updateEmployeeOnboardingW4AndTaxWithNotification(model: IEmployeeTax) {
        const url = `${this.api}/employeeOnboarding/w4/sendNotification`;
        return this.http.put<IEmployeeTax>(url, model)
        .pipe(
            catchError((error) => {
                this.msg.setTemporaryMessage('Update Employee Tax operation failed.', MessageTypes.error, 6000);
                return throwError(error);
            })
        );
    }

    previewW4FormWithData(employeeId, definitionId, employeeOnboardingW4Dto): void {
        const url = `${this.api}/onboarding/employees/${employeeId}/forms/${definitionId}/previewWithData`;
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/pdf');
        this.http.post<HttpResponse<Blob>>(url, employeeOnboardingW4Dto,  {headers: headers, responseType: 'blob' as 'json'})
        .subscribe((response) => {
            saveAs(response, 'Federal W-4 (Preview).pdf');
        });
    }

    getFormStatusData(employeeId: number) {
        const url = `${this.api}/onboarding/employees/${employeeId}/forms`;
        return this.http.get<IFormStatusData[]>(url)
            .pipe(
                catchError(this.httpError('getFormStatus', <IFormStatusData[]>[]))
            );
    }

    postEmployeeFormUpdatesWithoutFinalize(employeeId: number, forms: IFormStatusData[]) {
        const url = `${this.api}/onboarding/employees/${employeeId}/forms/withoutOnboardingFinalize`;
        return this.http.post(url, forms)
        .pipe(
            catchError(this.httpError('getFormStatus', <IFormStatusData[]>[]))
        );
    }

    viewOriginalForm(definitionId) {
        const url = `${this.api}/onboarding/forms/${definitionId}/vieworiginal`;
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/pdf');
        this.http.get<HttpResponse<Blob>>(url, {headers: headers, responseType: 'blob' as 'json'})
        .subscribe((response) => {
            saveAs(response, 'Federal W-4 (Preview).pdf');
        });
    }

    getCurrentFormAndSignDefinitionForFedW4(): Observable<IFormSignatureDefinition> {
        const url = `${this.api}/onboarding/forms/current-form-def-fw4`;
        return this.http.get<IFormSignatureDefinition>(url)
            .pipe(
            catchError(this.httpError('getCurrentFormAndSignDefinitionForFedW4', <IFormSignatureDefinition>{}))
            );
    }


    private httpError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            const errorMsg = error.error.errors != null && error.error.errors.length
                ? error.error.errors[0].msg
                : error.message;

            this.account.log(error, `${operation} failed: ${errorMsg}`);

            // TODO: better job of transforming error for user consumption
            this.msg.setTemporaryMessage(`Sorry, this operation failed: ${errorMsg}`, MessageTypes.error, 6000);

            // let app continue by return empty result
            return of(result as T);
        };
    }
}
