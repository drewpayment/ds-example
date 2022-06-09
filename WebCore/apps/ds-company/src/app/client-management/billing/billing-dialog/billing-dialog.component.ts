import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { tap } from 'rxjs/operators';
import * as moment from 'moment';
import { HttpErrorResponse } from '@angular/common/http';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { BillingWhatToCount } from '../../../enums/billing-what-to-count.enum';
import { BillingFrequency } from '../../../enums/billing-frequency.enum';
import { BillingPeriod } from '../../../enums/billing-period.enum';
import { BillingService } from '../../services/billing.service';
import { Observable } from 'rxjs';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BillingItem, BillingItemBase, BillingItemDescription, BillingPriceChart, PendingBillingCredit } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-billing-dialog',
  templateUrl: './billing-dialog.component.html',
  styleUrls: ['./billing-dialog.component.scss']
})
export class BillingDialogComponent implements OnInit {

  user: UserInfo;
  form: FormGroup = this.createForm();
  formSubmitted: boolean = false;
  isSaving: boolean = false;
  isLoading: boolean = true;

  billingType: number = 0; // 0 for normal, 1 for one time
  billingItemDescription: BillingItemDescription[];
  billingPriceChart: BillingPriceChart[];
  years = [];

  /** Enum Helpers for readability */
  billingWTCHelper = BillingWhatToCount;
  billingWTCList = BillingWhatToCount.getModel();
  billingFreqHelper = BillingFrequency.getModel();
  billingPeriodHelper = BillingPeriod.getModel();

  constructor(
    private billingSvc: BillingService,
    private accountSvc: AccountService,
    private fb: FormBuilder,
    private ngxMsgSvc: NgxMessageService,
    private dialogRef: MatDialogRef<BillingDialogComponent>,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) { }

  ngOnInit() {
    this.accountSvc.getUserInfo().pipe(tap(u => this.user = u)).subscribe(() => {

      this.billingSvc.data$.subscribe(data => {
        if (data == null) return;

        this.billingItemDescription = data[0].sort((x, y) => (x.description > y.description) ? 1 : -1);
        this.billingPriceChart = data[1].sort((x, y) => (x.description > y.description) ? 1 : -1);
        this.billingType = this.data.billingType;

        let dateStart = moment().subtract(1, 'y');
        const dateEnd = moment();
        while (dateEnd.diff(dateStart, 'years') >= 0) {
          this.years.push(dateStart.format('YYYY'));
          dateStart.add(1, 'year');
        };

        this.years.sort((x, y) => (+x > +y) ? -1 : 1);

        if (this.data.billingItem) {
          if (this.data.isPending) {
            const BI: PendingBillingCredit = this.data.billingItem;
            this.form.patchValue({
              name              : BI.comment,
              billingItem       : BI.billingItemDescriptionId,
              priceChart        : BI.billingPriceChartId,
              frequency         : BI.billingFrequency,
              line              : BI.line,
              flat              : BI.flat,
              perQty            : BI.perQty,
              whatToCount       : BI.billingWhatToCount,
              year              : BI.billingYear,
              payrollApplied    : BI.billingPeriod,
              chkIgnoreDiscount : BI.isStopDiscount,
              startDate         : BI.startBilling,
              note              : BI.note,
            });
          } else {
            const BI: BillingItem = this.data.billingItem;
            this.form.patchValue({
              name              : BI.comment,
              billingItem       : BI.billingItemDescriptionId,
              priceChart        : BI.billingPriceChartId,
              frequency         : BI.billingFrequency,
              line              : BI.line,
              flat              : BI.flat,
              perQty            : BI.perQty,
              whatToCount       : BI.billingWhatToCount,
              year              : BI.billingYear,
              payrollApplied    : BI.billingPeriod,
              chkIgnoreDiscount : BI.isStopDiscount,
              startDate         : BI.startBilling,
              note              : BI.note,
            });
          }
        } else {
          this.form.patchValue({
            billingItem    : (this.billingType) ? 1 : '',
            payrollApplied : (this.billingType) ? 1 : ''
          });
        }

        if (this.billingType) this.form.get('name').setValidators(Validators.required);
        else {
          this.form.get('frequency').setValidators(Validators.required);
          this.form.get('priceChart').setValidators(Validators.required);
        }
        this.form.get('line').valueChanges.subscribe((val) => {
          this.formatter(val, 'line', 0)
        });

        this.form.get('flat').valueChanges.subscribe((val) => {
          this.formatter(val, 'flat', 4)
        });

        this.form.get('perQty').valueChanges.subscribe((val) => {
          this.formatter(val, 'perQty', 4)
        });

        this.isLoading = false;
      });

    });
  }

  get f() { return this.form.controls; }

  createForm() {
    return this.fb.group({
      name              : [''],
      billingItem       : [1, Validators.required],
      priceChart        : [null],
      frequency         : [null],
      line              : [0, {updateOn: 'blur'}],
      flat              : [0, {updateOn: 'blur'}],
      perQty            : [0, {updateOn: 'blur'}],
      whatToCount       : [null],
      year              : [null],
      payrollApplied    : [null],
      chkIgnoreDiscount : [false],
      startDate         : [moment()],
      note              : [null],
    });
  }

  save() {
    this.formSubmitted = true;
    this.isSaving = true;
    this.form.markAllAsTouched();

    if (this.form.valid && this.validateForm()) {
      this.getSaver().subscribe((billingItem : BillingItemBase) => {
        this.ngxMsgSvc.setSuccessMessage("Billing item saved successfully.");
        this.updateItems();
        this.isSaving = false;
        this.dialogRef.close(null);
      }, (error: HttpErrorResponse) => {
        this.ngxMsgSvc.setErrorResponse(error);
        this.isSaving = false;
      });
    } // end of if

    this.formSubmitted = false;
    this.isSaving = false;

  } // end of save()

  getSaver() : Observable<BillingItemBase> {
    if (this.data.isPending)
      return this.billingSvc.savePendingBillingCredit(this.preparePendingCredit());
    return this.billingSvc.saveBillingItem(this.prepareBillingItem());
  }

  validateForm(): Boolean {
    const line = +this.form.get('line').value;
    const flat = +this.form.get('flat').value;
    const perQty = +this.form.get('perQty').value;
    const billingWTC = +this.form.get('whatToCount').value;
    const billingPriceChart = this.form.get('priceChart').value

    // Per Message Service Standards (as of 10/27/2020)
    // This is not standard practice to display what potentially could be
    // UI errors messages on the FORM elements.
    if ((flat < 0 || perQty < 0) && !(line == 0)) {
      this.ngxMsgSvc.setErrorMessage("Credits must have a line number of 0.");
      return false;
    }

    if (perQty > 0 && billingWTC == 0) {
      this.ngxMsgSvc.setErrorMessage("Per Qty can not have a value if there is nothing to count.");
      return false;
    }

    if ((billingPriceChart == 12 || billingPriceChart == 13) && billingWTC == 0) {
      this.form.get('whatToCount').setValue(this.billingWTCHelper.TimeClockEmployees);
    }

    return true;
  }

  cancel() {
    this.dialogRef.close(null);
  }

  updateItems() {
    this.billingSvc.fetchPendingItems(this.user.clientId);
    if (this.billingType) this.billingSvc.fetchOneTimeItems(this.user.clientId);
    else this.billingSvc.fetchBillingItems(this.user.clientId);
  }

  preparePendingCredit() : PendingBillingCredit {
    let billingItem: PendingBillingCredit;

    billingItem = this.data.billingItem;

    billingItem.billingItemDescriptionId = (this.billingType && this.form.get('billingItem').value == null) ? 1 : this.form.get('billingItem').value;
    billingItem.comment = this.form.get('name').value;
    billingItem.billingPriceChartId = (this.billingType && this.form.get('priceChart').value == null) ? 0 : this.form.get('priceChart').value;
    billingItem.billingFrequency = (this.billingType && this.form.get('frequency').value == null) ? 1 : this.form.get('frequency').value;
    billingItem.line = this.form.get('line').value;
    billingItem.flat = this.form.get('flat').value;
    billingItem.perQty = this.form.get('perQty').value;
    billingItem.billingWhatToCount = this.form.get('whatToCount').value;
    billingItem.billingYear = this.form.get('year').value;
    billingItem.billingPeriod = this.form.get('payrollApplied').value;
    billingItem.isStopDiscount = this.form.get('chkIgnoreDiscount').value;
    billingItem.startBilling = this.billingType ? null : this.form.get('startDate').value
    billingItem.note = this.form.get("note").value;

    return billingItem;
  }

  prepareBillingItem() : BillingItem {
    let billingItem: BillingItem;
    if (this.data.billingItem) {
      billingItem = this.data.billingItem;

      billingItem.billingItemDescriptionId = (this.billingType && this.form.get('billingItem').value == null) ? 1 : this.form.get('billingItem').value;
      billingItem.comment = this.form.get('name').value;
      billingItem.billingPriceChartId = (this.billingType && this.form.get('priceChart').value == null) ? 0 : this.form.get('priceChart').value;
      billingItem.billingFrequency = (this.billingType && this.form.get('frequency').value == null) ? 1 : this.form.get('frequency').value;
      billingItem.line = this.form.get('line').value;
      billingItem.flat = this.form.get('flat').value;
      billingItem.perQty = this.form.get('perQty').value;
      billingItem.billingWhatToCount = this.form.get('whatToCount').value;
      billingItem.billingYear = this.form.get('year').value;
      billingItem.billingPeriod = this.form.get('payrollApplied').value;
      billingItem.isStopDiscount = this.form.get('chkIgnoreDiscount').value;
      billingItem.startBilling = this.billingType ? null : this.form.get('startDate').value;
      billingItem.note = this.form.get("note").value;

    } else {
      billingItem = {
        billingItemId: 0,
        clientId: this.user.clientId,
        billingItemDescriptionId: (this.billingType && this.form.get('billingItem').value == null) ? 1 : this.form.get('billingItem').value,
        billingPriceChartId: (this.billingType && this.form.get('priceChart').value == null) ? 0 : this.form.get('priceChart').value,
        billingPriceChart: null,
        billingFrequency: (this.billingType && this.form.get('frequency').value == null) ? 1 : this.form.get('frequency').value,
        line: this.form.get('line').value,
        flat: this.form.get('flat').value,
        perQty: this.form.get('perQty').value,
        billingWhatToCount: this.form.get('whatToCount').value,
        comment: this.form.get('name').value,
        isOneTime: this.billingType ? true : false,
        payrollId: null,
        modified: moment().toDate(),
        modifiedBy: this.user.userName,
        billingPeriod: this.form.get('payrollApplied').value,
        billingYear: this.form.get('year').value,
        isStopDiscount: this.form.get('chkIgnoreDiscount').value,
        billingItemDescription: null,
        startBilling: this.billingType ? null : this.form.get('startDate').value,
        note: this.form.get('note').value,
      };
    } // end of if else

    return billingItem;
  } // end of prepare

  formatter(val, name, dec) {
    const isInteger = parseInt(val) >= 0;
    if (isInteger) {
      let nv = (+val).toFixed(dec);
      this.form.get(name).setValue(nv, { emitEvent: false });
    }
  }

}
