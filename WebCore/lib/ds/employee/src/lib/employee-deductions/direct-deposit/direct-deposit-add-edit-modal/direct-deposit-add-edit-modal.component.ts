import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, OnInit, Inject } from '@angular/core';
import { IDeductionsResult, IDeductionsData, Deductions, IInsertEmployeeDeductionDto, IUpdateEmployeeDeductionDto, IInsertEmployeeBankDto, IUpdateEmployeeBankDto, IInsertEmployeeBankDeductionDto, IInsertEffectiveDateDto, IClientDeductionInfo } from '../../models/Deductions';
import { FormGroup, FormControl, RequiredValidator, Validator, Validators, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { routingNumberValidator } from '@ds/core/ui/forms';
import { amountNumberValidator, amountAndAmountTypeValidator, numberValidator } from '../../shared/deductions-validators';
import { EmployeeDeductionsApiService } from '../../shared/employee-deductions-api.service';
import { UserInfo } from '@ds/core/shared';
import { isNullOrUndefined } from 'util';
import { convertToBackendTypeEnum } from '../../shared/helper-functions';
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";



@Component({
  selector: 'ds-direct-deposit-add-edit-modal',
  templateUrl: './direct-deposit-add-edit-modal.component.html',
  styleUrls: ['./direct-deposit-add-edit-modal.component.scss']
})
export class DirectDepositAddEditModalComponent implements OnInit {

  option: string;
  selectedDD: Deductions;
  DDFormGroup: FormGroup;
  userInfo: UserInfo;
  amountTypeList: any;
  NO_VALUE_SELECTED: number =  -2147483648;
  fromHsa: boolean;
  reminderDate: Date | null;
  multipleHundreds: boolean;
  clientInfo: IClientDeductionInfo;
  doubleClickDisable: boolean = false;
  isOwnDDAndIsSupervisor: boolean;
  formSubmitted : boolean = false;

  constructor(private dialogRef: MatDialogRef<DirectDepositAddEditModalComponent, IDeductionsResult>, @Inject(MAT_DIALOG_DATA) private data: IDeductionsData, private deducService: EmployeeDeductionsApiService, private msg: DsMsgService) {
    this.option = this.data.option;
    this.userInfo = data.userInfo;
    if(this.userInfo.lastEmployeeId == this.userInfo.employeeId && this.userInfo.userTypeId==4){
        this.isOwnDDAndIsSupervisor = true;
    }
    this.fromHsa = this.data.fromHSA;
    this.reminderDate = this.data.reminderDateTime;
    this.multipleHundreds = this.data.multipleHundreds;
    this.clientInfo = this.data.clientInfo;
    this.selectedDD = new Deductions(this.data.data); //will init to an empty class if data is null(add) or an obj with data to edit if not
    this.initializeDDForm(this.selectedDD);
  }

  ngOnInit() {
  }

  cancel(){
    this.dialogRef.close(null);
  }

  initializeDDForm(dataObj: Deductions){
    this.populateAmountTypeDropdown();
    this.DDFormGroup = new FormGroup({
        accountType: new FormControl({value: this.selectedDD.accountType, disabled: this.isOwnDDAndIsSupervisor && this.option == 'edit'}, [Validators.required]),
        amount: new FormControl({value: this.selectedDD.amount, disabled: this.option.includes('HSA')}, [Validators.required, numberValidator()]),
        amountType: new FormControl({value: this.selectedDD.amountType, disabled: this.option.includes('HSA') || this.isOwnDDAndIsSupervisor && this.option == 'edit'}, [Validators.required]),
        routingNumber: new FormControl({value: this.selectedDD.routingNumber, disabled: this.isOwnDDAndIsSupervisor && this.option == 'edit'}, [Validators.required, routingNumberValidator()]),
        accountNumber: new FormControl({value: this.selectedDD.accountNumber, disabled: this.isOwnDDAndIsSupervisor && this.option == 'edit'}, [Validators.required]),
        isActive: new FormControl({value: this.selectedDD.isActive, disabled: this.isOwnDDAndIsSupervisor}),
    },
    {
        validators: amountAndAmountTypeValidator(this.multipleHundreds)
    });
    if(this.clientInfo.noPreNote){ //if the client has noPrenote on
        this.DDFormGroup.addControl("isPreNote", new FormControl({value: false, disabled: this.isOwnDDAndIsSupervisor})); // still initialize, but default to false, and form control will be hidden so user cannot change this value from false in this case
    }
    else{ //if client can change prenote status
        this.DDFormGroup.addControl("isPreNote", new FormControl({value: this.selectedDD.isPreNote, disabled: this.isOwnDDAndIsSupervisor})); //add the form control and set it to the selected deductions prenote(defaults to true on add),
    }
  }

  saveForm(){
    this.formSubmitted = true;
    if(!this.DDFormGroup.valid) 
    return;
    // this.dialogRef.close(null); //close the dialog immediately when save is selected, so the user cannot double click the button and send multiple requests
    this.doubleClickDisable = true; //Because this is making multiple calls, better to disable the button, because there can be situations where the modal will be closed and stop execution before some calls are sent and resolved.
    if( this.reminderDate != null){
        //go through form controls, if any are changed, that is selectedDD and form value is different, call insert effectiveDate

        Object.keys(this.DDFormGroup.controls).forEach(
            key => {
                if(this.DDFormGroup.controls[key].value != this.selectedDD[key] && key != "isPreNote" && key != "accountNumber" && key !="routingNumber" && key != "accountType"){
                    let effectiveDateDto: IInsertEffectiveDateDto = {
                        effectiveDate: this.reminderDate.toJSON(),
                        table: 'EmployeeDeduction',
                        column: key.charAt(0).toUpperCase() + key.substr(1),
                        oldValue: this.selectedDD[key] == null ? "" : this.selectedDD[key],
                        newValue: this.DDFormGroup.controls[key].value,
                        appliedBy: "",
                        appliedOn: "null",
                        createdBy: this.userInfo.userId.toString(),
                        createdOn: new Date().toJSON(),
                        type: 1,
                        tablePKID: this.selectedDD.employeeDeductionID,
                        friendlyView: "Direct Deposit - " + key.charAt(0).toUpperCase() + key.substr(1),
                        accepted: null,
                        employeeID: this.userInfo.selectedEmployeeId(),
                        dataType: convertToBackendTypeEnum(this.DDFormGroup.controls[key].value)
                    }

                    if(key == "amountType"){ //special case for amount type because key were loopin over is amountType, but column and value passed to backend is employeeDeductionAmountTypeID
                        effectiveDateDto.column = "DeductionAmountTypeID";
                        effectiveDateDto.oldValue = this.selectedDD.deductionAmountTypeID == null ? "" : this.selectedDD.deductionAmountTypeID.toString();
                        effectiveDateDto.newValue = this.amountTypeList.find(amType => amType.description == this.DDFormGroup.controls['amountType'].value).employeeDeductionAmountTypeID.toString();
                        effectiveDateDto.dataType = "__EmployeeDeductionAmountType.Description";
                    }

                    if(key =="isActive"){
                        effectiveDateDto.oldValue = this.selectedDD.isActive == true ? "True" : "False";
                        effectiveDateDto.newValue = this.DDFormGroup.controls['isActive'].value == true ? "True" : "False";
                        effectiveDateDto.dataType = "1"; //copying current VB functionality, should probably be 6
                    }

                    this.deducService.insertEffectiveDate(effectiveDateDto).subscribe(
                        res => {
                            this.msg.setTemporarySuccessMessage("Reminder successfully applied");
                            this.dialogRef.close(null);
                        },
                        error => {
                            this.msg.showErrorMsg("Error - reminder application did not succeed");
                            this.dialogRef.close(null);
                        }
                    )
                }
            }
        )
        this.dialogRef.close(null);
    }
    else if( this.option == 'add' ){ //operation was an add
        // if operation is an add, need to call api to add new employee bank info, this returned val is the employeeBankId
        let addBank: IInsertEmployeeBankDto = {
            accountType: this.DDFormGroup.controls['accountType'].value,
            accountNumber: this.DDFormGroup.controls['accountNumber'].value,
            routingNumber: this.DDFormGroup.controls['routingNumber'].value,
            isPreNote: this.DDFormGroup.controls['isPreNote'].value,
            employeeID: this.userInfo.selectedEmployeeId(),
            clientID: this.userInfo.selectedClientId(),
            modifiedBy: this.userInfo.userId
        };

        this.deducService.insertEmployeeBankDto(addBank).subscribe( //call function to insert employee bank information
            res => {
                if (!isNullOrUndefined(res)){ //value returned from call is EmployeeBankID
                    let addDeduction: IInsertEmployeeDeductionDto = { //create Insert object with newly recieved EmployeeBankID
                        clientDeductionID: this.NO_VALUE_SELECTED,
                        employeeBankID: +res.toString(),
                        employeeBondID: this.NO_VALUE_SELECTED,
                        employeeID: this.userInfo.selectedEmployeeId(),
                        clientPlanID: this.NO_VALUE_SELECTED,
                        amount: this.DDFormGroup.controls['amount'].value,
                        deductionAmountTypeID: this.amountTypeList.find(amType => amType.description == this.DDFormGroup.controls['amountType'].value).employeeDeductionAmountTypeID,
                        max: -1,
                        maxType: this.NO_VALUE_SELECTED,
                        totalMax: -1,
                        clientVendorID: this.NO_VALUE_SELECTED,
                        additionalInfo: null,
                        isActive: this.DDFormGroup.controls['isActive'].value,
                        modifiedBy: this.userInfo.userId,
                        clientCostCenterID: this.NO_VALUE_SELECTED,
                    };

                    this.deducService.insertNewDeduction(addDeduction).subscribe( //user Insert object to call InsertDeduction endpoint.
                        res => { //if the call is successful
                            this.msg.setTemporarySuccessMessage("Direct deposit successfully created");
                            this.dialogRef.close(null); //close the dialog, and the data will refresh
                        },
                        error => {
                            this.msg.showErrorMsg("Error - direct deposit creation failed");
                            this.dialogRef.close(null); //close the dialog, and the data will refresh
                        }
                    );
                }
            },
            error => {
                this.msg.showErrorMsg("Error - direct deposit creation failed");
                this.dialogRef.close(null);
            }
        );
    }
    else if(this.option == "edit"){ //operations was an edit
      let updateBank: IUpdateEmployeeBankDto= {
        employeeBankID: this.selectedDD.employeeBankID,
        accountType: this.DDFormGroup.controls['accountType'].value,
        accountNumber: this.DDFormGroup.controls['accountNumber'].value,
        routingNumber: this.DDFormGroup.controls['routingNumber'].value,
        isPreNote: this.DDFormGroup.controls['isPreNote'].value,
        employeeID: this.userInfo.selectedEmployeeId(),
        clientID: this.userInfo.selectedClientId(),
        modifiedBy: this.userInfo.userId
      };
      this.deducService.updateEmployeeBankDto(updateBank).subscribe(
        res => {
            let editDeduction: IUpdateEmployeeDeductionDto = {
                employeeDeductionID: this.selectedDD.employeeDeductionID,
                clientDeductionID: this.NO_VALUE_SELECTED,
                employeeBankID: this.selectedDD.employeeBankID,
                employeeBondID: this.NO_VALUE_SELECTED,
                employeeID: this.userInfo.selectedEmployeeId(),
                clientPlanID: this.NO_VALUE_SELECTED,
                amount: this.DDFormGroup.controls['amount'].value,
                deductionAmountTypeID: this.amountTypeList.find(amType => amType.description == this.DDFormGroup.controls['amountType'].value).employeeDeductionAmountTypeID,
                max: -1,
                maxType: this.NO_VALUE_SELECTED,
                totalMax: -1,
                clientVendorID: this.NO_VALUE_SELECTED,
                additionalInfo: null,
                isActive: this.DDFormGroup.controls['isActive'].value,
                modifiedBy: this.userInfo.userId,
                clientCostCenterID: this.NO_VALUE_SELECTED
            };

            this.deducService.updateExistingDeduction(editDeduction).subscribe(
                res => {
                    this.msg.setTemporarySuccessMessage("Direct deposit successfully updated");
                    this.dialogRef.close(null);
                },
                error => {
                    this.msg.showErrorMsg("Error - direct deposit update failed");
                    this.dialogRef.close(null); //close the dialog, and the data will refresh
                }
            );
        },
        error => {
            console.log(error);
        }
      );
    }
    else if(this.option == "addHSA"){ //operation was to add a Direct Deposit for an HSA deduction

        let addHSABank: IInsertEmployeeBankDeductionDto = {
            accountType: this.DDFormGroup.controls['accountType'].value,
            accountNumber: this.DDFormGroup.controls['accountNumber'].value,
            routingNumber: this.DDFormGroup.controls['routingNumber'].value,
            isPreNote: this.DDFormGroup.controls['isPreNote'].value,
            employeeID: this.userInfo.selectedEmployeeId(),
            clientID: this.userInfo.selectedClientId(),
            modifiedBy: this.userInfo.userId,
            employeeDeductionID: this.selectedDD.employeeDeductionID
        };

        // if operation is an addHSA, need to call api to add new employee bank deduction info, this returned val is the employeeBankId
        this.deducService.insertEmployeeBankDeduction(addHSABank).subscribe( //call function to insert employee bank information
            res => {
                if (!isNullOrUndefined(res)){ //value returned from call is EmployeeBankID
                    let addHSA: IUpdateEmployeeDeductionDto = { //addHSA calls update employee deduction, NOT INSERT
                        employeeDeductionID: this.selectedDD.employeeDeductionID,
                        clientDeductionID: this.selectedDD.clientDeductionID,
                        employeeBankID: +res.toString(),
                        employeeBondID: this.NO_VALUE_SELECTED,
                        employeeID: this.userInfo.selectedEmployeeId(),
                        clientPlanID: this.selectedDD.clientPlanID == 0 || this.selectedDD.clientPlanID == null ? this.NO_VALUE_SELECTED : this.selectedDD.clientPlanID,
                        amount: this.selectedDD.amount,
                        deductionAmountTypeID: this.selectedDD.deductionAmountTypeID,
                        max: this.selectedDD.max == null ? -1 : this.selectedDD.max,
                        maxType: this.selectedDD.maxType == null ? this.NO_VALUE_SELECTED : this.selectedDD.maxType,
                        totalMax: this.selectedDD.totalMax == null ? -1 : this.selectedDD.totalMax,
                        clientVendorID: this.selectedDD.clientVendorID == null || this.selectedDD.clientVendorID == 0 ? this.NO_VALUE_SELECTED : this.selectedDD.clientVendorID,
                        additionalInfo: this.selectedDD.additionalInfo,
                        isActive: this.selectedDD.isActive,
                        modifiedBy: this.userInfo.userId,
                        clientCostCenterID: this.NO_VALUE_SELECTED
                    };

                    this.deducService.updateExistingDeduction(addHSA).subscribe( //user Insert object to call InsertDeduction endpoint.
                        res => { //if the call is successful
                            this.msg.setTemporarySuccessMessage("Direct deposit successfully created");
                            this.dialogRef.close(null); //close the dialog, and the data will refresh
                        },
                        error => {
                            this.msg.showErrorMsg("Error - Direct deposit creation failed");
                            this.dialogRef.close(null); //close the dialog, and the data will refresh
                        }
                    );
                }
            },
            error => {
                console.log(error);
            }
        );

    }
    else if(this.option == 'editHSA'){

        let editHSABank: IUpdateEmployeeBankDto = {
            employeeBankID: this.selectedDD.employeeBankID,
            accountType: this.DDFormGroup.controls['accountType'].value,
            accountNumber: this.DDFormGroup.controls['accountNumber'].value,
            routingNumber: this.DDFormGroup.controls['routingNumber'].value,
            isPreNote: this.DDFormGroup.controls['isPreNote'].value,
            employeeID: this.userInfo.selectedEmployeeId(),
            clientID: this.userInfo.selectedClientId(),
            modifiedBy: this.userInfo.userId,
        };

        this.deducService.updateEmployeeBankDto(editHSABank).subscribe(
            res => { //if the call is successful
                this.msg.setTemporarySuccessMessage("Direct deposit successfully updated");
                this.dialogRef.close(null); //close dialog, refreshing data in parent component
            },
            error => { //if the call fails
                this.msg.showErrorMsg("Direct deposit failed to update");
                this.dialogRef.close(null); //close dialog, refreshing data in parent component
            }
        );
    }
  }

  populateAmountTypeDropdown(){
    //amount type dropdown should only contain Flat($) and Percent of Net(only for Direct Deposits) per PAY-996
    this.amountTypeList = [
        {
            description: "Flat($)",
            employeeDeductionAmountTypeID: -3,
            displayOrder: 1,
            numberPrefix: "$"
        },
        {
            description: "Percent of Net",
            employeeDeductionAmountTypeID: -1,
            displayOrder: 3,
            numberPrefix: "%"
        }
    ];
  }
}


