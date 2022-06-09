import { Component, OnInit, AfterViewInit, AfterViewChecked } from '@angular/core';
import { DsStyleLoaderService, IStyleAsset } from '@ajs/ui/ds-styles/ds-styles.service';
import { FormBuilder, FormGroup, FormArray, FormControl, AbstractControl, Validators, ValidatorFn, ValidationErrors } from "@angular/forms";
import { EmployeeDirectDepositsFormComponent } from "../employee-direct-deposits-form/employee-direct-deposits-form.component";
import { EmployeeDirectDepositsService } from '../shared/employee-direct-deposit-api.service';
import { IEmployeeDirectDepositInfo } from '../shared/employee-direct-deposit-info.model';

import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { switchMap } from 'rxjs/operators';
import * as _ from "lodash";
import { IClientEssOptions } from '../shared/client-ess-options.model';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { MatDialog } from '@angular/material/dialog';

@Component({
    selector: 'ds-employee-direct-deposits',
    templateUrl: './employee-direct-deposits.component.html',
    styleUrls: ['./employee-direct-deposits.component.scss']
})
export class EmployeeDirectDepositsComponent implements OnInit, AfterViewChecked {
    f: FormGroup;
    mainStyle: IStyleAsset;
    user: UserInfo;
    formSubmitted: boolean;
    clientEssOptions: IClientEssOptions;
    accounts: IEmployeeDirectDepositInfo[];
    flatAccounts: IEmployeeDirectDepositInfo[];
    percentAccounts: IEmployeeDirectDepositInfo[];
    depositsToUpdate: IEmployeeDirectDepositInfo[];
    checkPayNote: string;
    percentCheckPay: number;
    sumOfPercent: number;
    allowCheck: boolean;
    allowDirectDeposits: boolean;
    allowEmployeesToUpdateAmount: boolean;
    allowEmployeesToAddAccounts: boolean;
    maxAccountsCountAllowed: number;
    isPrenoteClient: boolean;
    agreeDirectDepositDisclaimer: boolean;
    directDepositDisclaimerText: string;
    isLoading = true;
    percentAboveCap: boolean = false;
    accountMaxLengthError = false;

    constructor(
        private fb: FormBuilder,
        private styles: DsStyleLoaderService,
        private service: EmployeeDirectDepositsService,
        private accountService: AccountService,
        private msgSvc: DsMsgService,
        private dialog: MatDialog,
    ) {
        this.f = this.fb.group({
            flatAccountsArray: this.fb.array([]),
            percentAccountsArray: this.fb.array([]),
            agreeDisclaimer : this.fb.control(false, [this.agreementValidator()])
        });
    }

    ngOnInit() {
        this.agreeDirectDepositDisclaimer = false;
        this.allowCheck = false;
        this.allowDirectDeposits = false;
        this.allowEmployeesToUpdateAmount = false;
        this.allowEmployeesToAddAccounts = false;
        this.isPrenoteClient = true;
        this.maxAccountsCountAllowed = -1;
        this.percentCheckPay = 0;
        this.checkPayNote = "You will not receive any take home pay via paper check";
        this.accounts = [];
        this.flatAccounts = [];
        this.percentAccounts = [];

        this.accountService.getUserInfo().pipe(
            switchMap(user => {
                this.user = user;
                return this.service.isPrenoteClient(this.user.clientId);
            }),
            switchMap(isPrenoteClient => {
                this.isPrenoteClient = isPrenoteClient;
                return this.service.getClientEssOptions(this.user.clientId);
            }),
            switchMap(clientEssOptions => {
                this.clientEssOptions = clientEssOptions;
                this.allowCheck = clientEssOptions.allowCheck;
                this.allowDirectDeposits = clientEssOptions.allowDirectDeposit;
                this.allowEmployeesToUpdateAmount = clientEssOptions.manageDirectDepositAmount || clientEssOptions.manageDirectDepositAmountAndAccountInfo;
                this.allowEmployeesToAddAccounts = clientEssOptions.manageDirectDepositAmountAndAccountInfo;
                if (clientEssOptions.directDepositLimit && clientEssOptions.directDepositLimit > 0) {
                    this.maxAccountsCountAllowed = clientEssOptions.directDepositLimit;
                }
                this.directDepositDisclaimerText = clientEssOptions.directDepositDisclaimer;

                return this.service.getOnboardingAccounts(this.user.employeeId);
            }),
        ).subscribe(accounts => {
            this.accounts = accounts;
            this.flatAccounts = _.filter(accounts, function (o) {
                return o.amountType == -3;
            });

            this.percentAccounts = _.filter(accounts, function (o) {
                return o.amountType == -1;
            });

            this.isLoading = false;
            this.initializePage();
        });
    }

    private initializePage() {
        this.createForm();
    }

    getFormControlError(formArrayName: string, formGroupIndex: number, controlName: string, errorCodes: string[]): boolean {
        let flag: boolean = false;
        var control = (<FormGroup>(<FormArray>this.f.controls[formArrayName]).controls[formGroupIndex]).controls.amount as FormControl;

        _.forEach(errorCodes, (errorCode) => {
            flag = control.hasError(errorCode) && (control.touched || this.formSubmitted);
            if (flag === true)
                return false;
        });
        return flag;
    }

    ngAfterViewInit() {

    }

    private getFlatAccountsArray(): FormArray {
        return this.f.get('flatAccountsArray') as FormArray;
    }

    private addToFlatAccountsArray(flatAccountsArray: FormArray, item: IEmployeeDirectDepositInfo) {
        flatAccountsArray.push(this.fb.group({
            employeeId: item.employeeId,
            employeeDeductionId: item.employeeDeductionId,
            employeeBankId: item.employeeBankId,
            accountType: item.accountType,
            accountName: item.accountName,
            isPrenote: item.isPrenote,
            nickname: item.nickname,
            amount: [item.amount || 0, 
              Validators.compose([
                Validators.required,
                Validators.pattern(/^(\d+)(\.\d+)?$/)
              ])
            ],
            amountType: item.amountType,
            accountNumber: item.accountNumber,
            maskedAccountNumber: item.maskedAccountNumber,
            routingNumber: item.routingNumber,
            maskedRoutingNumber: item.maskedRoutingNumber,
            sortOrderIndex: item.sortOrderIndex
        }));
    }

    private getPercentAccountsArray(): FormArray {
        return this.f.get('percentAccountsArray') as FormArray;
    }

    private addToPercentAccountsArray(percentAccountsArray: FormArray, item: IEmployeeDirectDepositInfo) {
        percentAccountsArray.push(this.fb.group({
            employeeId: item.employeeId,
            employeeDeductionId: item.employeeDeductionId,
            employeeBankId: item.employeeBankId,
            accountType: item.accountType,
            accountName: item.accountName,
            isPrenote: item.isPrenote,
            nickname: item.nickname,
            amount: [
              item.amount || 0, 
              Validators.compose([
                Validators.required,
                Validators.pattern(/^(100|([1-9](\d)?|0)(\.\d{1,2})?)$/)
              ])
            ],
            amountType: item.amountType,
            //accountNumber: item.maskedAccountNumber,
            accountNumber: item.accountNumber,
            maskedAccountNumber: item.maskedAccountNumber,
            //routingNumber: item.maskedRoutingNumber,
            routingNumber: item.routingNumber,
            maskedRoutingNumber: item.maskedRoutingNumber,
            sortOrderIndex: item.sortOrderIndex
        }));
    }

    private setPayCheckText() {
        if (this.sumOfPercent >= 0) {
            this.percentCheckPay = 100 - this.sumOfPercent;
        }

        this.percentCheckPay = this.percentCheckPay < 0 ? 0 : this.percentCheckPay;

        if (this.percentCheckPay > 0) {
            this.checkPayNote = "The amount you will receive from your take home pay via paper check"
        }
        else {
            this.checkPayNote = "You will not receive any take home pay via paper check";
        }
    }

    private createForm(): void {

        const flatAccountsArray = this.getFlatAccountsArray();
        this.flatAccounts.forEach((item, i) => {
            this.addToFlatAccountsArray(flatAccountsArray, item);
        });

        //var sumOfPercent = _.sumBy(this.percentAccounts, function (o) { return o.amount; }); //Have to check why this errs out
        this.sumOfPercent = 0;
        const percentAccountsArray = this.getPercentAccountsArray();
        this.percentAccounts.forEach((item, i) => {
            this.addToPercentAccountsArray(percentAccountsArray, item);
        });
        this.calculateAndValidateTotal();
    }

    getAmount = function (account) {
        if (account.amountType === -3) {
            return '$' + account.amount;
        }

        if (account.amountType === -1) {
            return account.amount + '%';
        }
    };

    calculateAndValidateTotal(): void {
     
      this.sumOfPercent = 0;
      let accountsPA = <FormArray>this.f.controls['percentAccountsArray'];
      _.forEach(accountsPA.value, (item, i) => {
          let account = <IEmployeeDirectDepositInfo>_.find(this.accounts, { employeeDeductionId: item.employeeDeductionId });
          if (account) {
            let amount = account.amount.toString();
            let match = amount.match(/^(100|([1-9](\d)?|0)(\.\d{1,2})?)$/);

            if(match) {
              account.amount = item.amount;
            } else {
              account.amount = 0;
            }
            this.sumOfPercent = this.sumOfPercent + Number(item.amount);
          }
      });

      if (this.sumOfPercent > 100) {
        this.percentAboveCap = true;
      } else {
        this.percentAboveCap = false;
      }
      
      if (this.allowCheck) {
        this.setPayCheckText();

        // recalc total after check amount is updated
        this.sumOfPercent = this.percentCheckPay + this.sumOfPercent;
      }

      
    }

    private agreementValidator(): ValidatorFn {
      return (control: AbstractControl): ValidationErrors | null => {
          return control.value === true ? {} : {'unaccept': true};
      };
    }

  
    saveDirectDeposits(): void {
        this.formSubmitted = true;
        this.calculateAndValidateTotal();
        if (this.sumOfPercent != 100) return;
        this.depositsToUpdate = [];
        this.f.updateValueAndValidity();
        if (this.f.invalid) return;

        let accountsWithAmounts = 0;
        let accountsFA = <FormArray>this.f.controls['flatAccountsArray'];
        _.forEach(accountsFA.value, (item, i) => {
            let account = <IEmployeeDirectDepositInfo>_.find(this.accounts, { employeeDeductionId: item.employeeDeductionId });
            if (account) {
                account.amount = item.amount;
                if (account.amount) {
                  accountsWithAmounts++;
                }
                this.depositsToUpdate.push(account);
                return accountsWithAmounts;
            }
        });

        let accountsPA = <FormArray>this.f.controls['percentAccountsArray'];
        _.forEach(accountsPA.value, (item, i) => {
            let account = <IEmployeeDirectDepositInfo>_.find(this.accounts, { employeeDeductionId: item.employeeDeductionId });
            if (account) {
                account.amount = item.amount;
                if (account.amount) {
                  accountsWithAmounts++;
                }
                this.depositsToUpdate.push(account);
                return accountsWithAmounts;
            }
        });
        let accountMax = this.maxAccountsCountAllowed;
        if ( (accountMax > 0) && (accountMax < accountsWithAmounts) ) {
          this.accountMaxLengthError = true;
          return;
        } else {
          this.accountMaxLengthError = false;
        }
        this.service.updateEmployeeDirectDepositAmounts(this.depositsToUpdate, this.user.employeeId).subscribe(data => {
            if (_.isArray(data)) {
                this.accounts = [];
                this.flatAccounts = [];
                this.percentAccounts = [];

                _.forEach(data, (item, i) => {
                    this.accounts.push(item);

                    if (item.amountType == -3)
                        this.flatAccounts.push(item);

                    if (item.amountType == -1) {
                        this.percentAccounts.push(item);
                    }
                });

                this.f = this.fb.group({
                    flatAccountsArray: this.fb.array([]),
                    percentAccountsArray: this.fb.array([]),
                    agreeDisclaimer : this.fb.control(false, [this.agreementValidator()])
                });

                const flatAccountsArray = this.getFlatAccountsArray();
                this.flatAccounts.forEach((item, i) => {
                    this.addToFlatAccountsArray(flatAccountsArray, item);
                });

                const percentAccountsArray = this.getPercentAccountsArray();
                this.percentAccounts.forEach((item, i) => {
                    this.addToPercentAccountsArray(percentAccountsArray, item);
                });

                //this.setPayCheckText();
                this.msgSvc.setTemporarySuccessMessage("Changes saved successfully.", 5000);
                this.formSubmitted = false;
            }
        });
    }

    showUpdateAccountDialog(employeeDirectDeposit: IEmployeeDirectDepositInfo, deductionId: number): void {
        this.dialog.open(EmployeeDirectDepositsFormComponent, {
            width: '700px',
            data: {
                user: this.user,
                employeeDirectDeposit: employeeDirectDeposit,
                maxAccountsCountAllowed: this.maxAccountsCountAllowed,
                accountsCount: this.accounts.length
            }
        })
        .afterClosed()
        .subscribe(result => {
            if (result == null) return;
            this.msgSvc.sending(true);

            this.service.addEmployeeDirectDeposit(result, this.user.employeeId).subscribe(data => {
                if (!_.isEmpty(data)) {
                  let successMessage = "";
                  if (deductionId > 0) {
                    this.accounts.splice(_.findIndex(this.accounts, {employeeDeductionId: data.employeeDeductionId}), 1, data);

                    if (data.amountType == -3) {
                      this.flatAccounts.splice(_.findIndex(this.flatAccounts, {employeeDeductionId: data.employeeDeductionId}), 1, data);
                    }

                    if (data.amountType == -1) {
                      this.percentAccounts.splice(_.findIndex(this.percentAccounts, {employeeDeductionId: data.employeeDeductionId}), 1, data);
                    }

                    this.f = this.fb.group({
                      flatAccountsArray: this.fb.array([]),
                      percentAccountsArray: this.fb.array([])
                    });

                    const flatAccountsArray = this.getFlatAccountsArray();
                    this.flatAccounts.forEach((item, i) => {
                      this.addToFlatAccountsArray(flatAccountsArray, item);
                    });

                    this.sumOfPercent = 0;
                    const percentAccountsArray = this.getPercentAccountsArray();
                    this.percentAccounts.forEach((item, i) => {
                      this.sumOfPercent = this.sumOfPercent + item.amount;
                      this.addToPercentAccountsArray(percentAccountsArray, item);
                    });

                    this.setPayCheckText();

                    successMessage = "Successfully updated the account.";
                    this.msgSvc.setTemporarySuccessMessage(successMessage, 5000);
                  }
                  else {
                    this.accounts.push(data);

                    if (data.amountType == -3) {
                      this.flatAccounts.push(data);
                      this.addToFlatAccountsArray(this.getFlatAccountsArray(), data);
                    }

                    if (data.amountType == -1) {
                      this.percentAccounts.push(data);
                      this.sumOfPercent = this.sumOfPercent + data.amount;
                      this.addToPercentAccountsArray(this.getPercentAccountsArray(), data);
                      this.setPayCheckText();
                    }

                    successMessage = "Successfully added the account.";
                    this.msgSvc.setTemporarySuccessMessage(successMessage, 5000);
                  }
                }
            });
        });
    }

    trackByFn(index) {
        return index;
    }

    drop(event: CdkDragDrop<string[]>) {
        const percentAccountsArray = this.getPercentAccountsArray();

        var currentItem = percentAccountsArray.at(event.currentIndex);
        var currentAmount = currentItem.get('amount').value;
        var currentEmployeeDeductionId = currentItem.get('employeeDeductionId').value;

        var previousItem = percentAccountsArray.at(event.previousIndex);
        var previousAmount = previousItem.get('amount').value;
        var previousEmployeeDeductionId = previousItem.get('employeeDeductionId').value;

        previousItem.patchValue({ 'amount': currentAmount, 'employeeDeductionId': currentEmployeeDeductionId });
        currentItem.patchValue({ 'amount': previousAmount, 'employeeDeductionId': previousEmployeeDeductionId });


        percentAccountsArray.setControl(event.previousIndex, currentItem);
        percentAccountsArray.setControl(event.currentIndex, previousItem);
    }

    moveItemInFormArray(formArray: FormArray, fromIndex: number, toIndex: number): void {

        const from = this.clamp(fromIndex, formArray.length - 1);
        const to = this.clamp(toIndex, formArray.length - 1);

        if (from === to) {
            return;
        }

        const delta = to > from ? 1 : -1;
        for (let i = from; i * delta < to * delta; i += delta) {
            const previous = formArray.at(i);
            const current = formArray.at(i + delta);
            formArray.setControl(i, current);
            formArray.setControl(i + delta, previous);
        }
    }

    /** Clamps a number between zero and a maximum. */
    clamp(value: number, max: number): number {
        return Math.max(0, Math.min(max, value));
    }

    /**
     * We tell DsStyleLoaderService that this component should use main stylesheet AFTER OnInit and AfterViewInit
     * because we need to make sure that everything is resolved above this component. The DsStyleLoaderService is not
     * instantiated until after OutletComponent is finished loading.
     */
    ngAfterViewChecked() {
        this.styles.useMainStyleSheet();
    }
}
