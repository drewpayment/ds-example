import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, forkJoin, of, Subject } from 'rxjs';
import { catchError, tap, refCount, publishReplay } from 'rxjs/operators';
import { AccountService } from '@ds/core/account.service';
import { Store } from '@ngrx/store';
import { getUser, UserState } from '@ds/core/users/store/user.reducer';
import { AutomaticBilling, BillingItem, BillingItemDescription, BillingPriceChart, FeatureOptionGroup, FeatureOptions, PendingBillingCredit } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Injectable()
export class BillingService {

    private api = `api/billing`;
    private clientApi = `api/clients`;

    /***************************************
     * _______________DATA__________________
     * [0]: ClientBilling
     * [1]: ClientOneTimeBilling
     * [2]: BillingItemDescriptions
     * [3]: BillingPriceChartList
     *************************************/
    data$: Observable<any>;
    billingItems$ = new Subject<BillingItem[]>();
    oneTimeItems$ = new Subject<BillingItem[]>();
    automaticBillingItems$ = new Subject<AutomaticBilling[]>();
    pendingCreditItems$ = new Subject<PendingBillingCredit[]>();

    constructor(
        private http: HttpClient,
        private accountService: AccountService,
        private ngxMsgSvc: NgxMessageService,
        private store: Store<UserState>,
    ) { this.fetchFakeBillingResolver(); }

    getClientBilling(clientId: number): Observable<BillingItem[]> {
        const url = `${this.api}/client-billing/client/${clientId}`;
        let params = new HttpParams();
        params = params.append('clientId', clientId.toString());

        return this.http.get<BillingItem[]>(url, { params: params });
    }

    getClientOneTimeBilling(clientId: number): Observable<BillingItem[]> {
        const url = `${this.api}/client-one-time-billing/client/${clientId}`;
        let params = new HttpParams();
        params = params.append('clientId', clientId.toString());

        return this.http.get<BillingItem[]>(url, { params: params });
    }

    getBillingItemDescription() {
        const url = `${this.api}/billing-item-description-list`;
        let params = new HttpParams();

        return this.http.get<BillingItemDescription[]>(url, { params: params });
    }

    getBillingPriceChartList() {
        const url = `${this.api}/billing-price-chart-list`;
        let params = new HttpParams();

        return this.http.get<BillingPriceChart[]>(url, { params: params });
    }

    getClientFeatures(clientId: number): Observable<FeatureOptionGroup[]> {
        const url = `${this.clientApi}/${clientId}/features`;
        let params = new HttpParams();
        params = params.append('clientId', clientId.toString());

        return this.http.get<FeatureOptionGroup[]>(url, { params: params });
    }

    getFeatureOptions(): Observable<FeatureOptions[]> {
        const url = `api/client/get/account/features`;

        return this.http.get<FeatureOptions[]>(url);
    }

    getAutomaticBilling(): Observable<AutomaticBilling[]> {
        const url = `${this.api}/automatic`;

        return this.http.get<AutomaticBilling[]>(url);
    }

    getPendingBillingCredit(clientId: number): Observable<PendingBillingCredit[]> {
        const url = `${this.api}/pending/credit`
        let params = new HttpParams();
        params = params.append('clientId', clientId.toString());

        return this.http.get<PendingBillingCredit[]>(url, { params: params });
    }

    saveClientFeatures(clientId: number, features: FeatureOptionGroup[]) {
        const url = `${this.clientApi}/${clientId}/features/save`;

        return this.http.post<FeatureOptionGroup[]>(url, features);
    }

    saveBillingItem(billingItem: BillingItem) {
        let url = `${this.api}`;

        return this.http.post<BillingItem>(url, billingItem);
    }

    saveAutomaticBillingItem(billingItem: AutomaticBilling) {
        let url = `${this.api}/save/automatic/item`;

        return this.http.post<BillingItem>(url, billingItem);
    }

    savePendingBillingCredit(billingItem: PendingBillingCredit) {
        let url = `${this.api}/save/pending/credit`;

        return this.http.post<PendingBillingCredit>(url, billingItem);
    }

    transferPendingBillingCredit(billingItem: PendingBillingCredit) {
        let url = `${this.api}/transfer/pending/credit`;

        return this.http.post<PendingBillingCredit>(url, billingItem);
    }

    deleteBillingItem(billingItemId: number) {
        let url = `${this.api}`;
        let params = new HttpParams();
        params = params.append('billingItemId', billingItemId.toString());

        return this.http.delete(url, { params: params });
    }

    deleteAutomaticBillingItem(automaticBillingItemId: number) {
        let url = `${this.api}/delete/automatic/item`;
        let params = new HttpParams();
        params = params.append('automaticBillingItemId', automaticBillingItemId.toString());

        return this.http.delete(url, { params: params });
    }


    /***************************************
     * __DATA In Resolver__
     * [0]: BillingItemDescriptions
     * [1]: BillingPriceChartList
     *************************************/
    fetchFakeBillingResolver() {
        const apiCall = this.accountService.PassUserInfoToRequest(userInfo => forkJoin(
            this.getBillingItemDescription(),
            this.getBillingPriceChartList(),
            of(userInfo)
        ));

        this.data$ = this.handleError(apiCall.pipe(publishReplay(1), refCount()));
    } // end of billing resolver

    fetchBillingItems(clientId) {
        const apiCall = this.getClientBilling(clientId).pipe(tap(x => this.billingItems$.next(x)));
        this.handleError(apiCall).subscribe();
    }

    fetchOneTimeItems(clientId) {
        const apiCall = this.getClientOneTimeBilling(clientId).pipe(tap(x => this.oneTimeItems$.next(x)));
        this.handleError(apiCall).subscribe();
    }

    fetchPendingItems(clientId) {
        const apiCall = this.getPendingBillingCredit(clientId).pipe(tap(x => this.pendingCreditItems$.next(x)));
        this.handleError(apiCall).subscribe();
    }

    fetchAutomaticBillingItems() {
        const apiCall = this.getAutomaticBilling().pipe(tap(x => this.automaticBillingItems$.next(x)));
        this.handleError(apiCall).subscribe();
    }

    handleError<T>(apiCall: Observable<T>): Observable<any> {
        return apiCall.pipe(catchError(error => {
            this.ngxMsgSvc.setErrorMessage(error.message);
            return of(null);
        }));
    }

}
