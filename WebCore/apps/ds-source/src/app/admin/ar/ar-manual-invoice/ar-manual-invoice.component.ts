import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ArService } from '../shared/ar.service';
import { Observable, merge, throwError } from 'rxjs';
import { switchMap, tap, catchError } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators, FormControl, ValidatorFn, ValidationErrors} from '@angular/forms';
import * as moment from 'moment';
import { IArBillingItemDesc } from '../shared/ar-billing-item-desc.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { IArReportParameter } from '../shared/ar-report-parameter.model';
import { ArReportType } from '../shared/ar-report-type';
import { IOption } from '../shared/option.model';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { IArManualInvoice, IArManualInvoiceDetail } from '../shared/ar-manual-invoice.model';

@Component({
    selector: 'ds-ar-manual-invoice',
    templateUrl: './ar-manual-invoice.component.html',
    styleUrls: ['./ar-manual-invoice.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ArManualInvoiceComponent implements OnInit {
    numberPattern = '^[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$';
    disableEditMode = false;

    createForm(): FormGroup {
        return this.formBuilder.group({
            invoice: this.formBuilder.control('').setValidators([Validators.required]),
            client: this.formBuilder.control('').setValidators([Validators.required]),
            billingItems: this.formBuilder.control('').setValidators([Validators.required]),
            billingyears: this.formBuilder.control(''),
            amount: this.formBuilder.control('').setValidators([Validators.required, Validators.pattern(this.numberPattern)])
        })
    }
    manualInvoiceFormGroup: FormGroup = this.createForm();
    billingItemDescriptions: IArBillingItemDesc[] = [];
    billingyears: number[] = [];
    billingYearsVisibleForAdd: boolean = false;
    billingYearsVisibleForEdit: boolean = false;
    billingYearsVisible: boolean = false;
    options: IOption[] = [];
    acaReportingBillingItem: number = 141;
    w2ProcessingBillingItem: number = 95;
    manualInvoice: IArManualInvoice = {
        arManualInvoiceId: 0,
        clientId: 0,
        invoiceNum: '',
        totalAmount: 0,
        invoiceDate: null,
        postedBy: 0,
        isPaid: false,
        dominionVendorOpt: -1,
        invoiceNumAndDate: '',
        manualInvoiceDetails: []
    };
    manualInvoiceDetail$: Observable<IArManualInvoiceDetail[]>;
    manualInvoiceDetail: IArManualInvoiceDetail[];
    selectedClient: number;
    initLists$: Observable<any>;
    changeClient$: Observable<any>;
    manualInvoiceDetailsIsEmpty: boolean = this.isManualInvoiceDetailsEmpty();
    isAdd = true;
    billingItemsElem: any;
    isClientSelectorEmpty = true;

    @ViewChild('focus', { static: false }) nameField: ElementRef;

    get amount() {
        return this.manualInvoiceFormGroup.controls.amount as FormControl;
    }

    get client() {
        return this.manualInvoiceFormGroup.controls.client as FormControl;
    }

    constructor(
        private arService: ArService,
        private formBuilder: FormBuilder,
        private sb: MatSnackBar,
        private msg: DsMsgService,
        private el:ElementRef,
        private changeDetector: ChangeDetectorRef) {
        this.initLists$ = merge(
            this._initList(this.arService.getBillingItems(), this.billingItemDescriptions),
            this._initList(this.arService.getBillingYears(), this.billingyears),
            this._initList(this.arService.getClientList(), this.options)
        );

        this.changeClient$ = this.manualInvoiceFormGroup.get('client').valueChanges.pipe(tap(x => this.updateSelectedClient(x)));
    }

    ngOnInit() {
        this.manualInvoiceFormGroup.controls['invoice'].setValue(moment());
    }

    private _initList<T>(source: Observable<T[]>, target: T[]) {
        return source.pipe(
            tap(data => {
                for (let item of data) {
                    target.push(item);
                }
            })
        )
    }

    displayFn(client?: IOption): string | undefined {
        return client ? client.filter : undefined;
    }

    formValueCache: { [key: string]: any; value: any } = {} as { [key: string]: any; value: any };

    changeBillingItems() {

        var formControl = this.manualInvoiceFormGroup.get("billingItems");
        var selectedBillingItem = formControl.value;

        if (selectedBillingItem == this.acaReportingBillingItem || selectedBillingItem == this.w2ProcessingBillingItem) {
            this.manualInvoiceFormGroup.get('billingyears').setValue(this.billingyears[0]);
            this.billingYearsVisibleForAdd = true;
        } else {
            this.billingYearsVisibleForAdd = false;
        }

    }

    addManualInvoiceDetail() {
        this.manualInvoiceFormGroup.markAllAsTouched();

        if (this.manualInvoiceFormGroup.invalid || this.client.errors || this.client.value.id == 0) return;

        this.isAdd = false;

        let billingItem = this.manualInvoiceFormGroup.get('billingItems');

        var manualInvoiceDetail: IArManualInvoiceDetail = {
            amount: this.manualInvoiceFormGroup.get('amount').value,
            billingYear: !this.billingYearsVisibleForAdd ? null : this.manualInvoiceFormGroup.get('billingyears').value,
            itemCode: this.billingItemDescriptions.find(x => x.billingItemDescriptionId == billingItem.value).code,
            itemDescription: this.billingItemDescriptions.find(x => x.billingItemDescriptionId == billingItem.value).description,
            arManualInvoiceDetailId: 0,
            arManualInvoiceId: 0,
            action: "edit",
            isEdit: false
        };

        if(!this.billingYearsVisible){
            if(billingItem.value == this.acaReportingBillingItem ||
                billingItem.value == this.w2ProcessingBillingItem)
            {
                    this.billingYearsVisible = true;
            }
            else
            {
                for(let item of this.manualInvoice.manualInvoiceDetails)
                {
                    this.billingYearsVisible = this.showBillingYear(item);
                    if(this.billingYearsVisible)
                        break;
                }
            }
        }

        // var manualInvoiceDetail: IArManualInvoiceDetail = {
        //     amount: this.manualInvoiceFormGroup.get('amount').value,
        //     billingYear: this.manualInvoiceFormGroup.get('billingyears').value,
        //     itemCode: this.billingItemDescriptions.find(x => x.billingItemDescriptionId == this.manualInvoiceFormGroup.get('billingItems').value).code,
        //     itemDescription: this.billingItemDescriptions.find(x => x.billingItemDescriptionId == this.manualInvoiceFormGroup.get('billingItems').value).description,
        //     arManualInvoiceDetailId: 0,
        //     arManualInvoiceId: 0,
        //     action: "edit",
        //     isEdit: false
        // };

        this.manualInvoice.manualInvoiceDetails.push(manualInvoiceDetail);
        if(this.manualInvoice.manualInvoiceDetails.length){
            this.manualInvoiceDetailsIsEmpty = this.isManualInvoiceDetailsEmpty();
        } else {
            this.manualInvoiceDetailsIsEmpty = this.isManualInvoiceDetailsEmpty();
        }

        this.resetBillingItems();
        this.disableEditMode = false;
    }

    showBillingYear(item: IArManualInvoiceDetail){
        let billingItemDescId = this.billingItemDescriptions.find(x => x.description == item.itemDescription).billingItemDescriptionId
                return billingItemDescId == this.acaReportingBillingItem || billingItemDescId == this.w2ProcessingBillingItem
    }

    save() {
        this.manualInvoiceFormGroup.markAllAsTouched();
        if (this.isManualInvoiceDetailsEmpty() || this.client.errors || this.client.value.id == 0 || this.disableEditMode || this.isAdd) {
            return;
        }
        //this.manualInvoice.manualInvoiceDetails.isEdit = false;
        this.isAdd = false;
        this.msg.sending(true);
        this.manualInvoice.clientId = this.selectedClient;
        this.manualInvoice.invoiceDate = this.manualInvoiceFormGroup.get('invoice').value;

        this.arService.saveManualInvoice(this.manualInvoice)
            .pipe(
                catchError((error) => {
                    this.msg.clearMessage;
                    this.msg.setTemporaryMessage(error.error.errors[0].msg, MessageTypes.error, 15000);
                    return throwError(error);
                }),
                switchMap(result => {

                    var reportParams: IArReportParameter =
                    {
                        arReportId: ArReportType.ManualBillingInvoice,
                        fileFormat: "PDF",
                        clientId: this.selectedClient,
                        invoiceId: result.arManualInvoiceId,
                        arDepositId: 0,
                        startDate: null,
                        endDate: null,
                        billingItemCode: null,
                        agingDate: null,
                        agingPeriod: null,
                        reportType: ArReportType.ManualBillingInvoice,
                        lookBackDate: null,
                        gainsLossesToggle: null,
                        orderBy: null,
                        payrollId: null
                    };

                    return this.arService.generateReport(reportParams, false);
                })
            )
            .subscribe(result => {
                this.clearGrid();
                this.changeDetector.detectChanges();
                this.msg.setTemporaryMessage("Manual Invoice submitted successfully", MessageTypes.success, 5000);
            }),
            (error) => this.sb.open(error.error.errors[0].msg, 'dismiss', { duration: 60000 });
    }

    isManualInvoiceDetailsEmpty() {
        return !Array.isArray(this.manualInvoice.manualInvoiceDetails) || !this.manualInvoice.manualInvoiceDetails.length;
    }

    calculateTotal() {
        if (this.manualInvoice.manualInvoiceDetails === undefined || this.manualInvoice.manualInvoiceDetails.length == 0) {
            return 0;
        }

        return this.manualInvoice.manualInvoiceDetails.map(o => o.amount).reduce((a, c) => Number(a) + Number(c));
    }

    updateSelectedClient(client: IOption) {
        if (client && client.id && client.id !== 0) {
            this.isClientSelectorEmpty = false;
            this.selectedClient = client.id;
        }
    }

    saveRow(row: IArManualInvoiceDetail): void {
        this.manualInvoiceFormGroup.markAsUntouched();
        if (this.amount.invalid) return;
        const adjustedRow = row;
        adjustedRow.amount = this.manualInvoiceFormGroup.get('amount').value;
        adjustedRow.billingYear = !this.billingYearsVisibleForEdit ? null : this.manualInvoiceFormGroup.get('billingyears').value;
    }

    doneEditing(row: IArManualInvoiceDetail) {
        this.saveRow(row);
        row.isEdit = false;
        this.disableEditMode = false;
        this.resetBillingItems();
    }


    deleteRow(row: IArManualInvoiceDetail) {
        const index = this.manualInvoice.manualInvoiceDetails.indexOf(row);
        this.manualInvoice.manualInvoiceDetails.splice(index, 1);
        if(this.manualInvoice.manualInvoiceDetails.length){
            this.manualInvoiceDetailsIsEmpty = false;
        } else {
            this.manualInvoiceDetailsIsEmpty = true;
        }
        this.disableEditMode = false;

        let result = false;
        for(let manualInvoiceDetail of this.manualInvoice.manualInvoiceDetails){
            result = this.showBillingYear(manualInvoiceDetail)

            if(result){
                break;
            }
        }
        this.billingYearsVisible = result;
    }

    editRow(row: IArManualInvoiceDetail) {
        row.isEdit = true;
        this.disableEditMode = true;
        this.billingYearsVisibleForEdit = this.showBillingYear(row);
        this.manualInvoiceFormGroup.controls['amount'].setValue(row.amount);

        if(this.billingYearsVisibleForEdit){
            this.manualInvoiceFormGroup.controls['billingyears'].enable;
            this.manualInvoiceFormGroup.controls['billingyears'].setValue(row.billingYear);
        }
        else{
            this.manualInvoiceFormGroup.controls['billingyears'].disable;
        }
    }

    removeRow() {
        this.disableEditMode = false;
        this.resetBillingItems();
        this.isAdd = false;
    }

    clearGrid() {
        this.manualInvoice.manualInvoiceDetails = [];
        this.manualInvoiceDetailsIsEmpty = true;
    }

    addItem() {
        this.disableEditMode = true;
        this.isAdd = true;
        this.billingYearsVisibleForAdd = false;
        this.nameField.nativeElement.focus();
    }

    focusField(): void {
        setTimeout(()=>{
            this.nameField.nativeElement.focus();
        },0);
    }

    resetBillingItems() {
        // reset selectable fields for repeating billing items, rather than the whole form
        this.manualInvoiceFormGroup.controls['billingItems'].setValue("");
        this.manualInvoiceFormGroup.controls['billingyears'].setValue("");
        this.manualInvoiceFormGroup.controls['amount'].setValue("");
        this.manualInvoiceFormGroup.controls['billingItems'].markAsUntouched();
        this.manualInvoiceFormGroup.controls['billingyears'].markAsUntouched();
        this.manualInvoiceFormGroup.controls['amount'].markAsUntouched();
    }
}
