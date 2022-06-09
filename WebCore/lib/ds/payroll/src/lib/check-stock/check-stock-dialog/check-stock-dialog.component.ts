import { Component, OnInit, Inject, ChangeDetectorRef }                     from '@angular/core';
import { ICheckStockBilling, ICheckStockSupply, ICheckStockOrder } from '@ds/payroll/shared';
import { CheckStockService }                     from '@ds/payroll/shared/check-stock.service';
import { CheckStockType }                        from '@ds/payroll/shared/check-stock-type.model';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ICheckStockResult } from './check-stock-dialog-result.model';
import { ICheckStockDialogData } from './check-stock-dialog-data.model';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { NgForm, FormBuilder, FormArray, FormGroup, Validators, ValidationErrors, ValidatorFn, Form } from '@angular/forms';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { forkJoin } from 'rxjs';
import { startWith } from 'rxjs/operators';
import { MatTableDataSource } from '@angular/material/table';

@Component({
    selector    : 'ds-check-stock-dialog',
    templateUrl : './check-stock-dialog.component.html',
    styleUrls   : ['./check-stock-dialog.component.scss']
})
export class CheckStockDialogComponent implements OnInit { //,AfterContentChecked

    supplyList : ICheckStockSupply[] = 
    [
        { name: "Checks"          , id: CheckStockType.Checks         , hasCheckNumber: true , checkNumber:null, include:false, billingInfo:null, selectedBilling:null }, 
        { name: "Check Envelopes" , id: CheckStockType.CheckEnvelopes , hasCheckNumber: false, checkNumber:null, include:false, billingInfo:null, selectedBilling:null },  
        { name: "W-2 Envelopes"   , id: CheckStockType.W2Envelopes    , hasCheckNumber: false, checkNumber:null, include:false, billingInfo:null, selectedBilling:null },  
        { name: "ACA Envelopes"   , id: CheckStockType.ACAEnvelopes   , hasCheckNumber: false, checkNumber:null, include:false, billingInfo:null, selectedBilling:null },
        { name: "Delivery"        , id: CheckStockType.Delivery       , hasCheckNumber: false, checkNumber:null, include:true,  billingInfo:null, selectedBilling:null }
    ];
    displayedColumns      : string[] = ['include', 'product', 'quantity', 'amount'];
    supplyMatList         : any;
    checkInfoList         : ICheckStockBilling[];
    checkEnvelopeInfoList : ICheckStockBilling[];
    w2EnvelopeInfoList    : ICheckStockBilling[];
    acaEnvelopeInfoList   : ICheckStockBilling[];
    nextCheckNumber       : number = 0;
    deliveryInfo          : ICheckStockBilling;
    defaultPrice          : number = 0;
    deliveryPrice         : number = 0;
    totalPrice            : number = 0;
    includeAll            : boolean = false;
    itemSelected          : boolean = false;
    checkStockOrder       : ICheckStockOrder = {nextCheckNumber:0,totalChecks:0,checkEnvelopes:0,w2Envelopes:0,acaEnvelopes:0,isDelivery:false};
    isLoading             : boolean = true;
    formSubmitted         : boolean = false;

    //reactive form setup
    form: FormGroup = this.createForm();

    constructor(
        private dialogRef: MatDialogRef<CheckStockDialogComponent, ICheckStockResult>, 
        @Inject(MAT_DIALOG_DATA) 
        private dialogData: ICheckStockDialogData,
        private api: CheckStockService,
        private msg:DsMsgService,
        private fb: FormBuilder) { }

    ngOnInit() {

        forkJoin(
            this.api.getCheckStockPrice(),
            this.api.getCheckNumber()
        ).subscribe(([billing,checkNumberObject]) => {
            (<FormArray>this.form.controls.items).controls.forEach(fg => {
                if(fg.value.id == CheckStockType.Checks)
                    fg.patchValue({
                        hasCheckNumber : true,
                        checkNumber    : checkNumberObject.nextCheckNumber,
                        quantity       : billing.filter((bill) => bill.checkStockTypeId == fg.value.id),
                        
                    });
                else if (fg.value.id == CheckStockType.Delivery) {
                    fg.patchValue({
                        amount : billing.filter((bill) => bill.checkStockTypeId == fg.value.id)[0].price
                    });
                    this.deliveryPrice = billing.filter((bill) => bill.checkStockTypeId == fg.value.id)[0].price;
                    this.totalPrice    = this.deliveryPrice;
                }
                else
                    fg.patchValue({
                        quantity: billing.filter((bill) => bill.checkStockTypeId == fg.value.id)
                    });
            });
            this.supplyMatList = new MatTableDataSource<ICheckStockSupply>(this.form.controls.items.value);
            this.isLoading     = false;
        });

    }

    createForm() {
        return this.fb.group({
            items: this.createFormArray()
        });
    }

    createFormArray() {
        var result = this.fb.array([]);
        this.supplyList.forEach((element:ICheckStockSupply, i) => {
            let row = this.fb.group({
                id               : this.fb.control(element.id),
                name             : this.fb.control(element.name),
                hasCheckNumber   : this.fb.control(element.hasCheckNumber),
                include          : this.fb.control(element.include),
                product          : this.fb.control(element.name),
                checkNumber      : this.fb.control({value: element.checkNumber, enabled: element.hasCheckNumber}, element.hasCheckNumber ? [Validators.required, Validators.pattern(/^[0-9]{1,9}$/)] : []),
                quantity         : this.fb.control(null),
                selectedQuantity : this.fb.control(null),
                amount           : this.fb.control(0)
            });
            row.get('include').valueChanges.subscribe(val => {
                let quantity = row.get("selectedQuantity");
                let id       = row.get("id");
                if(val && id.value != CheckStockType.Delivery)
                    quantity.setValidators([Validators.required]);
                else    
                    quantity.clearValidators();

                quantity.updateValueAndValidity();
                this.recalculate();
            })
            result.push(row);
        });
        return result;
    }

    checkAll() {
        this.includeAll = !this.includeAll;
        (<FormArray>this.form.controls.items).controls.forEach((element : FormGroup) => {
            element.controls.include.setValue(this.includeAll);
        });

        this.recalculate();
    }

    setAmount(i:number) {
        if(this.form.get(['items',i,'selectedQuantity']).value)
            this.form.get(['items',i,'amount']).setValue(this.form.get(['items',i,'selectedQuantity']).value.price);  
        else
            this.form.get(['items',i,'amount']).setValue(0);  
        this.recalculate();
    }

    recalculate() {
        var itemCount   = 0;
        var total       = this.form.get(['items']).value.reduce((t,v) =>  v.include ?  t + v.amount : t, 0);
        this.totalPrice = total;
        (<FormArray>this.form.controls.items).controls.forEach(fg => {
            if (fg.value.id != CheckStockType.Delivery && fg.value.include) {
                itemCount = itemCount + 1;
            }
        });
        if(itemCount > 0)
            this.itemSelected = true;
        else 
            this.itemSelected = false;
    }

    cancel() {
        this.dialogRef.close(null);
    }

    save() {
        this.formSubmitted = true;
        if(this.form.invalid) {
            return;
        }

        this.prepareCheckStockOrder();

        this.api.createCheckStockOrder(this.checkStockOrder).subscribe((order : ICheckStockOrder) => {
            if(order.isDelivery)
                this.msg.setTemporaryMessage('Your order has been submitted successfully and will be shipped within two business days. If you have any questions please contact us at (616)-248-3835, Thank you!', MessageTypes.success, 15000);
            else
                this.msg.setTemporaryMessage('Your order has been submitted successfully and will be printed as soon as possible. If you have any questions please contact us at (616)-248-3835, Thank you!', MessageTypes.success, 15000);
            this.dialogRef.close(null);
        });
    }

    prepareCheckStockOrder() {
        (<FormArray>this.form.controls.items).controls.forEach(fg => {
            if (fg.value.id == CheckStockType.Checks) {
                this.checkStockOrder.nextCheckNumber = fg.value.checkNumber;
                this.checkStockOrder.totalChecks     = (fg.value.selectedQuantity != null) ? fg.value.selectedQuantity.quantity : 0;
            } else if (fg.value.id == CheckStockType.CheckEnvelopes) {
                this.checkStockOrder.checkEnvelopes  = (fg.value.selectedQuantity != null) ? fg.value.selectedQuantity.quantity : 0;
            } else if (fg.value.id == CheckStockType.W2Envelopes) {
                this.checkStockOrder.w2Envelopes     = (fg.value.selectedQuantity != null) ? fg.value.selectedQuantity.quantity : 0;
            } else if (fg.value.id == CheckStockType.ACAEnvelopes) {
                this.checkStockOrder.acaEnvelopes    = (fg.value.selectedQuantity != null) ? fg.value.selectedQuantity.quantity : 0;
            } else if (fg.value.id == CheckStockType.Delivery) {
                this.checkStockOrder.isDelivery      = fg.value.include;
            }
        });
    }

}
