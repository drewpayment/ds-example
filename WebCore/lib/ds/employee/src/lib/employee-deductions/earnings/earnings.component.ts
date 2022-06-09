import { Component, OnInit, ViewChild, Input, OnChanges, EventEmitter, Output } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource, MatTable } from '@angular/material/table';
import { Deductions, IDeleteEmployeeDeductionDto, IClientDeductionInfo, IClientCostCenter } from '../models/Deductions';
import { ModalService } from '../shared/modal.service';
import { UserInfo } from '@ds/core/shared';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { EmployeeDeductionsApiService } from '../shared/employee-deductions-api.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';

@Component({
  selector: 'ds-earnings',
  templateUrl: './earnings.component.html',
  styleUrls: ['./earnings.component.scss']
})
export class EarningsComponent implements OnInit, OnChanges {
    displayedColumns: string[] = ['isActive', 'code', 'earningDescription', 'deduction', 'amount', 'amountType', 'cost-center', 'max', 'maxTypeDescription', 'totalMax', 'actions' ];
    dataSource: MatTableDataSource<Deductions>;


    @ViewChild(MatPaginator, {static: false}) paginator: MatPaginator;
    @ViewChild(MatTable, {static: true}) table: MatTable<any>;
    @Input() showInactive: boolean;
    @Input() earnings: any;
    @Input() clientInformation: IClientDeductionInfo;
    @Input() userInfo: UserInfo;
    @Input() loading: boolean;
    @Input() reminderDate: Date;
    @Input() reminderChecked: boolean;
    @Input() invalidReminderDate: boolean;
    @Input() userHasAccountWriteAccess: boolean;
    @Input() userHasAmountWriteAccess: boolean;
    @Output() refreshDeductions = new EventEmitter<boolean>();

    constructor(private modalService: ModalService, private confirmService: DsConfirmService, private deducService: EmployeeDeductionsApiService, private msg: DsMsgService){
        this.dataSource = new MatTableDataSource<Deductions>();
    }

    ngOnInit() {
      this.dataSource.paginator = this.paginator;
    }

    openDialog(data?: Deductions){
        this.reminderDate = this.reminderChecked ? this.reminderDate : null;
        let usedEarningsArray = new Array<number>();
        this.earnings.list.forEach(ern => {
            usedEarningsArray.push(ern.clientDeductionID);
        });
        let dialogRef = this.modalService.openEarningsModal(data, usedEarningsArray ,this.clientInformation, this.userInfo, this.reminderDate);

        dialogRef.afterClosed().subscribe(result => {
            this.refreshDeductions.emit(true);
        })
    }

    ngOnChanges() {

        if (this.earnings.list.length > 0){
            this.dataSource.data = [];
            if(this.showInactive == false){ //if showInactive is Unchecked, only show the active rows
                this.earnings.list.forEach(elem => {
                    if(elem.isActive == true){ //if active -> push to the tables dataSource, otherwise -> ignore
                        this.dataSource.data.push(elem);
                    }
                });
            }
            else {//if showINactive is checked
                this.dataSource.data = this.earnings.list;  //show all data
            }
            this.loading = false;
            this.table.renderRows(); //render rows in this.dataSource.data to screen
        }
        else{
            this.dataSource.data = [];
        }
    }

    editEarning(deduction: Deductions){
        this.openDialog(deduction);
    }

    deleteEarning(selectedEarning: Deductions){
        this.confirmService.modalOptions.bodyText = "Are you sure you want to delete this " + selectedEarning.earningDescription + ' earning?';
        this.confirmService.modalOptions.actionButtonText = "Delete";
        this.confirmService.modalOptions.closeButtonText = "Cancel";
        this.confirmService.modalOptions.swapOkClose = true;

        this.confirmService.show().then(
            res => {

                let deleteDeductionObj: IDeleteEmployeeDeductionDto = {
                    employeeDeductionID: selectedEarning.employeeDeductionID,
                    userID: this.userInfo.userId
                }

                this.deducService.deleteEmployeeDeduction(deleteDeductionObj).subscribe(
                    res => {
                        this.msg.setTemporarySuccessMessage("Earning successfully deleted")
                        this.refreshDeductions.emit(true);
                    },
                    error => {
                        this.msg.showErrorMsg("Error - Earning deletion failed");
                    }
                );
            }
        );
    }

    displayCostCenterCode(ccId: number): string{
        let cc: IClientCostCenter;
        if( this.clientInformation != undefined){
            cc = this.clientInformation.costCenterList.find( cc => cc.clientCostCenterId == ccId);
            if (cc == undefined){
                return ""
            }
            else {
                return cc.code;
            }
        }
        else{
            return ""
        }
    }
}

