import { Component, OnInit, Input, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { ArService } from '../shared/ar.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import * as moment from 'moment';
import { catchError, tap, map, switchMap, takeUntil } from 'rxjs/operators';
import { throwError, Subject, Observable, forkJoin, merge, of } from 'rxjs';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { IArDominionCheckPayment } from '../shared/ar-dominion-check-payment.model';
import { ParamMap, ActivatedRoute, Router } from '@angular/router';
import { ArDeposit } from '../shared/ar-deposit.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';

@Component({
    selector: 'ds-ar-dominion-check-payment',
    templateUrl: './ar-dominion-check-payment.component.html',
    styleUrls: ['./ar-dominion-check-payment.component.scss']
})
export class ArDominionCheckPaymentComponent implements OnInit, AfterViewInit, OnDestroy {

    invoiceFormGroup: FormGroup;
    formSubmitted: boolean;
    deposit: ArDeposit;
    displayedColumns: string[] = ['clientCode', 'invoiceNumber', 'invoiceDate', 'checkNumber', 'checkDate', 'totalAmount', 'isPaid'];

    constructor(private fb: FormBuilder, private arService: ArService, private msg: DsMsgService, private route: ActivatedRoute, private router: Router) {

        this.invoiceFormGroup = fb.group({
            StartDate: fb.control(null),
            EndDate: fb.control(null)
        });
    }

    get StartDate() { return this.invoiceFormGroup.controls.StartDate as FormControl; }
    get EndDate() { return this.invoiceFormGroup.controls.EndDate as FormControl; }
    datasource = new MatTableDataSource<IArDominionCheckPayment>([]);
    @ViewChild(MatPaginator, {static: false}) paginator: MatPaginator;
    arDominionCheckPayments: IArDominionCheckPayment[] = [];
    arDominionCheckPaymentsLoaded: boolean = false;
    params$: Observable<any>;
    depositId: number;
    destroy = new Subject();
    ngOnInit() {
        this.setDefaultDateRange();

        this.params$ = merge(
            this.getOpenInvoices()
        )

        this.arService.getArDepositById$.pipe(
            takeUntil(this.destroy),
            tap((deposit: ArDeposit) => {
                this.deposit = deposit;
                this.depositId = deposit.arDepositId;
            })
        ).subscribe();
    }

    ngOnDestroy(){
        this.destroy.next();
    }

    ngAfterViewInit(){
        this.datasource.paginator = this.paginator;
    }

    setDefaultDateRange() {
        var previousDay = moment().subtract(1, 'day');
        this.StartDate.setValue(previousDay);
        this.EndDate.setValue(previousDay);

        if (previousDay.day() == 0)
            this.StartDate.setValue(moment().subtract(3, 'day'));
    }

    getOpenInvoices() {
        this.arDominionCheckPaymentsLoaded = false;
        this.arDominionCheckPayments = [];
        this.datasource.data = this.arDominionCheckPayments;

        return this.arService.getUnpaidDominionCheckPaymentsByDateRange(this.StartDate.value.format('YYYY-MM-DD'), this.EndDate.value.format('YYYY-MM-DD')).pipe(
            catchError((error) => {
                this.msg.setTemporaryMessage('Sorry, this operation failed: \'Get Unpaid Dominion Check Payments\'', MessageTypes.error, 60000);
                this.arDominionCheckPaymentsLoaded = true;
                return throwError(error);
            }),
            tap(x => {
                this.arDominionCheckPayments = x;
                this.datasource.data = this.arDominionCheckPayments;
                this.arDominionCheckPaymentsLoaded = true;
            })
        );
    }

    showInvoices() {
        this.getOpenInvoices();
        this.getOpenInvoices().subscribe();
    }

    toggleIsPaid(checkPayment: IArDominionCheckPayment){

        // if (this.isManualInvoiceDetailsEmpty()) {
        //     return;
        // }

        this.msg.sending(true);
        // this.manualInvoice.clientId = this.selectedClient;
        // this.manualInvoice.invoiceDate = this.manualInvoiceFormGroup.get('invoice').value;

        if(checkPayment.isPaid){
            this.arService.setCheckPaymentToPaid(this.depositId, checkPayment).pipe(
                catchError((error) => {
                    this.msg.clearMessage;
                    this.msg.setTemporaryMessage(error.error.errors[0].msg, MessageTypes.error, 3000);
                    return throwError(error);
                })
            )
            .subscribe(result => {
                this.refreshDataSourceWithUpdatedCheckPmt(result);
                this.msg.sending(false);
                this.msg.setTemporaryMessage(result.invoiceNumber ? "Invoice #" + result.invoiceNumber + " marked as paid" : "Invoice marked as paid", MessageTypes.success, 3000);
            })
        } else{
            this.arService.setCheckPaymentToUnpaid(this.depositId, checkPayment).pipe(
                catchError((error) => {
                    this.msg.clearMessage;
                    this.msg.setTemporaryMessage(error.error.errors[0].msg, MessageTypes.error, 3000);
                    return throwError(error);
                })
            )
            .subscribe(result => {
                //remove line ?
                this.refreshDataSourceWithUpdatedCheckPmt(result);
                this.msg.sending(false);
                this.msg.setTemporaryMessage(result.invoiceNumber ? "Invoice #" + result.invoiceNumber + " marked as unpaid" : "Invoice marked as unpaid", MessageTypes.success, 3000);
            });
        }
    }

    refreshDataSourceWithUpdatedCheckPmt(dominionCheckPmt: IArDominionCheckPayment) {
        // Find the index of the Dominion Check payment being updated
        var index = this.arDominionCheckPayments.indexOf(this.arDominionCheckPayments.find(x => x.invoiceId === dominionCheckPmt.invoiceId));
        this.arDominionCheckPayments[index] = dominionCheckPmt;     // Update the item in the array
        this.datasource.data = this.arDominionCheckPayments;         // Refresh the data source
    }

    saveButton(){
        this.getOpenInvoices().subscribe(result => {
            this.msg.setTemporaryMessage("Saved!", MessageTypes.success, 3000);
        });
    }

}
