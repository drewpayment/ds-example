import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { BillingDialogComponent } from './billing-dialog/billing-dialog.component';
import { filter, map, skip, switchMap, takeUntil, tap } from 'rxjs/operators';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { BillingWhatToCount } from '../../enums/billing-what-to-count.enum';
import { BillingFrequency } from '../../enums/billing-frequency.enum';
import { BillingPeriod } from '../../enums/billing-period.enum';
import { BillingService } from '../services/billing.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientSelectorService } from '@ds/core/ui/menu-wrapper-header/ng-client-selector/client-selector.service';
import { Store } from '@ngrx/store';
import { getUser, UserState } from '@ds/core/users/store/user.reducer';
import { UpdateLastClient, UpdateUser } from '@ds/core/users/store/user.actions';
import { of, Subject } from 'rxjs';
import { coerceNumberProperty } from '@angular/cdk/coercion';
import { environment } from 'apps/ds-company/src/environments/environment';
import { decodeQueryParam } from 'lib/utilties';
import { encodeQueryParam } from 'lib/utilties';
import { ConfirmModalComponent } from '@ds/core/resources/confirm-modal/confirm-modal.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { BillingItem, PendingBillingCredit } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
    selector: 'ds-billing',
    templateUrl: './billing.component.html',
    styleUrls: ['./billing.component.scss'],
})
export class BillingComponent implements OnInit {

    destroy$ = new Subject();

    private _user: UserInfo;
    isBillingLoading: Boolean = true;
    isOneTimeLoading: Boolean = true;
    isCreditLoading: Boolean = true;
    isBillingAdmin: Boolean = false;

    /** Mat Table variables */
    billingSource: any;
    oneTimeSource: any;
    creditSource: any;
    billingColumns = ['Name', 'Price Chart', 'Start Date', 'Frequency', 'Line',
        'Flat', 'Per Qty', 'What To Count', 'Ignore Discount', 'actions'];
    oneTimeColumns = ['Billing Item', 'Year', 'Name', 'Flat', 'Per Qty',
        'What To Count', 'Payroll Applied', 'Ignore Discount', 'actions'];
    creditColumns = ['Name', 'Price Chart', 'Frequency', 'Line', 'Flat', 'Per Qty', 'What To Count', 'isOneTime', 'RequestedBy'];
    billingPaginator: MatPaginator;
    oneTimePaginator: MatPaginator;
    creditPaginator: MatPaginator;


    /** Enum Helpers for readability */
    billingWTCHelper = BillingWhatToCount;
    billingFreqHelper = BillingFrequency;
    billingPeriodHelper = BillingPeriod;


    /** Setting Paginator Dynamically */
    @ViewChild('billingPaginator', { static: false }) set matBillingPaginator(mp: MatPaginator) {
        this.billingPaginator = mp;
        this.setBillingDataSourceAttr();
    }
    @ViewChild('oneTimePaginator', { static: false }) set matOneTimePaginator(mp: MatPaginator) {
        this.oneTimePaginator = mp;
        this.setOneTimeDataSourceAttr();
    }

    @ViewChild('creditPaginator', { static: false }) set matCreditPaginator(mp: MatPaginator) {
        this.creditPaginator = mp;
        this.setCreditDataSourceAttr();
    }

    blocker = false;

    constructor(
        private accountService: AccountService,
        private billingService: BillingService,
        private ngxMsgSvc: NgxMessageService,
        private dialog: MatDialog,
        private route: ActivatedRoute,
        private router: Router,
        private clientSelectorSvc: ClientSelectorService,
        private store: Store<UserState>
    ) {
        this.billingService.billingItems$.pipe().subscribe(res => {
            this.billingSource = new MatTableDataSource<BillingItem>(res);
            this.isBillingLoading = false;
        });
        this.billingService.oneTimeItems$.pipe().subscribe(res => {
            this.oneTimeSource = new MatTableDataSource<BillingItem>(res);
            this.isOneTimeLoading = false;
        });
        this.billingService.pendingCreditItems$.pipe().subscribe(res => {
            this.creditSource = new MatTableDataSource<PendingBillingCredit>(res);
            this.isCreditLoading = false;
        });
    }

    ngOnInit() {
        const clientQueryParamHashed = this.route.snapshot.queryParams['clientid'];
        let clientQueryParam = null;
        if (clientQueryParamHashed != null && clientQueryParamHashed != '')
            clientQueryParam = coerceNumberProperty(decodeQueryParam(clientQueryParamHashed));

        this.store.select(getUser)
            .pipe(
                takeUntil(this.destroy$),
                filter(user => user != null),
                switchMap(u => {
                    this._user = u;

                    this.isBillingAdmin = this._user.isBillingAdmin;
                    if (this.isBillingAdmin && (this.creditColumns.findIndex((c) => c == 'status' || c == 'edit') == -1) )
                        this.creditColumns.push('status', 'edit');

                    if (clientQueryParam > 0 && clientQueryParam !== this._user.lastClientId) {
                        return this.clientSelectorSvc.changeClient(clientQueryParam);
                    } else {
                        this.billingService.fetchBillingItems(u.lastClientId);
                        this.billingService.fetchOneTimeItems(u.lastClientId);
                        this.billingService.fetchPendingItems(u.lastClientId);
                        this.billingService.fetchFakeBillingResolver();
                        return of(null);
                    }
                }),
                tap(user => {
                    if (user) this.store.dispatch(new UpdateLastClient(user));
                })
            ).subscribe();

    } // end of on init

    openModal(billingType: number, billingItem?: BillingItem | PendingBillingCredit) {
        let isPending = false;
        if (billingType == 2) {
            isPending = true;
            billingType = billingItem.isOneTime ? 1 : 0;
        }
        this.dialog.open(BillingDialogComponent, {
            width: "600px",
            data: {
                billingType: billingType,
                billingItem: billingItem,
                isPending: isPending
            }
        });
    }

    confirmDeletion(updater: number, id: number) {
        const confirmDialogRef = this.dialog.open(ConfirmModalComponent, {
            width: "350px",
            data: {
                displayText: 'Are you sure you want to delete this billing item?',
                confirmButtonText: 'Delete',
                cancelButtonText: 'Cancel',
                swapOkClose: true
            }
        });

        confirmDialogRef.afterClosed().subscribe((confirmed: boolean) => {
            if (confirmed) {
                this.delete(updater, id);
            }
        });
    }

    delete(updater: number, billingItemId: number) {
        this.billingService.deleteBillingItem(billingItemId).subscribe(() => {
            this.ngxMsgSvc.setSuccessMessage("Billing item deleted successfully.");
            this.updateItems(updater);
        }, (error: HttpErrorResponse) => {
            this.ngxMsgSvc.setErrorResponse(error);
        });
    }

    updateItems(type: number) {
        if (type) this.billingService.fetchOneTimeItems(this._user.clientId);
        else this.billingService.fetchBillingItems(this._user.clientId);
    }

    setBillingDataSourceAttr() {
        if (this.billingSource) this.billingSource.paginator = this.billingPaginator;
    }
    setOneTimeDataSourceAttr() {
        if (this.oneTimeSource) this.oneTimeSource.paginator = this.oneTimePaginator;
    }
    setCreditDataSourceAttr() {
        if (this.creditSource) this.creditSource.paginator = this.creditPaginator;
    }

    transferPendingBillingCredit(isApproved: boolean, item: PendingBillingCredit) {
        item.isApproved = isApproved;
        item.needsApproval = false;

        this.billingService.transferPendingBillingCredit(item).subscribe(() => {
            const msg = "Pending credit " + ((isApproved) ? "approved " : "declined ") + "successfully.";
            this.billingService.fetchBillingItems(this._user.clientId);
            this.billingService.fetchOneTimeItems(this._user.clientId);
            this.billingService.fetchPendingItems(this._user.clientId);
            this.ngxMsgSvc.setSuccessMessage(msg);
        }, (error: HttpErrorResponse) => {
            this.ngxMsgSvc.setErrorResponse(error);
        });
    }

}
