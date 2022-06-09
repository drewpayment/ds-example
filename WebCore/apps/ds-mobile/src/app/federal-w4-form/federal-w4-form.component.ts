import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormControl, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TaxApiService } from '../shared/tax-api.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { forkJoin, of, Observable, from } from 'rxjs';
import { IFilingStatusWithDisplayOrder } from '@ds/employees/taxes/shared/filing-status.model';
import { switchMap, tap } from 'rxjs/operators';
import { FilingStatus } from '@ds/employees/taxes/shared/filing-status';
import { AccountService } from '@ds/core/account.service';
import { IEmployeeTax } from '@ds/employees/taxes/shared/employee-tax.model';
import { IEmployeeOnboardingTax } from '@ds/employees/taxes/shared/employee-onboarding-tax.model';
import { IFormSignatureStatusData, IFormStatusData } from '@ajs/onboarding/shared/models';
import { UserInfo } from '@ds/core/shared';
import { coerceNumberProperty } from '@angular/cdk/coercion';

@Component({
  selector: 'ds-federal-w4-form',
  templateUrl: './federal-w4-form.component.html',
  styleUrls: ['./federal-w4-form.component.scss']
})
export class FederalW4FormComponent implements OnInit {
  form: FormGroup = this.createForm();
  initComponent$: Observable<any>;
  taxId: number;
  taxDetails: any;
  filingStatuses: IFilingStatusWithDisplayOrder[];
  taxDescription: any;
  hasMoreThanOneJob: boolean;
  using2020FederalW4Setup: boolean;
  isFederal: boolean;
  selectedFiling: FilingStatus;
  hasEditPermissions: boolean;
  isLoading: boolean;
  hasAdjustments: boolean = true;
  lessThan200: boolean;
  use2020FederalForm: boolean;
  user: UserInfo;
  formSubmitted: boolean;
  isWaiverChecked: boolean = false;
  isSigned: boolean = false;

  constructor(
    public route: ActivatedRoute,
    private api: TaxApiService,
    private acctService: AccountService,
    private sb: MatSnackBar,
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
          this.user = user;
      }),
      switchMap(x => forkJoin(
          this.api.getEmployeeTaxDetails(this.user.employeeId, this.taxId),
          this.api.getFilingStatuses(this.taxId)
      )),
      tap(([details, filingStatuses]) => {
          this.taxDetails = details[0];
          this.filingStatuses = filingStatuses;
          this.taxDescription = this.taxDetails.filingStatusDescription;

          if (this.taxDescription.toLowerCase() === 'federal') {
            this.isFederal = true;
          }
      }),
      switchMap(x => this.acctService.canPerformActions('Tax.RequestUpdateEmployeeTax')),
      tap((hasEditPermissions) => {
          this.hasEditPermissions = (hasEditPermissions === true);
          if (!this.hasEditPermissions) {
              this.form.disable();
          }
          this.lessThan200 = false;
          this.isLoading = false;
      })
    );
  }

  async saveChanges() {
    //Check for required fields before saving
    if (!this.checkValidSave()) return;

    let retObj = {
      forms: null,
      tax: null
    };
    const taxDto = this.prepareModel();
    const formStatusDto = this.prepareFormStatus()
    retObj.tax = taxDto;
    retObj.forms = formStatusDto;
    this.api.updateEmployeeOnboardingW4AndTaxWithNotification(retObj.tax).subscribe(
      eTax => {
          this.api.postEmployeeFormUpdatesWithoutFinalize(this.user.employeeId, retObj.forms).subscribe(result =>{
              this.sb.open('Tax Change Submitted!','dismiss',{duration:2000});
              this.router.navigateByUrl('/profile/taxes/edit/' + `${this.taxId}`);
          });
      }
    );
  }

  changeExemptions(filingStatus: number) {
    this.selectedFiling = filingStatus;
  }

  private createForm(): FormGroup {
    return new FormGroup({
      filing: this.fb.control(null, [Validators.required]),
      taxCredit: this.fb.control(0, [Validators.pattern("^[0-9]*$")]),
      wageDeduction: this.fb.control(0, [Validators.pattern("^[0-9]*$")]),
      otherTaxableIncome: this.fb.control(0, [Validators.pattern("^[0-9]*$")]),
      hasMoreThanOneJob: this.fb.control(false),
      additionalAmount: this.fb.control(0, [Validators.pattern("^[0-9]*$")]),
      additionalIncome: this.fb.control(0, [Validators.pattern("^[0-9]*$")]),
      firstNameLastName: this.fb.control(null, [Validators.required]),
      qualifyingChildren: this.fb.control(0, [Validators.pattern("^[0-9]*$")]),
      otherDependents: this.fb.control(0, [Validators.pattern("^[0-9]*$")]),
      esign: this.fb.control(false, [Validators.required]),
    });
  }

  toggleHasAdjusments(checked):void{
    this.hasAdjustments = checked;
  }

  toggleDependents(checked):void{
    this.lessThan200 = checked;
  }

  waiverCheck(checked):void {
    this.isWaiverChecked = checked;
}

  private prepareModel() {
      let returnDto: IEmployeeOnboardingTax;

      let filingStatusDesc = null;
      for(let status of this.filingStatuses){
          if(status.filingStatus == this.selectedFiling){
              filingStatusDesc = status.description
          }
      }

      let qualifyingChildren = this.form.controls['qualifyingChildren'].value == null ? 0 : coerceNumberProperty(this.form.controls['qualifyingChildren'].value); //declaring here for code brevity below
      let otherDependents = this.form.controls['otherDependents'].value == null ? 0 : coerceNumberProperty(this.form.controls['otherDependents'].value); //declaring here for code brevity below
      let newFilingStatus = null;
      if( this.form.controls['hasMoreThanOneJob'].value){ //user has more than one job
          if(this.selectedFiling == 1){ //married
              newFilingStatus = 19 //married w/ box2
          }
          else if(this.selectedFiling == 2){ //single
              newFilingStatus = 17 //single w/ box2
          }
          else if(this.selectedFiling == 3){ //Hoh
              newFilingStatus = 21 //Hoh w/ box2
          }
      }
      else{ //user didnt check has more than one job
          if(this.selectedFiling == 1){ //married
              newFilingStatus = 20 //married w/o box2
          }
          else if(this.selectedFiling == 2){ //single
              newFilingStatus = 18 //single w/o box2
          }
          else if(this.selectedFiling == 3){ // Hoh
              newFilingStatus = 22 //Hoh w/o box2
          }
      }

      returnDto = {
          taxCategory: 1, //1 for federal taxes
          empTaxId: null,//not the same as employee tax id, this is emptaxId from employeeOnboardingW4
          employeeId: this.user.employeeId,
          filingStatus: newFilingStatus,
          isAdditionalAmountWithheld: this.form.controls['additionalAmount'].value == 0 || this.form.controls['additionalAmount'].value == null? false : true,
          additionalWithholdingAmt: this.form.controls['additionalAmount'].value == null ? 0 : coerceNumberProperty(this.form.controls['additionalAmount'].value),

          qualifyingChildren: qualifyingChildren,
          qualifyingChildrenAmount: qualifyingChildren*2000,
          otherDependents: otherDependents,
          otherDependentsAmount: otherDependents*500,
          taxCredit: qualifyingChildren*2000 + otherDependents*500,

          otherTaxableIncome: this.form.controls['otherTaxableIncome'].value == null ? 0 : coerceNumberProperty(this.form.controls['otherTaxableIncome'].value),
          wageDeduction:  this.form.controls['wageDeduction'].value == null ? 0 : coerceNumberProperty(this.form.controls['wageDeduction'].value),
          hasMoreThanOneJob: this.form.controls['hasMoreThanOneJob'].value,
          using2020FederalW4Setup: true,

          createDt: null,
          stateId: null,
          allowances: 0
      }
      return returnDto
  }

  private prepareFormStatus(): IFormStatusData[] {
    let sig: IFormSignatureStatusData[] = [{
        isSigned: false,
        roleIdentifier: "employee",
        roleName: "Employee",
        signatureDate: new Date(),
        signatureDefinitionId: this.use2020FederalForm == false ? 1023 : 1039, //1023 sigDefId for 2019W4 form, 1026 for 2020W4 form, 1039 for 2022W4 form
        signatureId: null,
        signatureName: this.user.firstName + " " + this.user.lastName,
        signeeFirstName: this.user.firstName,
        signeeLastName: this.user.lastName,
        signeeInitials: this.user.firstName[0].toUpperCase() + this.user.lastName[0].toUpperCase(),
        signeeMiddle: this.user.middleInitial,
        signeeTitle: null
    }];

    let retForm: IFormStatusData[] = [{
        formId: null,  //always a new form
        employeeId: this.user.employeeId,
        clientId: this.user.clientId,
        formTypeId: 10,
        formDefinitionId: this.use2020FederalForm == false ? 54 : 67, //54 is for 2019 w4 form, 57 is for 2020 w4 form, 67 is for 2022 w4 form
        formName: "Federal W-4",
        formVersion: this.use2020FederalForm == false ? "2019" : "2022",
        isComplete: false,
        isCurrentVersion: true,
        DefinitionInfo: null,
        Signatures: sig
    }];
    return retForm;
  }

  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;
  }

  checkValidSave(){
    let f = this.form;
    if (f.get('filing').value == null) {f.get('filing').markAsTouched(); return false};
    if (f.get('esign').value == false) {f.get('esign').markAsDirty(); return false} //disclaimer checkbox
    if (f.get('firstNameLastName').value == null) {f.get('firstNameLastName').markAsTouched(); return false};
    return true;
  }

  closeForm(){
    this.router.navigateByUrl('/profile/taxes/edit/' + `${this.taxId}`);
  }

  //end class
}
