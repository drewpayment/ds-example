import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormControl, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TaxApiService } from '../shared/tax-api.service';
import { AccountService } from '@ds/core/account.service';
import { IEmployeeTaxDetails } from '../../models/tax.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { forkJoin, of, Observable, from } from 'rxjs';
import { IFilingStatusWithDisplayOrder } from '@ds/employees/taxes/shared/filing-status.model';
import { NumberFoundLegacy } from 'libphonenumber-js/min';
import { AssetHelperService } from '@ds/core/ui/ui-helper/asset-helper';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';
import { switchMap, tap } from 'rxjs/operators';
import { FilingStatus } from '@ds/employees/taxes/shared/filing-status';
import { coerceNumberProperty } from '@angular/cdk/coercion';

@Component({
  selector: 'ds-tax-edit',
  templateUrl: './tax-edit.component.html',
  styleUrls: ['./tax-edit.component.scss']
})
export class TaxEditComponent implements OnInit {
  form: FormGroup = this.createForm();
  taxId: number;
  employeeId: number;
  selectedFiling: FilingStatus;
  taxDetails: IEmployeeTaxDetails;
  numExemptions: number;
  taxDescription: string;
  isLoading = true;
  filingStatuses: IFilingStatusWithDisplayOrder[];
  isFederal = false;
  hasEditPermissions = false;
  taxCredit: number;
  wageDeduction: number;
  otherTaxableIncome: number;
  hasMoreThanOneJob: boolean;
  using2020FederalW4Setup: boolean;
  additionalAmount: number;
  additionalPercent: number;
  initComponent$: Observable<any>;

  constructor(
    public route: ActivatedRoute,
    private api: TaxApiService,
    private acctService: AccountService,
    private sb: MatSnackBar,
    private assets: AssetHelperService,
    private fb: FormBuilder,
    private router: Router,
  ) { }

  ngOnInit() {
    this.initComponent$ = this.route.params.pipe(
        tap(params => {
            this.taxId = +params['id'];
        }),
        switchMap(x => this.acctService.getUserInfo()),
        tap(user => {
            this.employeeId = user.userEmployeeId;
        }),
        switchMap(x => forkJoin(
            this.api.getEmployeeTaxDetails(this.employeeId, this.taxId),
            this.api.getFilingStatuses(this.taxId)
        )),
        tap(([details, filingStatuses]) => {
            this.taxDetails = details[0];
            this.filingStatuses = filingStatuses;
            this.numExemptions = this.taxDetails.numberOfExemptions;
            this.taxDescription = this.taxDetails.filingStatusDescription;
            this.taxCredit = this.taxDetails.taxCredit;
            this.wageDeduction = this.taxDetails.wageDeduction;
            this.otherTaxableIncome = this.taxDetails.otherTaxableIncome;
            this.hasMoreThanOneJob = this.taxDetails.hasMoreThanOneJob;
            this.using2020FederalW4Setup = this.taxDetails.using2020FederalW4Setup;
            this.additionalAmount = this.taxDetails.additionalAmount;
            this.additionalPercent = this.taxDetails.additionalPercent;

            if (this.taxDescription.toLowerCase() === 'federal') {
              this.isFederal = true;
              this.form.disable();
            }
        }),
        switchMap(() => from(this.getFilingStatusToDisplay()) as Observable<FilingStatus>),
        tap((filingStatusIdToDisplay) => {
            this.selectedFiling = this.filingStatuses.map(x => x.filingStatusId).find(x => x === filingStatusIdToDisplay as FilingStatus);
            if (this.selectedFiling === undefined) {
                // If we get here, it means that the EE's current FilingStatusId wasn't found in the array provided by our API.
                console.log(`Error: could not find fillingStatusId in dropDownList items!`);
                // console.log(`Original filingStatusId='${this.taxDetails.filingStatusId}', mapped fillingStatusId='${filingStatusIdToDisplay}'.`);
                this.selectedFiling = FilingStatus.Single;
                // This will default the status to: {id:2, description:'Single'},
                // which will be mapped upon save (for federal tax only)
                // to the equivilant 2020 W2 filingstatus (depending on the state of box2checked).
            } else {
                // this.patchForm();
            }

            this.patchForm();
        }),
        switchMap(x => this.acctService.canPerformActions('Tax.RequestUpdateEmployeeTax')),
        tap((hasEditPermissions) => {
            this.hasEditPermissions = (hasEditPermissions === true);
            if (!this.hasEditPermissions) {
                this.form.disable();
            }
            this.isLoading = false;
        })
    );
  }

  async saveChanges() {
    if(this.form.valid) {
      const employeeTaxChanges: IEmployeeTaxDetails = {
        employeeId: this.employeeId,
        employeeTaxId: this.taxId,
        filingStatusId: await this.getFilingStatusToSave(),
        numberOfExemptions: this.form.controls['exemptions'].value == null ? 0 : coerceNumberProperty(this.form.controls['exemptions'].value),
        filingStatusDescription: this.taxDescription,
        taxCredit: this.taxDetails.taxCredit,
        wageDeduction: this.taxDetails.wageDeduction,
        otherTaxableIncome: this.taxDetails.otherTaxableIncome,
        hasMoreThanOneJob: this.taxDetails.hasMoreThanOneJob,
        using2020FederalW4Setup: this.taxDetails.using2020FederalW4Setup,
        additionalAmount: this.form.controls['additionalAmount'].value == null ? 0 : coerceNumberProperty(this.form.controls['additionalAmount'].value),
        additionalPercent: this.form.controls['additionalPercent'].value == null ? 0 : coerceNumberProperty(this.form.controls['additionalPercent'].value)
        //additionalAmount: this.taxDetails.additionalAmount,
        //additionalPercent: this.taxDetails.additionalPercent
      };
      console.log("Tax Changes: ", employeeTaxChanges);
      this.api.insertEmployeeTaxRequestChange(employeeTaxChanges).subscribe(() => {
        this.sb.open('Tax Change Requested!','dismiss',{duration:2000});
        this.router.navigateByUrl('/profile/taxes');
      });
    }
  }

  changeExemptions(filingStatus: number) {
    if (this.selectedFiling === this.taxDetails.filingStatusId) {
      this.numExemptions = this.taxDetails.numberOfExemptions;
      this.additionalAmount = this.taxDetails.additionalAmount;
      this.additionalPercent = this.taxDetails.additionalPercent;
    } else {
      this.numExemptions = 0;
      this.additionalAmount = 0;
      this.additionalPercent = 0;
    }
    this.selectedFiling = filingStatus;
  }

  private createForm(): FormGroup {
    return new FormGroup({
      filing: this.fb.control({value: ''}, Validators.required),
      exemptions: this.fb.control({value: ''}, [Validators.pattern('^[0-9]{1,2}[:.,-]?$'), Validators.required]),
      taxCredit: this.fb.control({ value: ''}),
      wageDeduction: this.fb.control({ value: ''}),
      otherTaxableIncome: this.fb.control({ value: ''}),
      hasMoreThanOneJob: this.fb.control({ value: ''}),
      additionalAmount: this.fb.control({ value: ''}),
      additionalPercent: this.fb.control({ value: ''})
    });
  }

  private patchForm() {
    this.form.patchValue({
      filing: this.selectedFiling,
      exemptions: this.taxDetails.numberOfExemptions,
      taxCredit: this.taxDetails.taxCredit,
      wageDeduction: this.taxDetails.wageDeduction,
      otherTaxableIncome: this.taxDetails.otherTaxableIncome,
      hasMoreThanOneJob: this.taxDetails.hasMoreThanOneJob,
      additionalAmount: this.taxDetails.additionalAmount,
      additionalPercent: this.taxDetails.additionalPercent,
    });
  }

  getDesktopUrl(): Observable<string> {
    return this.acctService.getSiteUrls().pipe(
      switchMap((urls, _) => {
        const essUrl = urls.find(u => u.siteType == ConfigUrlType.Ess);
        return of(essUrl ? essUrl.url : '');
      })
    );
  }

  goToFederalW4Form() {
    this.router.navigateByUrl('/profile/taxes/w4/'+ `${this.taxDetails.employeeTaxId}`);
  }

  private async getFilingStatusToDisplay() {
    let filingStatusToDisplay = this.taxDetails.filingStatusId;

    if (this.using2020FederalW4Setup && this.isFederal) {
        const result = await this.api.getDisplayed2020FederalFilingStatus(this.taxDetails.filingStatusId).toPromise();
        filingStatusToDisplay = result.filingStatus;
    }

    return filingStatusToDisplay;
  }

  private async getFilingStatusToSave() {
    let filingStatusToSave = this.selectedFiling;

    if (this.using2020FederalW4Setup && this.isFederal) {
        const result = await this.api.get2020FederalFilingStatus(this.taxDetails.filingStatusId, this.hasMoreThanOneJob).toPromise();
        filingStatusToSave = result.filingStatus;
    }

    return filingStatusToSave;
  }
}
