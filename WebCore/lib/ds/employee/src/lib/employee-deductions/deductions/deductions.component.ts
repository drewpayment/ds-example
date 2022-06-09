import { ModalService } from './../shared/modal.service';
import { Component, OnInit, ViewChild, Input, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { Deductions, IDeleteEmployeeDeductionDto, IDeleteEmployeeBankDeductionDto, IUpdateEmployeeDeductionDto } from '../models/Deductions';
import { EmployeeDeductionsApiService } from '../shared/employee-deductions-api.service';
import { UserInfo } from '@ds/core/shared';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource, MatTable } from '@angular/material/table';

@Component({
  selector: 'ds-deductions',
  templateUrl: './deductions.component.html',
  styleUrls: ['./deductions.component.scss']
})
export class DeductionsComponent implements OnInit, OnChanges {
    displayedColumns: string[] = ['isActive', 'code', 'deduction', 'amount', 'amountType', 'max' , 'maxTypeDescription', 'totalMax', 'vendor', 'plan', "additionalInfo", "actions"]; //
    dataSource: MatTableDataSource<Deductions>;
    deductionsDescriptionsList: Array<string>;
    NO_VALUE_SELECTED: number =  -2147483648;

    @ViewChild(MatPaginator, {static: false}) paginator: MatPaginator;
    @ViewChild(MatTable, {static: true}) table: MatTable<any>;
    @Input() showInactive: boolean;
    @Input() deductions: any;
    @Input() clientInformation: any;
    @Input() userInfo: UserInfo;
    @Input() loading: boolean;
    @Input() reminderDate: Date;
    @Input() reminderChecked: boolean;
    @Input() invalidReminderDate;
    @Input() deductionsThatAllowDDsList: Array<number>;
    @Input() userHasAccountWriteAccess: boolean;
    @Input() userHasAmountWriteAccess: boolean;

    @Output() refreshDeductions = new EventEmitter<boolean>();
    @Output() refreshClientDeductionInfo = new EventEmitter<any>();
    constructor(private modalService: ModalService, private deducService: EmployeeDeductionsApiService, private confirmService: DsConfirmService, private msg: DsMsgService){
        this.dataSource = new MatTableDataSource<Deductions>();
    }

    ngOnInit() {
      this.dataSource.paginator = this.paginator;
    }

    ngOnChanges() {
        if (this.deductions.list.length > 0){
            this.dataSource.data = [];
            if(this.showInactive == false){ //if showInactive is Unchecked, only show the active rows
                this.deductions.list.forEach(elem => {
                    if(elem.isActive == true){ //if active -> push to the tables dataSource, otherwise -> ignore
                        this.dataSource.data.push(elem);
                    }
                });
            }
            else {//if showINactive is checked
                this.dataSource.data = this.deductions.list;  //show all data
            }
            this.loading = false;
            this.table.renderRows(); //render rows in this.dataSource.data to screen
        }
        else{
            this.dataSource.data = [];
        }
    }

    addDeduction() {
        let dialogRef = this.modalService.openDeductionsModal(null, this.clientInformation, this.userInfo, null);

        dialogRef.afterClosed().subscribe(result => {
            this.refreshDeductions.emit(true);
        })
    }

    editDeduction(deduction: Deductions) {
        this.reminderDate = this.reminderChecked ? this.reminderDate : null;
        let dialogRef = this.modalService.openDeductionsModal(deduction, this.clientInformation, this.userInfo, this.reminderDate);

        dialogRef.afterClosed().subscribe(result => {
            this.refreshDeductions.emit(true);
        })
    }

    deleteDeduction(selectedDeduction: Deductions){
        this.confirmService.modalOptions.bodyText = "Are you sure you want to delete this " + selectedDeduction.deduction + " deduction?";
        this.confirmService.modalOptions.actionButtonText = "Delete";
        this.confirmService.modalOptions.closeButtonText = "Cancel";
        this.confirmService.modalOptions.swapOkClose = true;

        this.confirmService.show().then(
            res => {

                let deleteDeductionObj: IDeleteEmployeeDeductionDto = {
                    employeeDeductionID: selectedDeduction.employeeDeductionID,
                    userID: this.userInfo.userId
                }

                this.deducService.deleteEmployeeDeduction(deleteDeductionObj).subscribe(
                    res => {
                        this.msg.setTemporarySuccessMessage("Deduction successfully deleted");
                        this.refreshDeductions.emit(true);
                    },
                    error => {
                        this.msg.showErrorMsg("This deduction has pay history and cannot be deleted. Please inactivate the deduction.");
                        this.refreshDeductions.emit(true);
                    }
                );
            }
        );
    }

    addDDforHSA(DDdata: Deductions) {
        this.reminderDate = this.reminderChecked ? this.reminderDate : null; //make sure reminderDate is null if the reminder date is not selected, otherwise changes will not happen
        let dialogRef = this.modalService.openDirectDepositModal(DDdata, this.clientInformation, this.userInfo, this.reminderDate, false, true);

        dialogRef.afterClosed().subscribe(result => {
            this.refreshDeductions.emit(true);
        })
    }

    editDDforHSA(DDdata: Deductions) {
        this.reminderDate = this.reminderChecked ? this.reminderDate : null; //make sure reminderDate is null if the reminder date is not selected, otherwise changes will not happen
        let dialogRef = this.modalService.openDirectDepositModal(DDdata, this.clientInformation, this.userInfo, this.reminderDate, false, true, true);

        dialogRef.afterClosed().subscribe(result => {
            this.refreshDeductions.emit(true);
        })
    }

    deleteDDforHSA(selectedHSADeduction: Deductions){
        this.confirmService.modalOptions.bodyText = "Are you sure you want to delete the Direct Deposit for this deduction?";
        this.confirmService.modalOptions.actionButtonText = "Delete";
        this.confirmService.modalOptions.closeButtonText = "Cancel";
        this.confirmService.modalOptions.swapOkClose = true;

        this.confirmService.show().then(
            res => { //If yes is selected on confirm modal

                let updateDeducToDelDDObj: IUpdateEmployeeDeductionDto = {
                    employeeDeductionID: selectedHSADeduction.employeeDeductionID,
                    clientDeductionID: selectedHSADeduction.clientDeductionID,
                    employeeBankID: this.NO_VALUE_SELECTED,
                    employeeBondID: this.NO_VALUE_SELECTED,
                    employeeID: this.userInfo.selectedEmployeeId(),
                    clientPlanID: selectedHSADeduction.clientPlanID == 0 || selectedHSADeduction.clientPlanID == null ? this.NO_VALUE_SELECTED : selectedHSADeduction.clientPlanID,
                    amount: selectedHSADeduction.amount,
                    deductionAmountTypeID: selectedHSADeduction.deductionAmountTypeID,
                    max: selectedHSADeduction.max == null ? -1 : selectedHSADeduction.max,
                    maxType: selectedHSADeduction.maxType == null ? this.NO_VALUE_SELECTED : selectedHSADeduction.maxType,
                    totalMax: selectedHSADeduction.totalMax == null ? -1 : selectedHSADeduction.totalMax,
                    clientVendorID: selectedHSADeduction.clientVendorID == null || selectedHSADeduction.clientVendorID == 0 ? this.NO_VALUE_SELECTED : selectedHSADeduction.clientVendorID,
                    additionalInfo: selectedHSADeduction.additionalInfo,
                    isActive: selectedHSADeduction.isActive,
                    modifiedBy: this.userInfo.userId,
                    clientCostCenterID: this.NO_VALUE_SELECTED
                };
                this.deducService.updateExistingDeduction(updateDeducToDelDDObj).subscribe( //update existing deduction with NO_VALUE_SELECTED for BankID, to remove DD for this HSA deduction
                    res => {
                        let deleteDeductionObj: IDeleteEmployeeBankDeductionDto = {
                            employeeDeductionID: selectedHSADeduction.employeeDeductionID,
                            employeeBankID: selectedHSADeduction.employeeBankID,
                            modifiedBy: this.userInfo.userId
                        }

                        this.deducService.deleteEmployeeBankDeduction(deleteDeductionObj).subscribe( //Delete Employee Bank Deduction Info for change history
                            res => {
                                this.msg.setTemporarySuccessMessage("Direct Deposit successfully deleted")
                                this.refreshDeductions.emit(true);
                            },
                            error => {
                                this.refreshDeductions.emit(true);
                                this.msg.showErrorMsg("Direct Deposit deletion failed");
                            }
                        );
                    },
                    error => {
                        this.refreshDeductions.emit(true);
                        this.msg.showErrorMsg("Direct Deposit deletion failed");
                    }
                );
            }
        );
    }


    vendorClicked(){
        window.open("CompanyVendor.aspx?employeeId=" + this.userInfo.selectedEmployeeId(),
        'popup', 'width=700,height=500,menubar=no,toolbar=no');
    }
}










