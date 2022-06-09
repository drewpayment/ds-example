import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IDeductionsResult, IDeductionsData, Deductions, IInsertEmployeeDeductionDto, IUpdateEmployeeDeductionDto, IAmountTypeDescription, IDeductionDescription, IInsertEffectiveDateDto, IClientDeductionInfo } from '../../models/Deductions';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { numberValidator, amountAndAmountTypeValidator } from '../../shared/deductions-validators';
import { UserInfo } from '@ds/core/shared';
import { EmployeeDeductionsApiService } from '../../shared/employee-deductions-api.service';
import { convertToBackendTypeEnum } from '../../shared/helper-functions';
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";


@Component({
  selector: 'ds-earnings-add-edit-modal',
  templateUrl: './earnings-add-edit-modal.component.html',
  styleUrls: ['./earnings-add-edit-modal.component.scss']
})
export class EarningsAddEditModalComponent implements OnInit {

    option: string;
    selectedEarning: Deductions | null;
    clientInfo: IClientDeductionInfo;
    earningsForm: FormGroup;
    NO_VALUE_SELECTED: number =  -2147483648;
    userInfo: UserInfo;
    amountTypeList: Array<IAmountTypeDescription>;
    earningDropdownList: Array<IDeductionDescription>;
    reminderDate: Date | null;
    doubleClickDisable: boolean = false;
    formSubmitted: boolean = false;


    constructor(private dialogRef: MatDialogRef<EarningsAddEditModalComponent, IDeductionsResult>, @Inject(MAT_DIALOG_DATA) private data: IDeductionsData, private deducService: EmployeeDeductionsApiService, private msg: DsMsgService) {
        this.option = this.data.option;
        this.clientInfo = this.data.clientInfo;
        this.userInfo = this.data.userInfo;
        this.reminderDate = this.data.reminderDateTime;
        this.selectedEarning = new Deductions(this.data.data);
        this.initializeEarningForm();
    }

    ngOnInit() {
    }

    initializeEarningForm(): void{
        this.populateAmountTypeDropdown();
        this.populateEarningsDropdown();
        this.earningsForm = new FormGroup({
            amount: new FormControl(this.selectedEarning.amount, [Validators.required, numberValidator()]),
            amountType: new FormControl(this.selectedEarning.amountType, [Validators.required]),
            clientDeductionID: new FormControl(this.selectedEarning.clientDeductionID, [Validators.required]),
            clientCostCenterID: new FormControl(this.selectedEarning.clientCostCenterID),
            max: new FormControl(this.selectedEarning.max, [numberValidator()]),
            maxType: new FormControl(this.selectedEarning.maxType),
            totalMax: new FormControl(this.selectedEarning.totalMax, [numberValidator()]),
            isActive: new FormControl(this.selectedEarning.isActive)
        },
        {
            validators: amountAndAmountTypeValidator(false)
        });
    }

    saveForm(){
        this.formSubmitted = true;
        if(!this.earningsForm.valid) 
        return;
        this.doubleClickDisable = true; //Because this is making multiple calls, better to disable the button, because there can be situations where the modal will be closed and stop execution before some calls are sent and resolved.
        if( this.reminderDate != null){ //reminderDate is checked, alternate endpoint required
            //go through form controls, if any are changed, that is selectedDD and form value is different, call insert effectiveDate
            Object.keys(this.earningsForm.controls).forEach(
                key => {
                    if(this.earningsForm.controls[key].value != this.selectedEarning[key]){
                        let effectiveDateDto: IInsertEffectiveDateDto = {
                            effectiveDate: this.reminderDate.toJSON(),
                            table: 'EmployeeDeduction',
                            column: key.charAt(0).toUpperCase() + key.substr(1), //Capitalize first letter of form control name to get column, handle table column name != form control name below
                            oldValue: this.selectedEarning[key] == null ? "" : this.selectedEarning[key],
                            newValue: this.earningsForm.controls[key].value,
                            appliedBy: "",                                       //AppliedBy and AppliedOn are used by other pages/sprocs to actually implement these changes when they are ready
                            appliedOn: "null",
                            createdBy: this.userInfo.userId.toString(),
                            createdOn: new Date().toJSON(),
                            type: 1,                                             //Type is 1 for ChangeRequests(which these all are on EMDeductions)
                            tablePKID: this.selectedEarning.employeeDeductionID, //The Primary Key ID for the table the object were changing belongs in, in EMDeductions this shoudl always be employeeDeductionID
                            friendlyView: "Earning - " + key.charAt(0).toUpperCase() + key.substr(1),
                            accepted: null,
                            employeeID: this.userInfo.selectedEmployeeId(),
                            dataType: convertToBackendTypeEnum(this.earningsForm.controls[key].value) //enum in Ds.Source/DataServices/dsEffectiveDate.vb
                        }

                        if(key == "amountType"){ //special case for amount type because key were loopin over is amountType, but column and value passed to backend is employeeDeductionAmountTypeID
                            effectiveDateDto.column = "DeductionAmountTypeID";
                            effectiveDateDto.oldValue = this.selectedEarning.deductionAmountTypeID == null ? "" : this.selectedEarning.deductionAmountTypeID.toString();
                            effectiveDateDto.newValue = this.amountTypeList.find(amType => amType.description == this.earningsForm.controls['amountType'].value).employeeDeductionAmountTypeID.toString();
                            effectiveDateDto.dataType = "__EmployeeDeductionAmountType.Description";
                        }

                        if(key =="isActive"){
                            effectiveDateDto.oldValue = this.selectedEarning.isActive == true ? "True" : "False";
                            effectiveDateDto.newValue = this.earningsForm.controls['isActive'].value == true ? "True" : "False";
                            effectiveDateDto.dataType = "1"; //copying current VB functionality, should probably be 6
                        }

                        if(key == "maxType") {
                            effectiveDateDto.oldValue = this.selectedEarning.maxType == null ? "" : this.selectedEarning.maxType.toString();
                            effectiveDateDto.newValue = this.earningsForm.controls['maxType'].value;
                            effectiveDateDto.dataType = "__EmployeeDeductionMaxType.Description";
                        }

                        if(key == "clientDeductionID"){ //special case for amount type because column name is ClientDeductionID and key were evaluating on is 'deduction'
                            if (effectiveDateDto.newValue == effectiveDateDto.oldValue){ // if old value and new value are the same, should skip this run of the loop, so return
                                return;
                            }
                            effectiveDateDto.column = "ClientDeductionID";
                            effectiveDateDto.oldValue = this.selectedEarning.clientDeductionID == null ? "" :this.selectedEarning.clientDeductionID.toString();
                            effectiveDateDto.newValue = this.earningsForm.controls['clientDeductionID'].value;
                            effectiveDateDto.dataType = "__ClientDeduction.Description";
                        }

                        if(key == "clientCostCenterID"){
                            effectiveDateDto.column = "ClientCostCenterID";
                            effectiveDateDto.dataType = "__ClientCostCenter.Description";
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
        else if(this.option == 'add') { //operation was an add
            let addDeduction: IInsertEmployeeDeductionDto = {
                clientDeductionID: this.earningsForm.controls['clientDeductionID'].value,
                employeeBankID: this.NO_VALUE_SELECTED,
                employeeBondID: this.NO_VALUE_SELECTED,
                employeeID: this.userInfo.selectedEmployeeId(),
                clientPlanID: this.NO_VALUE_SELECTED,
                amount: this.earningsForm.controls['amount'].value,
                deductionAmountTypeID: this.amountTypeList.find(amType => amType.description == this.earningsForm.controls['amountType'].value).employeeDeductionAmountTypeID,
                max: (this.earningsForm.controls['max'].value == null || this.earningsForm.controls['max'].value == "" ) ? -1 : this.earningsForm.controls['max'].value,
                maxType: (this.earningsForm.controls['maxType'].value == null || this.earningsForm.controls['maxType'].value == "") ? this.NO_VALUE_SELECTED : this.earningsForm.controls['maxType'].value,
                totalMax: (this.earningsForm.controls['totalMax'].value == null || this.earningsForm.controls['totalMax'].value == "") ? -1 : this.earningsForm.controls['totalMax'].value,
                clientVendorID: this.NO_VALUE_SELECTED,
                additionalInfo: null,
                isActive: this.earningsForm.controls['isActive'].value,
                modifiedBy: this.userInfo.userId,
                clientCostCenterID: (this.earningsForm.controls['clientCostCenterID'].value == "" || this.earningsForm.controls['clientCostCenterID'].value == null) ? this.NO_VALUE_SELECTED : this.earningsForm.controls['clientCostCenterID'].value
            };

            this.deducService.insertNewDeduction(addDeduction).subscribe(
                res => {
                    this.msg.setTemporarySuccessMessage("Earning successfully created");
                    this.dialogRef.close(null); //close the dialog, and the data will refresh
                },
                error => {
                    this.msg.showErrorMsg("Error - Earning creation failed");
                    this.dialogRef.close(null);
                }
            )
        }
        else { //operation was an edit
            let editDeduction: IUpdateEmployeeDeductionDto = {
                employeeDeductionID: this.selectedEarning.employeeDeductionID,
                clientDeductionID: this.earningsForm.controls['clientDeductionID'].value,
                employeeBankID: this.NO_VALUE_SELECTED,
                employeeBondID: this.NO_VALUE_SELECTED,
                employeeID: this.userInfo.selectedEmployeeId(),
                clientPlanID: this.NO_VALUE_SELECTED,
                amount: this.earningsForm.controls['amount'].value,
                deductionAmountTypeID: this.amountTypeList.find(amType => amType.description == this.earningsForm.controls['amountType'].value).employeeDeductionAmountTypeID,
                max: (this.earningsForm.controls['max'].value == null || this.earningsForm.controls['max'].value == "") ? -1 : this.earningsForm.controls['max'].value,
                maxType: (this.earningsForm.controls['maxType'].value == null || this.earningsForm.controls['maxType'].value == "") ? this.NO_VALUE_SELECTED : this.earningsForm.controls['maxType'].value,
                totalMax: (this.earningsForm.controls['totalMax'].value == null || this.earningsForm.controls['totalMax'].value == "") ? -1 : this.earningsForm.controls['totalMax'].value,
                clientVendorID: this.NO_VALUE_SELECTED,
                additionalInfo: null,
                isActive: this.earningsForm.controls['isActive'].value,
                modifiedBy: this.userInfo.userId,
                clientCostCenterID: (this.earningsForm.controls['clientCostCenterID'].value == "" || this.earningsForm.controls['clientCostCenterID'].value == null) ? this.NO_VALUE_SELECTED : this.earningsForm.controls['clientCostCenterID'].value
            };

            this.deducService.updateExistingDeduction(editDeduction).subscribe(
                res => {
                    this.msg.setTemporarySuccessMessage("Earning successfully updated");
                    this.dialogRef.close(null); //close the dialog, and the data will refresh
                },
                error => {
                    this.msg.showErrorMsg("Error - Earning update failed");
                    this.dialogRef.close(null);
                }
            )
        }
      }

    cancel(){
      this.dialogRef.close(null);
    }

    populateAmountTypeDropdown(rePopulate: boolean = false){

        let populateCDID = this.selectedEarning.clientDeductionID != null ? this.selectedEarning.clientDeductionID : 0; //for first populate, pass a 0 if add, and CDID if edit
        if( rePopulate ){
            populateCDID = this.earningsForm.controls['clientDeductionID'].value; //if the user changes the <select> tag, were repopulating the dropdown based on that earnings clientDeductionID, so get it
        }
        this.deducService.getEmployeeAmountType(this.userInfo.selectedClientId(), populateCDID ).subscribe(
            (res : IAmountTypeDescription[]) => {
                this.amountTypeList = [];
                for( let desc of res) {
                    this.amountTypeList.push(desc);
                }
            },
            error => {
                console.log(error);
            }
        );
    }

    populateEarningsDropdown(){
    this.deducService.getDeductionDescriptionList(
        this.userInfo.selectedEmployeeId(),
        this.selectedEarning.clientDeductionID == null ? -1: this.selectedEarning.clientDeductionID,
        this.NO_VALUE_SELECTED,//payrollID
        1, //deductionType for earnings
        this.userInfo.clientId,
        "null" //subcheck?
        ).subscribe(
            (res: IDeductionDescription[]) => {
                this.earningDropdownList = [];
                for( let deduc of res){ //loop through all the earnings returned from the endpoint
                    //if the earning is not already being earned, (or its not the currently selected earning(for an edit))
                    if(!this.data.usedEarningsIds.includes(deduc.clientDeductionID) || this.selectedEarning.clientDeductionID == deduc.clientDeductionID) {
                        this.earningDropdownList.push(deduc); //add it to the dropdown to let the user pick
                    }
                }
            },
            error => {
                console.log(error);
            }
        );
    }
  }
