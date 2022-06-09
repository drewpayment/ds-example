import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormBuilder, FormControl, FormArray, Validators } from '@angular/forms';
import { ArDeposit } from '../shared/ar-deposit.model';
import { BehaviorSubject, Observable, Subject, throwError } from 'rxjs';
import { ArService } from '../shared/ar.service';
import { IArPayment } from '../shared/ar-payment.model';
import { tap, catchError, map } from 'rxjs/operators';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';

@Component({
    selector: 'ds-edit-dialog-modal',
    templateUrl: './edit-posting-dialog.component.html',
    styleUrls: ['./edit-posting-dialog.component.scss']
})
export class EditPostingDialogComponent implements OnInit {
    isLoading$: Observable<boolean>;
    isLoading: boolean = true;
    postings: ArDeposit[] = [];
    postingsDatasource: BehaviorSubject<ArDeposit[]> = new BehaviorSubject(null);
    postingsColumns: string[] = ['arDepositId', 'type', 'createdDate', 'createdByUsername', 'postedDate', 'postedByUsername', 'total'];
    payments: IArPayment[] = [];
    paymentsDatasource: BehaviorSubject<IArPayment[]> = new BehaviorSubject(null);
    paymentsColumns: string[] = ['clientCode', 'clientName', 'invoiceNum', 'invoiceDate', 'checkNumber', 'amount', 'isNsf', 'paymentDate', 'delete'];
    formGroup: FormGroup;
    get paymentsFormArray(): FormArray {
        return this.formGroup.get('arpayments') as FormArray
    }
    get PostingEndDate() { return this.formGroup.controls.postedDate as FormControl; }

    constructor(private dialogRef: MatDialogRef<EditPostingDialogComponent>, private formBuilder: FormBuilder,
        @Inject(MAT_DIALOG_DATA) private data: any, private arService: ArService, private msg: DsMsgService) { }

    ngOnInit() {
        this.createForm(this.data.posting);
        this.postings.push(this.data.posting as ArDeposit);

        this.isLoading$ = this.arService.getPaymentsByDepositId(this.data.posting.arDepositId).pipe(
            catchError((error) => {
                this.msg.clearMessage;
                this.msg.setTemporaryMessage(error.error.errors[0].msg, MessageTypes.error, 120000);
                return throwError(error);
            }),
            map((result) => {
                for (let item of result) {
                    this.payments.push(item);
                }
                this.formGroup.setControl('arpayments', this.createPaymentsFormArray(result));
                this.isLoading = false;
                return false;
            }),
            tap(() => {
                this.paymentsDatasource.next(this.payments);
                this.postingsDatasource.next(this.postings);
            })
        );
    }

    close(): void {
        this.dialogRef.close(null);
    }

    saveForm(): void {
        if(!this.postings[0].postedDate){
            return;
        }

        if (this.formGroup.invalid) return;

        this.postings[0].arPayments = this.payments;

        this.arService.updateDeposit(this.formGroup.value).pipe(
            catchError((error) => {
                this.msg.setTemporaryMessage('Sorry, this operation failed: \'Update Deposit\'', MessageTypes.error, 6000);
                return throwError(error);
            }),
            tap(x => {
                this.data.posting.postedDate = x.postedDate;
                this.close();
                this.msg.setTemporaryMessage('Changes to deposit Number ' + x.arDepositId + ' saved successfully', MessageTypes.success, 3000);
            })
        ).subscribe();
    }

    private createPaymentsFormArray(payments: IArPayment[]): FormArray {
        var result: FormArray = this.formBuilder.array([]);
        payments.forEach(p => {
            result.push(this.formBuilder.group({
                arPaymentId: this.formBuilder.control(p.arPaymentId || '', [Validators.required]),
                clientId: this.formBuilder.control(p.clientId),
                genBillingHistoryId: this.formBuilder.control(p.genBillingHistoryId),
                manualInvoiceId: this.formBuilder.control(p.manualInvoiceId),
                arDepositId: this.formBuilder.control(p.arDepositId),
                invoiceNum: this.formBuilder.control(p.invoiceNum),
                paymentDate: this.formBuilder.control(convertToMoment(p.paymentDate) || '', [Validators.required]),
                amount: this.formBuilder.control(p.amount),
                memo: this.formBuilder.control(p.memo),
                isCredit: this.formBuilder.control(p.isCredit),
                postedBy: this.formBuilder.control(p.postedBy),
                isNsf: this.formBuilder.control(p.isNsf),
                markedNsfBy: this.formBuilder.control(p.markedNsfBy),
                markedNsfDate: this.formBuilder.control(p.markedNsfDate)
            }));
        });

        return result;
    }

    private createForm(posting: any): void {
        this.formGroup = this.formBuilder.group({
            arDepositId: this.formBuilder.control(posting.arDepositId),
            type: this.formBuilder.control(posting.type),
            createdDate: this.formBuilder.control(posting.createdDate),
            createdBy: this.formBuilder.control(posting.createdBy),
            createdByUsername: this.formBuilder.control(posting.createdByUsername),
            total: this.formBuilder.control(posting.total),
            postedDate: this.formBuilder.control(posting.postedDate, [Validators.required]),
            postedBy: this.formBuilder.control(posting.postedBy),
            postedByUsername: this.formBuilder.control(posting.postedByUsername),
            arpayments: this.formBuilder.array([])
        })
    }

    deletePayment(payment: IArPayment){
        if(this.postings[0].postedDate){
            return;
        }

        this.msg.sending(true);

        const index = this.payments.map(e => e.arPaymentId).indexOf(payment.arPaymentId);

        this.arService.deletePayment(payment).pipe(
            catchError((error) => {
                this.msg.clearMessage;
                this.msg.setTemporaryMessage(error.error.errors[0].msg, MessageTypes.error, 120000);
                return throwError(error);
            })
        )
        .subscribe(result => {
            if (index > -1) {
                this.payments.splice(index, 1);
                this.paymentsDatasource.next(this.payments);
                this.data.posting.total -= payment.amount;
                this.msg.setTemporaryMessage(payment.invoiceNum ? 'Deleted payment for invoice number ' + payment.invoiceNum : "Deleted payment", MessageTypes.success, 3000);
            }
        })
    }


}
