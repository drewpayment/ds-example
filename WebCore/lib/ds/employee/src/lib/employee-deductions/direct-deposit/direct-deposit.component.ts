import { Component, OnInit, ViewChild, Input, OnChanges, Output, EventEmitter } from '@angular/core';
import { Deductions, IDeleteEmployeeDeductionDto } from '../models/Deductions';
import { ModalService } from '../shared/modal.service';
import { UserInfo } from '@ds/core/shared';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { EmployeeDeductionsApiService } from '../shared/employee-deductions-api.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { IClientDeductionInfo } from './../models/Deductions'
import { MatMenuTrigger } from '@angular/material/menu';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource, MatTable } from '@angular/material/table';

@Component({
  selector: 'ds-direct-deposit',
  templateUrl: './direct-deposit.component.html',
  styleUrls: ['./direct-deposit.component.scss']
})
export class DirectDepositComponent implements OnInit, OnChanges {
    displayedColumns: string[] = ['isActive', 'accountTypeDescription', 'amount', 'amountType', 'routingNumber', 'accountNumber', 'isPreNote', 'actions'];
    dataSource: MatTableDataSource<Deductions>;
    hideSequenceButton: boolean = true;
    maxDirectDeposits: number;


    @ViewChild(MatPaginator, {static: false}) paginator: MatPaginator;
    @ViewChild(MatTable, {static: true}) table: MatTable<any>;
    @ViewChild(MatMenuTrigger, {static: false}) trigger: MatMenuTrigger;
    @Input() showInactive: boolean;
    @Input() directDeposits: any;
    @Input() clientInformation: IClientDeductionInfo;
    @Input() userInfo: UserInfo;
    @Input() loading: boolean;
    @Input() reminderDate: Date;
    @Input() reminderChecked: boolean;
    @Input() invalidReminderDate: boolean;
    @Input() userHasAccountWriteAccess: boolean;
    @Input() userHasAmountWriteAccess: boolean;
    @Input() isOwnDDAndIsSupervisor: boolean;
    @Output() refreshDeductions = new EventEmitter<boolean>();


    constructor(private modalService: ModalService, private confirmService: DsConfirmService, private deducService: EmployeeDeductionsApiService, private msg: DsMsgService){
        this.dataSource = new MatTableDataSource<Deductions>();
    }

    ngOnInit() {
      this.dataSource.paginator = this.paginator;
      this.maxDirectDeposits = this.clientInformation.clientEssOptions.directDepositLimit == null ? 999 : this.clientInformation.clientEssOptions.directDepositLimit;
    }

    openDialog(data: Deductions | null){
        let multipleHundreds = false;
        let hundredCount = 0;
        if(this.dataSource.data.length > 0){
            for(let row of this.dataSource.data){
                if (row.amount == 100 && row.amountType == "Percent of Net" && row.isActive == true ){
                    if( data != null){ //if this is an edit
                        if (data.employeeDeductionID != row.employeeDeductionID){ //and we arent editing the row that is the active 100 percent of net
                            hundredCount += 1;
                        }
                    }
                    else{ //its an add
                        hundredCount += 1;
                    }
                }
            }
        }
        if(hundredCount >= 1){
            multipleHundreds = true;
        }
        this.reminderDate = this.reminderChecked ? this.reminderDate : null;
        let dialogRef = this.modalService.openDirectDepositModal(data, this.clientInformation, this.userInfo, this.reminderDate, multipleHundreds);

        dialogRef.afterClosed().subscribe(result => {
            this.refreshDeductions.emit(true); //after modal is closed, refresh parent component table data
        });
    }

    ngOnChanges() {
        if (this.directDeposits.list.length > 0){
            this.dataSource.data = [];
            if(this.showInactive == false){ //if showInactive is Unchecked, only show the active rows
                this.directDeposits.list.forEach(elem => {
                    if(elem.isActive == true){ //if active -> push to the tables dataSource, otherwise -> ignore
                        this.dataSource.data.push(elem);
                    }
                });
            }
            else {//if showINactive is checked
                this.dataSource.data = this.directDeposits.list;  //show all data
            }

            if(this.directDeposits.list.length > 1){
                this.hideSequenceButton = false;
            }
            else{
                this.hideSequenceButton = true;
            }
            this.table.renderRows(); //render rows in this.dataSource.data to screen
        }
        else{
            this.dataSource.data = [];
        }

    }

    editDD(selectedDD: Deductions){
        this.openDialog(selectedDD);
    }

    deleteDD(selectedDD: Deductions){
        this.confirmService.modalOptions.bodyText = "Are you sure you want to delete Direct Deposit for " + selectedDD.accountTypeDescription + " account: " + selectedDD.accountNumber + '?';
        this.confirmService.modalOptions.actionButtonText = "Delete";
        this.confirmService.modalOptions.closeButtonText = "Cancel";
        this.confirmService.modalOptions.swapOkClose = true;

        this.confirmService.show().then(
            res => {

                let deleteDeductionObj: IDeleteEmployeeDeductionDto = {
                    employeeDeductionID: selectedDD.employeeDeductionID,
                    userID: this.userInfo.userId
                }

                this.deducService.deleteEmployeeDeduction(deleteDeductionObj).subscribe(
                    res => {
                        this.msg.setTemporarySuccessMessage("Direct deposit successfully deleted");
                        this.refreshDeductions.emit(true);
                    },
                    error => {
                        this.msg.showErrorMsg("Error - Direct deposit deletion failed");
                        this.refreshDeductions.emit(true);
                    }
                );
            }
        );
    }

    changeSequenceClicked(){
        let newWindow = window.open("EmployeeDeductionSequence.aspx?Type=Bank&EmployeeID=" + this.userInfo.selectedEmployeeId(), 'popup', 'width=700,height=500,menubar=no,toolbar=no');
        let that = this;
        newWindow.onunload = function() {
            that.refreshDeductions.emit(true);
        }
    }
  }



