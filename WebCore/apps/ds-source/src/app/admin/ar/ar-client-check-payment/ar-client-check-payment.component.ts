import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { ChangeDetectionStrategy, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { merge, Observable, Subject, throwError } from 'rxjs';
import { catchError, switchMap, takeUntil, tap } from 'rxjs/operators';
import { IArClientCheckPayment, PaymentType } from '../shared/ar-client-check-payment.model';
import { ArDeposit } from '../shared/ar-deposit.model';
import { ArService } from '../shared/ar.service';
import { IOption } from '../shared/option.model';

@Component({
    selector: 'ds-ar-client-check-payment',
    templateUrl: './ar-client-check-payment.component.html',
    styleUrls: ['./ar-client-check-payment.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ArClientCheckPaymentComponent implements OnInit, OnDestroy {
    createForm(): FormGroup {
        return this.formBuilder.group({
            client: this.formBuilder.control('').setValidators([Validators.required]),
            checkPayments: this.formBuilder.array([])
        })
    }
    clientCheckPaymentFormGroup: FormGroup = this.createForm();
    params$: Observable<any>;
    depositId: string;
    deposit: ArDeposit;
    options: IOption[] = [];
    selectedClient: number;
    arClientCheckPaymentsLoaded: boolean = false;
    arDominionCheckPayments: IArClientCheckPayment[] = [];
    matTableDatasource = new MatTableDataSource<IArClientCheckPayment>([]);
    formSubmitted = false;
    destroy = new Subject();
    invoicesFiltered = false;
    isClientSelectorEmpty = false;
    displayedColumns: string[] = ['invoiceNumber', 'invoiceDate', 'paymentDate', 'invoiceAmount', 'prevPaidAmount', 'paymentType', 'paymentAmount', 'memo', 'isPaid'];
    paymentTypeDropdownList = [
        {
            designation: 'Full',
            value: '0'
        },
        {
            designation: 'Partial',
            value: '1'
        }
    ];
    get checkPaymentsFormArray(): FormArray {
        return this.clientCheckPaymentFormGroup.get('checkPayments') as FormArray
    }

    constructor(private arService: ArService, private msg: DsMsgService, private route: ActivatedRoute, private formBuilder: FormBuilder,
        private cdr: ChangeDetectorRef) {}

    ngOnInit() {
        this.arClientCheckPaymentsLoaded = true;

        this.params$ = merge(
            this.route.params.pipe(
                tap(params => {
                    this.depositId = params['id'];
                }),
                switchMap(x => {
                    return this.arService.getArDepositById(x['id']).pipe(
                        tap(x => {
                            this.deposit = x;
                        } )
                    )
                })
            ),
            this.arService.getClientList().pipe(
                tap(x =>{
                    for (let item of x) {
                        this.options.push(item);
                    }
                })
            )
        );

        this.clientCheckPaymentFormGroup.get('client').valueChanges.pipe(takeUntil(this.destroy),tap(x => this.updateSelectedClient(x))).subscribe();
    }

    ngOnDestroy() {
        this.destroy.next();
    }

    updateSelectedClient(client: IOption) {
        if (client) {
            // Review
            this.isClientSelectorEmpty = false;
            this.selectedClient = client.id;
        }
    }

    openInvoicesClicked(){
        // Review
        if (!this.selectedClient) {
            this.isClientSelectorEmpty = true;
            return
        }
        this.clientCheckPaymentFormGroup.markAsUntouched();
        this.loadInvoices().subscribe();
    }

    loadInvoices(){
        if (this.clientCheckPaymentFormGroup.invalid) return;

        this.arClientCheckPaymentsLoaded = false;
        this.arDominionCheckPayments = [];
        this.matTableDatasource.data = this.arDominionCheckPayments;

        return this.arService.getUnpaidInvoicesByClientId(this.selectedClient).pipe(
            catchError((error) => {
                this.msg.setTemporaryMessage('Sorry, this operation failed: \'Get Unpaid Invoices\'', MessageTypes.error, 60000);
                this.arClientCheckPaymentsLoaded = true;
                return throwError(error);
            }),
            tap(x => {
                this.arDominionCheckPayments = x;
                this.clientCheckPaymentFormGroup.setControl('checkPayments', this.createCheckPaymentsFormArray(x));
                this.matTableDatasource.data = x;
                this.arClientCheckPaymentsLoaded = true;
                this.invoicesFiltered = true;
                this.cdr.detectChanges();
            })
        );
    }

    private createCheckPaymentsFormArray(checkPayments: IArClientCheckPayment[]): FormArray {
        var result: FormArray = this.formBuilder.array([]);
        checkPayments.forEach(p => {
            result.push(this.formBuilder.group({
                invoiceAmount: this.formBuilder.control(p.invoiceAmount),
                invoiceDate: this.formBuilder.control(p.invoiceDate),
                invoiceId: this.formBuilder.control(p.invoiceId),
                invoiceNumber: this.formBuilder.control(p.invoiceNumber),
                isManualInvoice: this.formBuilder.control(p.isManualInvoice),
                clientId: this.formBuilder.control(p.clientId),
                clientCode: this.formBuilder.control(p.clientCode),
                clientName: this.formBuilder.control(p.clientName),
                prevPaidAmount: this.formBuilder.control(p.prevPaidAmount),
                paymentType: this.formBuilder.control(p.paymentType),
                paymentDate: this.formBuilder.control(p.paymentDate),
                paymentAmount: this.formBuilder.control({value: "", disabled: true}),
                memo: this.formBuilder.control({value: p.memo, disabled: true}),
                isPaid: this.formBuilder.control({value: p.isPaid, disabled: true})
            }, {validators: [checkPaymentDateInvalid, checkPaymentAmountInvalid]}));
        });

        return result;
    }

    save() {
        this.clientCheckPaymentFormGroup.markAllAsTouched();

        if(this.clientCheckPaymentFormGroup.invalid) return;

        let filteredCheckPayments = this.clientCheckPaymentFormGroup.getRawValue().checkPayments
        .filter(function (x) { return x.paymentType == PaymentType.Full && x.isPaid || x.paymentType == PaymentType.Partial && x.paymentAmount >= 0 || x.paymentType == PaymentType.Partial && x.isPaid});

        if(filteredCheckPayments.length == 0){
            this.msg.setTemporaryMessage('No payments sent.', MessageTypes.success, 3000);
            return;
        }

        this.formSubmitted = true;

        this.arService.saveClientCheckPayments(this.depositId, this.clientCheckPaymentFormGroup.getRawValue().checkPayments).pipe(
            catchError((error) => {
                this.msg.setTemporaryMessage(error.error.errors[0].msg, MessageTypes.error, 5000);
                return throwError(error);
            }),
            tap(result => {
                this.msg.setTemporaryMessage('Check payments saved successfully', MessageTypes.success, 5000);
                this.openInvoicesClicked();
            })
        ).subscribe();
    }

    paymentTypeChanged(index: number){
        let paymentType = this.checkPaymentsFormArray.at(index).get('paymentType');
        let invoiceAmountFormControl = this.checkPaymentsFormArray.at(index).get('invoiceAmount');
        let prevPaidAmountFormControl = this.checkPaymentsFormArray.at(index).get('prevPaidAmount');
        let paymentAmountFormControl = this.checkPaymentsFormArray.at(index).get('paymentAmount');
        let isPaidFormControl = this.checkPaymentsFormArray.at(index).get('isPaid');
        let memoFormControl = this.checkPaymentsFormArray.at(index).get('memo');

        this.clientCheckPaymentFormGroup.markAsUntouched();

        if(paymentType.value == PaymentType.Full){
            paymentAmountFormControl.disable();
            isPaidFormControl.disable();
            memoFormControl.enable();
            isPaidFormControl.setValue(true);
            paymentAmountFormControl.setValue((invoiceAmountFormControl.value - prevPaidAmountFormControl.value).toFixed(2));
        } else if(paymentType.value == PaymentType.Partial) {
            isPaidFormControl.setValue(false);
            paymentAmountFormControl.enable();
            isPaidFormControl.enable();
            memoFormControl.enable();
            paymentAmountFormControl.setValue("");
        } else {

            paymentAmountFormControl.disable();
            isPaidFormControl.disable();
            memoFormControl.disable();
        }
    }
}

export const checkPaymentDateInvalid: ValidatorFn = (control: FormGroup): ValidationErrors | null => {
    const paymentType = control.get('paymentType');

    if(!paymentType || !paymentType.value){
        return null;
    }

    const paymentDate = control.get('paymentDate');
    return (paymentType && paymentDate && paymentType.value.value && !paymentDate.value) ? { paymentDateInvalid: true } : null;
  };

  export const checkPaymentAmountInvalid: ValidatorFn = (control: FormGroup): ValidationErrors | null => {
    const paymentType = control.get('paymentType');

    if(!paymentType || !paymentType.value){
        return null;
    }

    const paymentAmount = control.get('paymentAmount').value;
    const invoiceAmount = control.get('invoiceAmount').value;
    const prevPaidAmount = control.get('prevPaidAmount').value;
    let remainingInvoiceAmount = invoiceAmount - prevPaidAmount;

    if(paymentAmount > remainingInvoiceAmount){
        return { paymentAmountExceededRemainingInvoiceAmount: true }
    }

    return paymentType
        && paymentType.value == PaymentType.Partial
        && (paymentAmount < 0 || paymentAmount === "" || paymentAmount === undefined)
        ? { paymentAmountInvalid: true }
        : null;
  };

