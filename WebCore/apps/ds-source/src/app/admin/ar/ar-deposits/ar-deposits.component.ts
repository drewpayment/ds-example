import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Observable, merge, Subject, of, throwError } from 'rxjs';
import { ArDeposit } from '../shared/ar-deposit.model';
import { ArService } from '../shared/ar.service';
import { tap, concatMap, finalize, catchError, filter, map, takeUntil } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import * as moment from 'moment';
import { DatePipe } from '@angular/common';
import { EditPostingTriggerComponent } from '../edit-posting-dialog/edit-posting-trigger.component';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { IArReportParameter } from '../shared/ar-report-parameter.model';
import { ArReportType } from '../shared/ar-report-type';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
    selector: 'ds-ar-deposits',
    templateUrl: './ar-deposits.component.html',
    styleUrls: ['./ar-deposits.component.scss']
})
export class ArDepositsComponent implements OnInit, AfterViewInit, OnDestroy {
    createForm(): FormGroup {
        return this.formBuilder.group({
            postingStartDate: this.formBuilder.control(''),
            postingEndDate: this.formBuilder.control('')
        })
    };
    formGroup: FormGroup = this.createForm();
    openPostingsDisplayedColumns: string[] = ['arDepositId', 'createdBy', 'total', 'edit'];
    closedPostingsDisplayedColumns: string[] = ['arDepositId', 'type', 'createdDate', 'createdBy', 'postedDate', 'postedBy', 'total', 'edit'];
    closedPostingsDatasource = new MatTableDataSource<ArDeposit>([]);
    openPostingDatasource = new MatTableDataSource<ArDeposit>([]);
    @ViewChild('closedPostingsPaginator', {static: false}) closedPostingsPaginator: MatPaginator;
    @ViewChild('openPostingsPaginator', {static: false}) openPostingsPaginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort : MatSort
    openPostings: ArDeposit[];
    closedPostings: ArDeposit[];
    disableDelete: boolean;
    depositDetailUrl: string = "api/ar/getdirectdepositdetailreportbydepositid";
    initLists$: Observable<any>;
    get PostingStartDate() { return this.formGroup.controls.postingStartDate as FormControl; }
    get PostingEndDate() { return this.formGroup.controls.postingEndDate as FormControl; }
    @ViewChild(EditPostingTriggerComponent, { static: true }) modalTrigger: EditPostingTriggerComponent;
    arDepositsWithNoPostedDateLoaded: boolean = false;
    arOpenDepositsLoaded: boolean = false;
    userInfo: UserInfo;
    closedDepositCount: number;
    filteredClosedDepositCount: number;
    destroy = new Subject();

    constructor(private formBuilder: FormBuilder, private arService: ArService, private accountService: AccountService, private msg: DsMsgService) {}

    ngOnInit() {
        this.accountService.getUserInfo().subscribe(data => {
            this.userInfo = data;
        });

        this.setDefaultPostingStartAndEndDate();

        this.arService.getArDepositsWithNoPostedDate().pipe(
            catchError((error) => {
                this.msg.setTemporaryMessage('Sorry, this operation failed: \'Get Open Postings\'', MessageTypes.error, 60000);
                return throwError(error);
            }),
            tap(x => {
                this.openPostings = x;
                this.arOpenDepositsLoaded = true;
                this.openPostingDatasource.data = this.openPostings;
            })
        ).subscribe();

        this.getPostedArDeposits();
    }

    ngOnDestroy() {
        this.destroy.next();
    }

    ngAfterViewInit() {
        this.closedPostingsDatasource.paginator = this.closedPostingsPaginator;
        this.openPostingDatasource.paginator = this.openPostingsPaginator;
        this.closedPostingsDatasource.sort = this.sort;
    }

    private setDefaultPostingStartAndEndDate() {
        this.PostingStartDate.setValue(moment().subtract(1, 'week'));
        this.PostingEndDate.setValue(moment());
    }

    filter(){
        if (this.formGroup.invalid) return;

        this.arDepositsWithNoPostedDateLoaded = false;
        this.closedPostings = [];
        this.closedPostingsDatasource.data = this.closedPostings;
        this.getPostedArDeposits();
    }

    private getPostedArDeposits() {
        this.arService.getPostedArDeposits(this.PostingStartDate.value.format('YYYY-MM-DD'), this.PostingEndDate.value.format('YYYY-MM-DD')).pipe(
            catchError((error) => {
                this.msg.setTemporaryMessage('Sorry, this operation failed: \'Get Closed Postings\'', MessageTypes.error, 60000);
                return throwError(error);
            }),
            tap(x => {
                this.closedPostings = x;
                this.closedPostingsDatasource.data = this.closedPostings;
                this.arDepositsWithNoPostedDateLoaded = true;
                this.closedDepositCount = this.closedPostings.length;
                this.filteredClosedDepositCount = this.closedDepositCount;
                this.filterClosedDeposits('');
            })
        ).subscribe();
    }

    editClosedPosting(posting: ArDeposit): void {
        this.modalTrigger.openPosting(posting);
    }

    generateAchDepositCsv() {
        this.msg.loading(true);
        this.arService.generateAchDepositCsv(this.PostingStartDate.value, this.PostingEndDate.value)
            .subscribe((res) => {},
                error => console.error(error),
                () => { this.msg.loading(false) });
    }

    generateArDepositDetailReport(arDepositId: number) {
        this.msg.loading(true);

        var reportParams: IArReportParameter =
        {
            arReportId: ArReportType.DepositDetail,
            fileFormat: 'PDF',
            clientId: null,
            invoiceId: null,
            arDepositId: arDepositId,
            startDate: new Date('1/1/1900'),
            endDate: new Date('1/1/1900'),
            billingItemCode: null,
            agingDate: null,
            agingPeriod: null,
            reportType: null,
            lookBackDate: null,
            gainsLossesToggle:null,
            orderBy: null,
            payrollId: null
        }

        this.arService.generateReport(reportParams).subscribe((res) => {},
            (error) => console.error(error),
            () => { this.msg.loading(false); }
        )
    }

    createDeposit() {

        this.arService.createDeposit().pipe(
            catchError((error) => {
                this.msg.setTemporaryMessage('Sorry, this operation failed: \'Create Deposit\'', MessageTypes.error, 60000);
                return throwError(error);
            }),
            tap(x => {
                this.openPostings.push(x);                            // add new deposit to the array
                this.openPostingDatasource.data = this.openPostings;   // update the open deposits data source
                this.msg.setTemporaryMessage('Deposit #' + x.arDepositId + ' created successfully!')
            })
        ).subscribe();
    }

    editOpenDeposit(deposit: ArDeposit) {

    }

    postDeposit(deposit: ArDeposit){
        if (deposit.total == 0) {
            this.msg.setTemporaryMessage('Deposit #' + deposit.arDepositId + ' has no payments. Cannot be posted.', MessageTypes.error)
        }
        else {
            this.msg.loading;
            this.arService.postDeposit(deposit).subscribe((res) => {},
                (error) => {
                    this.msg.setTemporaryMessage('An error occurred while posting deposit #' + deposit.arDepositId + '. Please try again.', MessageTypes.error, 60000);
                },

                () => {
                    const index = this.openPostings.indexOf(deposit, 0);
                    if (index > -1) {
                        this.openPostings.splice(index, 1);
                        this.openPostingDatasource.data = this.openPostings;
                        this.msg.setTemporaryMessage('Deposit #' + deposit.arDepositId + ' posted successfully!');
                    }
                    this.getPostedArDeposits();
                }
            )
        }
    }

    reopenDeposit(deposit: ArDeposit) {
        this.arService.reopenDeposit(deposit).subscribe((res) => {

                const index = this.closedPostings.indexOf(deposit, 0)
                if (index > -1) {
                    this.closedPostings.splice(index, 1);                       // Remove deposit from clost postings array
                    this.closedPostingsDatasource.data = this.closedPostings;    // Update closed deposits data source
                }

                // Add deposit to open deposts array
                this.openPostings.push(res);

                // Update the open deposits datasource after sorting by deposit ID
                this.openPostingDatasource.data = this.openPostings.sort((d1,d2) => {
                    if (d1.arDepositId > d2.arDepositId) {
                        return 1;
                    }

                    if (d1.arDepositId < d2.arDepositId) {
                        return -1;
                    }

                    return 0;
                })
                this.msg.setTemporaryMessage('Deposit #' + deposit.arDepositId + ' reopened successfully!')

            },
            (error) => {
                this.msg.setTemporaryMessage('An error occurred while reopening deposit #' + deposit.arDepositId + '. Please try again.', MessageTypes.error, 60000);
                console.log(error);
            }
        )
    }

    filterClosedDeposits(filter: string) {
        var filteredPostings = this.closedPostings.filter(
            posting => posting.arDepositId.toString().trim().toLowerCase().includes(filter.trim().toLowerCase()) ||     // Filter by Deposit ID
            posting.arPayments.some(pmt => pmt.invoiceNum.trim().toLowerCase().includes(filter.trim().toLowerCase()))   // Filter by Invoice Number
        );

        this.filteredClosedDepositCount = filteredPostings.length;

        this.closedPostingsDatasource.data = filteredPostings;
    }

    doesPostingHavePayments(deposit: ArDeposit){
        return deposit.arPayments && deposit.arPayments.length > 0;
    }

    deletePosting(deposit: ArDeposit){

        this.msg.sending(true);

        const index = this.openPostings.map(e => e.arDepositId).indexOf(deposit.arDepositId);

        this.arService.deleteArDeposit(deposit.arDepositId).pipe(
            catchError((error) => {
                this.msg.clearMessage;
                this.msg.setTemporaryMessage(error.error.errors[0].msg, MessageTypes.error, 120000);
                return throwError(error);
            })
        )
        .subscribe(result => {
            if (index > -1) {
                this.openPostings.splice(index, 1);
                this.openPostingDatasource.data = this.openPostings;
                this.msg.setTemporaryMessage('Deleted deposit number ' + deposit.arDepositId, MessageTypes.success, 3000);
            }
        })
    }
}
