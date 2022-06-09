import { IDeductionsResult, Deductions, IDeductions, IDeductionDescription, IInsertEmployeeDeductionDto, IAmountTypeDescription, IUpdateEmployeeDeductionDto, IInsertEffectiveDateDto, IClientDeductionInfo } from './../../models/Deductions';
import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogConfig, MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ModalService } from '../../shared/modal.service';
import { IDeductionsData, IPlanDeduction } from '../../models/Deductions';
import { HttpClientModule } from '@angular/common/http';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { amountNumberValidator, numberValidator, amountAndAmountTypeValidator, greaterThen16Validator } from '../../shared/deductions-validators';
import { EmployeeDeductionsApiService } from '../../shared/employee-deductions-api.service';
import { UserInfo } from '@ds/core/shared';
import { convertToBackendTypeEnum } from '../../shared/helper-functions';
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { Subject, Observable } from 'rxjs';
import { switchMap, startWith } from 'rxjs/operators';


@Component({
  selector: 'ds-deductions-add-edit-modal',
  templateUrl: './deductions-add-edit-modal.component.html',
  styleUrls: ['./deductions-add-edit-modal.component.scss']
})
export class DeductionsAddEditModalComponent implements OnInit {

  option: string;
  selectedDeduction: Deductions;
  deductionFormGroup: FormGroup;
  clientInfo: IClientDeductionInfo;
  amountTypeList: Array<IAmountTypeDescription>;
  userInfo: UserInfo;
  deductionDropdownList: Array<IDeductionDescription>;
  NO_VALUE_SELECTED: number =  -2147483648;
  reminderDate: Date | null;
  doubleClickDisable: boolean = false;
  formSubmitted : boolean =  false;

  //Create a
  hookForGettingplans: Subject<number> = new Subject();

  //Created a plan list observable that observes a collection of plans
  planList$: Observable<IPlanDeduction[]>;



  constructor(private dialogRef: MatDialogRef<DeductionsAddEditModalComponent, IDeductionsResult>, @Inject(MAT_DIALOG_DATA) private data: IDeductionsData, private deducService: EmployeeDeductionsApiService, private msg: DsMsgService) {
        this.option = this.data.option;
        this.clientInfo = this.data.clientInfo;
        this.userInfo = this.data.userInfo;
        this.selectedDeduction = new Deductions(this.data.data);
        this.reminderDate = data.reminderDateTime;
        this.initializeDeductionForm();

        //Defines the behavior for how to observe a subject stream.
        //initialize the observable list with wither the deductionId if available or default to 0
        //When the subject stream is updated, use a switchMap to take the observable of the value
        //that was inserted into the subject stream and switch it with an observable value returned from
        // calling getPlanListByClientDeductionId.
        this.planList$ = this.hookForGettingplans.pipe(
            startWith(this.deductionFormGroup.get('deduction').value || 0),
            switchMap(x => this.deducService.getPlanListByClientDeductionId(this.userInfo.selectedClientId(), x)));
  }

  ngOnInit() {
  }

  initializeDeductionForm(): void{
    this.populateAmountTypeDropdown();
    this.populateDeductionDropdown();
    this.deductionFormGroup = new FormGroup({
        deduction: new FormControl(this.selectedDeduction.clientDeductionID, [Validators.required]),
        amount: new FormControl(this.selectedDeduction.amount, [Validators.required, numberValidator(), greaterThen16Validator()]), //greaterThen16Validator added for case SR-371
        amountType: new FormControl(this.selectedDeduction.amountType, [Validators.required]),
        numberPrefix: new FormControl(this.selectedDeduction.numberPrefix),
        max: new FormControl(this.selectedDeduction.max, [numberValidator(), greaterThen16Validator()]), //max is max per payroll labeled field on the form
        maxType: new FormControl(this.selectedDeduction.maxType),
        totalMax: new FormControl(this.selectedDeduction.totalMax, [numberValidator(), greaterThen16Validator()]), //total max is limit/balance labeled field on the form
        vendor: new FormControl(this.selectedDeduction.clientVendorID),
        plan: new FormControl(this.selectedDeduction.clientPlanID),
        additionalInfo: new FormControl(this.selectedDeduction.additionalInfo),
        isActive: new FormControl(this.selectedDeduction.isActive)
    },
    {
        validators: amountAndAmountTypeValidator(false)
    });

    if(this.deductionFormGroup.controls['plan'].value != 0 && this.deductionFormGroup.controls['plan'].value != null){
        this.deductionFormGroup.controls['amount'].disable();
        this.deductionFormGroup.controls['amountType'].disable();
    }
  }

  saveForm(){
    this.formSubmitted = true;
    if(!this.deductionFormGroup.valid) 
    return;
    this.doubleClickDisable = true; //Because this is making multiple calls, better to disable the button, because there can be situations where the modal will be closed and stop execution before some calls are sent and resolved.
    if( this.reminderDate != null){ //reminderDate is checked, alternate endpoint required
        //go through form controls, if any are changed, that is selectedDD and form value is different, call insert effectiveDate
        Object.keys(this.deductionFormGroup.controls).forEach(
            key => {
                if(this.deductionFormGroup.controls[key].value != this.selectedDeduction[key]){
                    let effectiveDateDto: IInsertEffectiveDateDto = {
                        effectiveDate: this.reminderDate.toJSON(),
                        table: 'EmployeeDeduction',
                        column: key.charAt(0).toUpperCase() + key.substr(1), //Capitalize first letter of form control name to get column, handle table column name != form control name below
                        oldValue: this.selectedDeduction[key] == null ? "" : this.selectedDeduction[key],
                        newValue: this.deductionFormGroup.controls[key].value,
                        appliedBy: "",                                       //AppliedBy and AppliedOn are used by other pages/sprocs to actually implement these changes when they are ready
                        appliedOn: "null",
                        createdBy: this.userInfo.userId.toString(),
                        createdOn: new Date().toJSON(),
                        type: 1,                                             //Type is 1 for ChangeRequests(which these all are on EMDeductions)
                        tablePKID: this.selectedDeduction.employeeDeductionID, //The Primary Key ID for the table the object were changing belongs in, in EMDeductions this shoudl always be employeeDeductionID
                        friendlyView: this.selectedDeduction.deduction + " - " + key.charAt(0).toUpperCase() + key.slice(1),
                        accepted: null,
                        employeeID: this.userInfo.selectedEmployeeId(),
                        dataType: convertToBackendTypeEnum(this.deductionFormGroup.controls[key].value) //enum in Ds.Source/DataServices/dsEffectiveDate.vb
                    }

                    if(key == "amountType"){ //special case for amount type because key were loopin over is amountType, but column and value passed to backend is employeeDeductionAmountTypeID
                        effectiveDateDto.column = "DeductionAmountTypeID";
                        effectiveDateDto.oldValue = this.selectedDeduction.deductionAmountTypeID ==  null ? "" : this.selectedDeduction.deductionAmountTypeID.toString();
                        effectiveDateDto.newValue = this.amountTypeList.find(amType => amType.description == this.deductionFormGroup.controls['amountType'].value).employeeDeductionAmountTypeID.toString();
                        effectiveDateDto.dataType = "__EmployeeDeductionAmountType.Description";
                    }

                    if(key == "deduction"){ //special case for amount type because column name is ClientDeductionID and key were evaluating on is 'deduction'
                        effectiveDateDto.column = "ClientDeductionID";
                        effectiveDateDto.oldValue = this.selectedDeduction.clientDeductionID == null ? "" :this.selectedDeduction.clientDeductionID.toString();
                        effectiveDateDto.newValue = this.deductionFormGroup.controls['deduction'].value;
                        if (effectiveDateDto.newValue == effectiveDateDto.oldValue){ // if old value and new value are the same, should skip this run of the loop, so return
                            return;
                        }
                        effectiveDateDto.dataType = "__ClientDeduction.Description";
                    }

                    if(key == "plan"){
                        if(this.deductionFormGroup.controls['plan'].value == null){
                            return;
                        }
                        effectiveDateDto.column = "ClientPlanID";
                        effectiveDateDto.oldValue = this.selectedDeduction.clientPlanID == null ? "" : this.selectedDeduction.clientPlanID.toString();
                        effectiveDateDto.newValue = this.deductionFormGroup.controls['plan'].value;
                        effectiveDateDto.dataType = "__ClientPlan.Description";
                    }

                    if(key == "vendor"){
                        if( this.deductionFormGroup.controls['vendor'].value == null){
                            return;
                        }
                        effectiveDateDto.column = "ClientVendorID";
                        effectiveDateDto.oldValue = this.selectedDeduction.clientVendorID == null ? "" : this.selectedDeduction.clientPlanID.toString();
                        effectiveDateDto.newValue = this.deductionFormGroup.controls['vendor'].value;
                        effectiveDateDto.dataType = "__ClientVendor.Name";
                    }

                    if(key =="isActive"){
                        effectiveDateDto.oldValue = this.selectedDeduction.isActive == true ? "True" : "False";
                        effectiveDateDto.newValue = this.deductionFormGroup.controls['isActive'].value == true ? "True" : "False";
                        effectiveDateDto.dataType = "1"; //copying current VB functionality, should probably be 6
                    }

                    if(key == "maxType") {
                        effectiveDateDto.oldValue = this.selectedDeduction.maxType == null ? "" : this.selectedDeduction.maxType.toString();
                        effectiveDateDto.newValue = this.deductionFormGroup.controls['maxType'].value;
                        effectiveDateDto.dataType = "__EmployeeDeductionMaxType.Description";
                    }

                    this.deducService.insertEffectiveDate(effectiveDateDto).subscribe(
                        res => {
                            this.msg.setTemporarySuccessMessage("Reminder successfully applied");
                            this.dialogRef.close(null);
                        },
                        error => {
                            this.msg.showErrorMsg("Error - Reminder application did not succeed");
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
            clientDeductionID: this.deductionFormGroup.controls['deduction'].value,
            employeeBankID: this.NO_VALUE_SELECTED,
            employeeBondID: this.NO_VALUE_SELECTED,
            employeeID: this.userInfo.selectedEmployeeId(),
            clientPlanID: (this.deductionFormGroup.controls['plan'].value == null || this.deductionFormGroup.controls['plan'].value == 0) ? this.NO_VALUE_SELECTED : this.deductionFormGroup.controls['plan'].value,
            amount: this.deductionFormGroup.controls['amount'].value,
            deductionAmountTypeID: this.amountTypeList.find(amType => amType.description == this.deductionFormGroup.controls['amountType'].value).employeeDeductionAmountTypeID,
            max: (this.deductionFormGroup.controls['max'].value == null || this.deductionFormGroup.controls['max'].value == "")? -1 : this.deductionFormGroup.controls['max'].value,
            maxType: (this.deductionFormGroup.controls['maxType'].value == null || this.deductionFormGroup.controls['maxType'].value == "") ? this.NO_VALUE_SELECTED : this.deductionFormGroup.controls['maxType'].value,
            totalMax: (this.deductionFormGroup.controls['totalMax'].value == null || this.deductionFormGroup.controls['totalMax'].value == "") ? -1 : this.deductionFormGroup.controls['totalMax'].value,
            clientVendorID: (this.deductionFormGroup.controls['vendor'].value == null || this.deductionFormGroup.controls['vendor'].value == 0) ? this.NO_VALUE_SELECTED : this.deductionFormGroup.controls['vendor'].value,
            additionalInfo: this.deductionFormGroup.controls['additionalInfo'].value == "" ? null : this.deductionFormGroup.controls['additionalInfo'].value,
            isActive: this.deductionFormGroup.controls['isActive'].value,
            modifiedBy: this.userInfo.userId,
            clientCostCenterID: this.NO_VALUE_SELECTED
        };
        this.deducService.insertNewDeduction(addDeduction).subscribe(
            res => {
                this.msg.setTemporarySuccessMessage("New deduction successfully created");
                this.dialogRef.close(null); //close the dialog, and the data will refresh
            },
            error => {
                this.msg.showErrorMsg("Error - deduction creation failed");
                this.dialogRef.close(null); //close the dialog, and the data will refresh
            }
        )
    }
    else { //operation was an edit
        let editDeduction: IUpdateEmployeeDeductionDto = {
            employeeDeductionID: this.selectedDeduction.employeeDeductionID,
            clientDeductionID: this.deductionFormGroup.controls['deduction'].value,
            employeeBankID: this.selectedDeduction.employeeBankID == null ? this.NO_VALUE_SELECTED : this.selectedDeduction.employeeBankID,
            employeeBondID: this.NO_VALUE_SELECTED,
            employeeID: this.userInfo.selectedEmployeeId(),
            clientPlanID: (this.deductionFormGroup.controls['plan'].value == null || this.deductionFormGroup.controls['plan'].value == 0) ? this.NO_VALUE_SELECTED : this.deductionFormGroup.controls['plan'].value,
            amount: this.deductionFormGroup.controls['amount'].value,
            deductionAmountTypeID: this.amountTypeList.find(amType => amType.description == this.deductionFormGroup.controls['amountType'].value).employeeDeductionAmountTypeID,
            max: (this.deductionFormGroup.controls['max'].value == null || this.deductionFormGroup.controls['max'].value == "")? -1 : this.deductionFormGroup.controls['max'].value,
            maxType: (this.deductionFormGroup.controls['maxType'].value == null || this.deductionFormGroup.controls['maxType'].value == "") ? this.NO_VALUE_SELECTED : this.deductionFormGroup.controls['maxType'].value,
            totalMax: (this.deductionFormGroup.controls['totalMax'].value == null || this.deductionFormGroup.controls['totalMax'].value == "") ? -1 : this.deductionFormGroup.controls['totalMax'].value,
            clientVendorID: (this.deductionFormGroup.controls['vendor'].value == null || this.deductionFormGroup.controls['vendor'].value == 0) ? this.NO_VALUE_SELECTED : this.deductionFormGroup.controls['vendor'].value,
            additionalInfo: this.deductionFormGroup.controls['additionalInfo'].value == "" ? null : this.deductionFormGroup.controls['additionalInfo'].value,
            isActive: this.deductionFormGroup.controls['isActive'].value,
            modifiedBy: this.userInfo.userId,
            clientCostCenterID: this.NO_VALUE_SELECTED
        };

        this.deducService.updateExistingDeduction(editDeduction).subscribe(
            res => {
                this.msg.setTemporarySuccessMessage("Deduction successfully updated");
                this.dialogRef.close(null); //close the dialog, and the data will refresh
            },
            error => {
                this.msg.showErrorMsg("Error - Deduction update failed");
                this.dialogRef.close(null); //close the dialog, and the data will refresh
            }
        )
    }
  }

  cancel(){
    this.dialogRef.close(null);
  }

  populateAmountTypeDropdown(rePopulate: boolean = false){

    let populateCDID = this.selectedDeduction.clientDeductionID != null ? this.selectedDeduction.clientDeductionID : 0; //for first populate, pass a 0 if add, and CDID if edit
    if( rePopulate ){
        populateCDID = this.deductionFormGroup.get('deduction').value; //if the user changes the <select> tag, were repopulating the dropdown based on that deductions clientDeductionID, so get it
    }

    this.deducService.getEmployeeAmountType(this.userInfo.selectedClientId(), populateCDID).subscribe(
        (res: IAmountTypeDescription[]) => {
            this.amountTypeList = [];
            for( let desc of res) {
                this.amountTypeList.push(desc);
            }

            this.updateNumberPrefix();
        },
        error => {
            console.log(error);
        }
    );

    // add ClientDeductionId to Subject stream
    this.hookForGettingplans.next(populateCDID);

  }

  populateDeductionDropdown(){
    this.deducService.getDeductionDescriptionList(
        this.userInfo.selectedEmployeeId(),
        this.selectedDeduction.clientDeductionID == null ? this.NO_VALUE_SELECTED: this.selectedDeduction.clientDeductionID,
        this.NO_VALUE_SELECTED,//payrollID
        2, //deductionType for deductions
        this.userInfo.selectedClientId(),
        "null" //subcheck?
    ).subscribe(
            (res: IDeductionDescription[]) => {
                this.deductionDropdownList = [];
                for( let deduc of res){
                    this.deductionDropdownList.push(deduc);
                }
            },
            error => {
                console.log(error);
            }
    );
  }

  /**
   * planSelected
   * event listener, called when the plan dropdown value is changed by the user
   * when a plan is selected, this function will populate the plans amount and amount Type data into the add/edit form and disable those form controls.
   */
  planSelected(event: any){
    if(event.target.value != ""){ //a plan was selected (populate and disable amount + amountType fields as these are governed by the plan)
        const selectedPlan = this.clientInfo.planList.find(pl => pl.clientPlanID == event.target.value);
        this.deductionFormGroup.controls['amount'].setValue(selectedPlan.amount);
        this.deductionFormGroup.controls['amount'].disable();
        this.deductionFormGroup.controls['amountType'].setValue(selectedPlan.deductionAmountType);
        this.deductionFormGroup.controls['amountType'].disable();
    }
    else{ //the empty option was selected(clear and reenable fields)
        this.deductionFormGroup.controls['amount'].enable();
        this.deductionFormGroup.controls['amount'].setValue(this.selectedDeduction.amount);
        this.deductionFormGroup.controls['amountType'].enable();
        this.deductionFormGroup.controls['amountType'].setValue(this.selectedDeduction.amountType);
        this.deductionFormGroup.controls['amountType'].markAsUntouched();
    }
  }

  updateNumberPrefix() {
    for (let i = 0; i < this.amountTypeList.length; i++) {
        if (this.amountTypeList[i].description == this.deductionFormGroup.controls['amountType'].value) {
            this.selectedDeduction.numberPrefix = this.amountTypeList[i].numberPrefix;
            this.deductionFormGroup.controls['numberPrefix'].setValue(this.selectedDeduction.numberPrefix);
            break;
        }
    }
  }

  isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 46  || charCode == 45) //allow perdiod for dollar amount 
        return true;
    else if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    else
        return true;
  }
}
