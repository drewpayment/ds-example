import { Component, OnInit, Inject } from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { UserInfo } from '@ds/core/shared';
import { forkJoin } from 'rxjs';
import { BillingWhatToCount } from '../../../enums/billing-what-to-count.enum';
import { BillingFrequency } from '../../../enums/billing-frequency.enum';
import { BillingPeriod } from '../../../enums/billing-period.enum';
import { BillingService } from '../../services/billing.service';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AutomaticBilling, BillingItemDescription } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';


@Component({
  selector: 'ds-automatic-billing-dialog',
  templateUrl: './automatic-billing-dialog.component.html',
  styleUrls: ['./automatic-billing-dialog.component.scss']
})
export class AutomaticBillingDialogComponent implements OnInit {

  formSubmitted = false;
  isSaving = false;
  isLoading = true;
  user: UserInfo;
  form: FormGroup = this.createForm();
  billingItemDescription: BillingItemDescription[];

  flatErrMsg: string = "Please enter a valid flat number.";
  perQtyErrMsg: string = "Please enter a valid per quantity number.";
  doubleErrMsg: string = "Flat or Per Quantity must have a value greater than 0.";
  bothWrong = false;

  /** Enum Helpers for readability */
  billingWTCHelper = BillingWhatToCount;
  billingWTCList = BillingWhatToCount.getModel();
  billingFreqHelper = BillingFrequency.getModel();
  billingPeriodHelper = BillingPeriod.getModel();

  constructor(
    private billingSvc: BillingService,
    private accountSvc: AccountService,
    private fb: FormBuilder,
    private ngxMsg: NgxMessageService,
    private dialogRef: MatDialogRef<AutomaticBillingDialogComponent>,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) { }

  ngOnInit() {

    forkJoin(
      this.accountSvc.getUserInfo(),
      this.billingSvc.getBillingItemDescription()
    ).subscribe(([user, BIDs]) => {
      this.user = user;
      this.billingItemDescription = BIDs;

      if (this.data.billingItem) {
        const BI: AutomaticBilling = this.data.billingItem;
        this.form.patchValue({
          name              : BI.billingItemDescriptionId,
          frequency         : BI.billingFrequency,
          line              : BI.line,
          flat              : BI.flat,
          perQty            : BI.perQty,
          whatToCount       : BI.billingWhatToCountId
        });
      }

      this.form.get('name').setValidators(Validators.required);

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
    }, (error: HttpErrorResponse) => {
      this.ngxMsg.setErrorMessage(error.message);
    }); // end of fork join

  } // end of init

  get f() { return this.form.controls; }

  createForm() {
    return this.fb.group({
      name              : [null, Validators.required],
      frequency         : [null, Validators.required],
      line              : [0, {updateOn: 'blur'}],
      flat              : [0, {updateOn: 'blur'}],
      perQty            : [0, {updateOn: 'blur'}],
      whatToCount       : [null],
    });
  }

  formatter(val, name, dec) {
    const isInteger = parseInt(val) >= 0;
    if (isInteger) {
      let nv = (+val).toFixed(dec);
      this.form.get(name).setValue(nv, { emitEvent: false });
    }
  }

  validateForm(): Boolean {
    const line = +this.form.get('line').value;
    const flat = +this.form.get('flat').value;
    const perQty = +this.form.get('perQty').value;
    const billingWTC = +this.form.get('whatToCount').value;

    this.method('flat');
    this.method('perQty');

    // Per Message Service Standards (as of 10/27/2020)
    // This is not standard practice to display what potentially could be
    // UI errors messages on the FORM elements.
    if ((flat < 0 || perQty < 0) && !(line == 0)) {
      this.ngxMsg.setErrorMessage("Credits must have a line number of 0.");
      return false;
    }

    if (perQty > 0 && billingWTC == 0) {
      this.ngxMsg.setErrorMessage("Per Qty can not have a value if there is nothing to count.");
      return false;
    }

    return true;
  }

  prepareBillingItem(): AutomaticBilling {
    let billingItem: AutomaticBilling;
    billingItem = {
      automaticBillingId       : (this.data.billingItem) ? this.data.billingItem.automaticBillingId : 0,
      billingItemDescriptionId : this.form.get('name').value,
      billingFrequency         : this.form.get('frequency').value,
      line                     : this.form.get('line').value,
      flat                     : this.form.get('flat').value,
      perQty                   : this.form.get('perQty').value,
      billingWhatToCountId     : this.form.get('whatToCount').value,
      featureOptionId          : this.data.featureOptionId,
      billingItemDescription   : null
    }
    return billingItem;
  }

  save() {
    this.isSaving = true;
    this.form.markAllAsTouched();

    if (this.validateForm() && this.form.valid) {
      this.billingSvc.saveAutomaticBillingItem(this.prepareBillingItem()).subscribe(() => {
        this.ngxMsg.setSuccessMessage("Billing item saved successfully.");
        this.billingSvc.fetchAutomaticBillingItems();
        this.isSaving = false;
        this.formSubmitted = false;
        this.dialogRef.close(null);
      }, (error: HttpErrorResponse) => {
        this.ngxMsg.setErrorResponse(error);
        this.isSaving = false;
      });
    } // end of if

    this.isSaving = false;

  }

  // Try to avoid doing this. This get triggered in every angular life cycle. Bad for performance
  method(controlName: string): boolean {
    if (this.form.get('flat').value == 0 && this.form.get('perQty').value == 0 && this.formSubmitted)
      this.bothWrong = true;
    else
      this.bothWrong = false;

    const isInValid = (this.form.get('flat').value == 0 && this.form.get('perQty').value == 0 && this.formSubmitted ) || (this.form.get(controlName).value == null && (this.form.get(controlName).touched || this.formSubmitted));

    if (isInValid)
      this.form.get(controlName).setErrors({'incorrect': true});
    else
      this.form.get(controlName).setErrors(null);

    return isInValid;
  }

  cancel() {
    this.dialogRef.close(null);
  }

}
