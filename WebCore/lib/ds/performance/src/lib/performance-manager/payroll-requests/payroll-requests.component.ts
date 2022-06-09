import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';
import { EvaluationsApiService } from '@ds/performance/evaluations/shared/evaluations-api.service';
import { PerformanceManagerService } from '../performance-manager.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { IReviewStatusSearchOptions } from '..';
import { MatSort } from '@angular/material/sort';
import { IPayrollRequestItem } from '../shared/payroll-request-item.model';
import { IPayrollRequest } from '../shared/payroll-request.model';
import { IReview } from '@ds/performance/reviews/shared/review.model';
import { PayrollRequestService } from './payroll-request.service';
import * as moment from 'moment';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { distinctUntilChanged, map, tap, switchMap, takeUntil, filter } from 'rxjs/operators';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { Observable, Subject } from 'rxjs';
import { PayrollRequestReportStoreService } from './payroll-request-report/payroll-request-report-store.service';
import { PayrollRequestReportArgsStore } from './payroll-request-report/payroll-request-report-args.store';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import * as saveAs from 'file-saver';
import { PayrollRequestReportArgs } from './payroll-request-report/payroll-request-report-args.model';
import { PayrollRequestFilterFactory } from './payroll-request-report/payroll-request-filter.service';
import { PayrollRequestToEmpSectionConverterService } from './payroll-request-report/payroll-request-to-emp-section-converter.service';
import { Maybe } from '@ds/core/shared/Maybe';
import { EmpRequestSection } from './payroll-request-report/shared/report-display-data.model';
import { changeDrawerHeightOnOpen, matDrawerAfterHeightChange } from '@ds/core/ui/animations/drawer-auto-height-animation';

@Component({
  selector: 'ds-payroll-requests',
  templateUrl: './payroll-requests.component.html',
  styleUrls: ['./payroll-requests.component.scss'],
  animations: [
    changeDrawerHeightOnOpen,
    matDrawerAfterHeightChange
  ]
})
export class PayrollRequestsComponent implements OnInit, OnDestroy {

  private paginator: MatPaginator;
  private sort: MatSort;
  private data: PayrollRequestReportArgs;

  @ViewChild(MatPaginator, {static: false}) set paginatorSetter(pag: MatPaginator) {
    this.paginator = pag;
    this.matTableDs.paginator = this.paginator;
    if (pag)
      this.listenToTable();
  }

  @ViewChild(MatSort, {static: false}) set sortSetter(srt: MatSort) {
    this.sort = srt;
    this.matTableDs.sort = this.sort;
  }

  payrollRequest: IPayrollRequest;
  viewState = "table";
  displayedColumns = ["select", "avatar", "employeeLastName", "employeeNumber", "award", "earning", "type", "amount", "annualizedTotalAmount", "effective",
      "status", "payrollStatus", "payout", "recommendation", "supervisor", "jobTitle", "action"];
  displayedCardTableColumns = ["select" , "earning" , "currentAmount" , "increaseType" , "increaseAmount" , "proposedTotal" , "effective", "payrollStatus", "approvalStatus"];
  displayedOTTableColumns = ["select", "earning", "increaseType", "increaseAmount", "basedOn","proposedTotal", "effective", "payrollStatus", "approvalStatus"];
  isPageLoad:boolean = true;
  showMerit = true;
  showOneTime = true;
  showApproved = false;
  showNeedsApproval = false;
  showNoRequest = false;
  showDeclined = false;
  pendingCount:number;
  approvedCount:number;
  declinedCount:number;
  pendingMerits:string;
  approvedMerits:string;
  declinedMerits:string;
  isLoading = true;
  hasData = false;
  activeReviewId = 0;
  activeReview: IReview;
  reviewArgs: number[];
  payrollRequestItemArgs: number[];
  selectAll = false;
  matTableDs = new MatTableDataSource<IPayrollRequestItem>([]);
  paginatedRequests: IPayrollRequestItem[];
  pageSize = 10;
  meritCount = 0;
  otCount = 0;
  isScoringEnabled: boolean = false;
  private readonly unsubscriber = new Subject();
  constructor(
    private reviewService: PerformanceReviewsService,
    private evalSvc: EvaluationsApiService,
    private manager: PerformanceManagerService,
    private msgSvc: DsMsgService,
    private payrollRequestSvc: PayrollRequestService,
    private confirmSvc: DsConfirmService,
    public reportPopupSvc: PayrollRequestReportStoreService,
    public argsStore: PayrollRequestReportArgsStore
  ) { }

    ngOnInit() {
        this.argsStore.showMeritClicked(this.showMerit);
        this.argsStore.showOneTimeClicked(this.showOneTime);
        this.setShowTable(this.viewState);
    /** Every time the filter changes reInit data  */
    this.getPayrollRequest(this.manager.activeReviewSearchOptions$.pipe(
        filter(options => options.reviewTemplateId != null),
        tap(options => {
            this.reviewService.isScoringEnabledForReviewTemplate(options.reviewTemplateId).subscribe(x => {
                this.isScoringEnabled = x.data;
            });
        }),
      map(options => {
        this.isLoading = true;
        this.hasData = false;
        const statusOptions: IReviewStatusSearchOptions = options;
        statusOptions.includeScores = true;
        return statusOptions;
      })
    )).pipe(takeUntil(this.unsubscriber)).subscribe();

    /** This is observing a subject in the service */

this.payrollRequestSvc.payrollRequest().pipe(
  tap(result => {
      this.clearDrawer();
      if (result == null) this.hasData = false;
      this.payrollRequest = result;
      if (this.payrollRequest) this.defaultFilters();
    }),
    takeUntil(this.unsubscriber)).subscribe();

    /** Every time the items are updated reload the table */
    this.payrollRequestSvc.prTableView(this.payrollRequestItemArgs).pipe(
      tap(result => {
      let m = 0;
      let o = 0;
      if (result)
        result.forEach(item => {
          item._isProposalOpen = !this.proposalIsApprovedOrDeclined(item.reviewId);
          if (item.payoutTo == 0 && item.canViewPayout) item.payoutTo = this.payrollRequestSvc.calculateProposedTo(item);
          if (item.requestType == 1) m++
          if (item.requestType == 2) o++
        });
      this.meritCount = m;
      this.otCount = o;
      this.matTableDs.data = result;
      this.matTableDs.sort = this.sort;
    }),
    takeUntil(this.unsubscriber)).subscribe();

    this.argsStore.reportParams$.pipe(tap(data => {
      this.data = data;
    })).subscribe();
    }

    setShowTable(currentView: string): void {
        this.argsStore.showTable(currentView != 'card')
    }

  listenToTable() {
    this.paginator.page.pipe(
      distinctUntilChanged((prev,curr) => prev === curr ),
      tap((paginatedData) => {
        let start = 0;
        let end = 0;

        this.pageSize = paginatedData.pageSize;
        this.selectAll = false;
        let pi = paginatedData.pageIndex;

        start = (paginatedData.pageIndex == 0) ? 0 : (pi * paginatedData.pageSize) ;
        end = (start + (paginatedData.pageSize-1));
        if (end > paginatedData.length-1) {
          end = paginatedData.length-1;
        }

        this.payrollRequestSvc.setPaginatedItems(start, end);
      }),
      takeUntil(this.unsubscriber)).subscribe();
  }

  /**************************************************************************
   * @param  {IReviewStatusSearchOptions} statusOptions: filter options
   * @description Gets the initial payrollRequest object from the back end,
   *  after this method payroll-request.service.ts managers the data. When
   *  the data is updated by the service, an observer will update the
   *  payroll request in this component.
   *************************************************************************/
  getPayrollRequest(statusOptions$: Observable<IReviewStatusSearchOptions>) {
    return statusOptions$.pipe(
      switchMap(options => this.payrollRequestSvc.getPayrollRequests(options)),
      tap(result => {
        this.payrollRequest = result;
        if (this.payrollRequest)
          if (this.payrollRequest.reviews != null && this.payrollRequest.reviews.length)
            this.hasData = true;

        if (this.hasData) {
          this.defaultFilters();
          this.getPayrollRequestArgs();
          this.getReviewArgs();
        }

        this.isLoading = false;
      }));
  }

  get meritIncreaseCount() {
    return this.payrollRequestSvc.meritCount();
  }

  get additionalEarningCount() {
    return this.payrollRequestSvc.otCount();
  }

  get payrollRequestItems() {
    return this.payrollRequestSvc.prTableView(this.payrollRequestItemArgs).pipe(
    );
  }

  get prItemsPerReview() {
    return this.payrollRequestSvc.perReviewView(this.activeReviewId);
  }

  getPayrollRequestArgs() {
    let filter = <number[]>[];

    if (this.showMerit) filter.push(1);
    if (this.showOneTime) filter.push(2);

    this.payrollRequestItemArgs = filter;
    this.payrollRequestSvc.filterPrTableView(this.payrollRequestItemArgs);
    this.selectAll = false;
    this.payrollRequestSvc.setPaginatedItems(0, this.pageSize-1);
  }

  getReviewArgs() {
    let args = <number[]>[];

    // activeReview could of been removed from the view
    // close the drawer
    this.clearDrawer();

    if (this.showApproved) args.push(2);
    if (this.showNeedsApproval) args.push(1);
    if (this.showDeclined) args.push(3);
    if (this.showNoRequest) args.push(4);

      this.reviewArgs = args;
      this.argsStore.setSelectedApprovalStatus(this.reviewArgs);
  }

  toggleActiveReviewId(reviewId: number) {
    if (this.activeReviewId == reviewId) {
      this.clearDrawer();
    } else {
      this.activeReviewId = reviewId;
      this.payrollRequestSvc.filterPerReviewView(this.activeReviewId);
      this.activeReview = this.payrollRequest.reviews.find(review => {
        return (review.reviewId == this.activeReviewId);
      });
    }
  }

  clearDrawer() {
    this.activeReviewId = 0;
    this.activeReview = null;
  }

  defaultFilters() {
    this.pendingCount  = this.payrollRequest.pendingMeritIncreaseCount  + this.payrollRequest.pendingAdditionalEarningCount  ;
    this.approvedCount = this.payrollRequest.approvedMeritIncreaseCount + this.payrollRequest.approvedAdditionalEarningCount ;
    this.declinedCount = this.payrollRequest.declinedMeritIncreaseCount + this.payrollRequest.declinedAdditionalEarningCount ;

    //let f = (a:number, b:number):string => (a + (b > 0 ? ('+' + b) : '' )) ;
    let f = (a:number, b:number):string => (a + b) + '' ;

    this.pendingMerits  = f(this.payrollRequest.pendingMeritIncreaseCount  , this.payrollRequest.pendingAdditionalEarningCount ) ;
    this.approvedMerits = f(this.payrollRequest.approvedMeritIncreaseCount , this.payrollRequest.approvedAdditionalEarningCount) ;
    this.declinedMerits = f(this.payrollRequest.declinedMeritIncreaseCount , this.payrollRequest.declinedAdditionalEarningCount) ;

    if(this.isPageLoad){
      this.showNeedsApproval = true;
      this.showApproved = (this.pendingCount == 0);
      this.showDeclined = (this.pendingCount == 0 && this.approvedCount == 0);
      this.showNoRequest = (this.pendingCount == 0 && this.approvedCount == 0 && this.declinedCount == 0);
      this.isPageLoad = false;
    }
  }

  getReviewColor(approvalStatusId?: Number) {
    switch(approvalStatusId) {
      case 1:
        return 'warning';
      case 2:
        return 'success';
      case 3:
        return 'danger';
      case null:
        return 'gray';
      case undefined:
        return 'gray';
      default:
        return 'info';
    }
  }

  getApprovalStatusBtnColor(approvalStatusId?: Number) {
    switch(approvalStatusId) {
      case 1:
        return 'btn-warning';
      case 2:
        return 'btn-success';
      case 3:
        return 'btn-danger';
      case null:
        return 'btn-gray';
      case undefined:
        return 'btn-gray';
      default:
        return 'btn-gray';
    }
  }

  getApprovalStatusLabel(approvalStatusId?: Number) {
    switch(approvalStatusId) {
      case 1:
        return 'Needs Approval';
      case 2:
        return 'Approved';
      case 3:
        return 'Declined';
      case null:
        return 'No Request';
      case undefined:
        return 'No Request';
      default:
        return 'No Request';
    }
  }

  get canEdit() {
    return this.isApprovedOrDeclined() && !this.proposalHasItemProcessedByPayroll();
  }
  get canApprove() {
    return !this.isApprovedOrDeclined();
  }
  get canDecline() {
    return (this.activeReview.proposal != null && this.activeReview.proposal.approvalStatus == 1);
  }

  get recommendedPercent() {
    return this.payrollRequestSvc.recommendedPercentIncrease(this.activeReview.reviewId);
  }

  get recommendedGoalPercent() {
    return (this.activeReview.completedGoals/this.activeReview.totalGoals);
  }

  isApprovedOrDeclined() {
    return (this.activeReview.proposal != null) && (this.activeReview.proposal.approvalStatus == 2 || this.activeReview.proposal.approvalStatus == 3);
  }

  proposalHasItemProcessedByPayroll() {
    if (this.activeReview.proposal != null)
      return this.payrollRequestSvc.proposalHasItemProcessedByPayroll(this.activeReview.reviewId);
    return false;
  }

  proposalHasItemProcessedByPayrollByReview(reviewId: number) {
    return this.payrollRequestSvc.proposalHasItemProcessedByPayroll(reviewId);
  }

  proposalIsApprovedOrDeclined(reviewId: number) {
    return this.payrollRequestSvc.proposalIsApprovedOrDeclined(reviewId);
  }

  updateSelectAll() {
    this.payrollRequestSvc.updateSelectAll(this.selectAll);
  }

  checkSelectAll() {
    if (this.selectAll) this.selectAll = false;
  }

  enablingRequestOnCard(request: IPayrollRequestItem) {
    if (request.approvalStatusId == null || request.approvalStatusId == 4) {
      this.updateItemApprovalStatus(request, 1)
    }
  }

  updateItemApprovalStatus(request: IPayrollRequestItem, newStatus: number) {
    if (request.approvalStatusId == newStatus) return;
    if (this.checkDateBeforeSaving(request.effectiveDate)) {
      if (this.checkAmountBeforeSaving(request.increaseAmount))
        this.payrollRequestSvc.updateItemApprovalStatus(request, newStatus);
      else
        this.invalidAmountWontSave(request);
    } else {
      this.invalidDateWontSave(request);
    }
    }

    updateItems(requests: IPayrollRequestItem[]) {
        this.payrollRequestSvc.saveForm(requests);
    }

  massUpdateApprovalStatus(newStatus: number) {
    this.payrollRequestSvc.massUpdateApprovalStatus(newStatus);
  }

  save(request: IPayrollRequestItem, items: IPayrollRequestItem[]) {

    if (!this.checkDateBeforeSaving(request.effectiveDate)) {
      this.invalidDateWontSave(request);
      return;
    }

    if (!this.checkAmountBeforeSaving(request.increaseAmount)) {
      this.invalidAmountWontSave(request);
      return;
    }

    items = items.filter(x => x == request || this.validateRequest(x));
    this.payrollRequestSvc.saveForm(items);
  }

  private validateRequest(request: IPayrollRequestItem): boolean {
const isDateValid = this.checkDateBeforeSaving(request.effectiveDate);
const isAmountValid = this.checkAmountBeforeSaving(request.increaseAmount);

return isDateValid && isAmountValid;
  }

  declineProposal() {
    this.confirmSvc.show(null, {
      swapOkClose: true,
      bodyText: 'Are you sure you want to decline this request?',
      actionButtonText: 'Yes',
      closeButtonText: 'Cancel'
    }).then(() => {
      this.payrollRequestSvc.declineProposal();
    });
  }

  approveProposal() {
    this.confirmSvc.show(null, {
      bodyText: 'Are you sure you want to approve this request?',
      actionButtonText: 'Yes',
      closeButtonText: 'Cancel'
    }).then(() => {
      this.payrollRequestSvc.approveProposal();
    });

  }

  editProposal() {
    this.payrollRequestSvc.editProposal(this.activeReview);
  }

  checkDateBeforeSaving(effectiveDate: Date) {
    // request.effectiveDate.
    if (moment(effectiveDate).isValid())
      if (moment(effectiveDate) > moment('01/01/1753'))
        return true;

    return false;
  }

  checkAmountBeforeSaving(increaseAmount: number) {
    //if (increaseAmount >= 0)
    //  return true;
    //return false;
    return true; // Negative amount valid
  }

  pageIsSaving() {
    return this.payrollRequestSvc.pageIsSaving();
  }

  invalidDateWontSave(request: IPayrollRequestItem) {
    this.msgSvc.setTemporaryMessage(this.getInvalidDateMessage(request), MessageTypes.error);
  }

  invalidAmountWontSave(request: IPayrollRequestItem) {
    this.msgSvc.setTemporaryMessage(this.getInvalidAmountMessage(request), MessageTypes.error);
  }

  private getInvalidAmountMessage(request: IPayrollRequestItem) {
return "Save failed. " + request.employeeFirstName + ' ' + request.employeeLastName + "'s amount for " + request.awardDescription + " cannot be negative and must have a value. Please confirm the value before continuing.";
  }

  private getInvalidDateMessage(request: IPayrollRequestItem){
return "Save failed. " + request.employeeFirstName + ' ' + request.employeeLastName + "'s effective date for " + request.awardDescription + " is invalid. Please confirm the value before continuing.";
  }

  reCalcPayout(request: IPayrollRequestItem) {
    return this.payrollRequestSvc.calculateProposedTo(request);
  }

  reCalcAnnualizedTotalAmount(request: IPayrollRequestItem) {
    return this.payrollRequestSvc.calculateAnnualizedTotalAmount(request);
  }

  ngOnDestroy(): void {
    this.unsubscriber.next(null);
  }

  download(){
    let csvHeader = "Last Name, First Name,Number,Job Title,Department,Division,Base Pay(Current),Overall Score,Merit Recommendation,Earning,Type,Merit Increase Awarded,Base Pay(Changed To),Effective On," +
      "Goals Completed,Eligible Goals,Bonus,Bonus Effective On,Target,% of Base Pay,Status,Supervisor,Payroll Status";
    let csvBody = "";
    let totCurrentPayout:number = 0;
    let totNewPayout:number = 0;
    let totAdditionalEarning:number = 0;

    let requestItems = <IPayrollRequestItem[]>this.matTableDs.data;
    let nameSorter = (a:IPayrollRequestItem,b:IPayrollRequestItem) => a.employeeLastName.toLowerCase().localeCompare(b.employeeLastName.toLowerCase()) == 0 ?
    a.employeeFirstName.toLowerCase().localeCompare(b.employeeFirstName.toLowerCase()) :
    a.employeeLastName.toLowerCase().localeCompare(b.employeeLastName.toLowerCase());

    const filterFactory = new PayrollRequestFilterFactory();
    const payrollRequestConverter = new PayrollRequestToEmpSectionConverterService();
    const filter = filterFactory.getFilter(this.data.clientSideFilters);
    const empSections = payrollRequestConverter.ConvertPayrollRequestsToEmpSections(new Maybe( this.payrollRequest), filter);
    const monthlyCost = payrollRequestConverter.findMonthyCost(empSections);


    const monthlyCostFrom: number = monthlyCost.map(x => x.from).value();
    const monthlyCostTo: number =  monthlyCost.map(x => x.to).value();
    const additionalEarningsPayout: number = this.findAdditionalEarningPayout(empSections).value();

    (requestItems||[]).sort(nameSorter ).forEach((x:IPayrollRequestItem) => {
      let payProcessed = (x.approvalStatusId == 2 && !x.processedByPayroll) ? 'Pending' :
        (x.approvalStatusId == 2 && x.processedByPayroll) ? 'Processed' : (x.approvalStatusId != 2) ? 'Not Processed' : '';

      let score = x.score;
      let completionPerc = 0;
      if(x.approvalStatusId && x.approvalStatusId != 4 && x.requestType == 1){
        completionPerc = x.percent
      } else if(x.approvalStatusId && x.approvalStatusId != 4 && x.requestType == 2){
        completionPerc = x.totalGoals > 0 ? (x.completedGoals/x.totalGoals) * 100 : 0;
      }

      let percOfBase = 0;
      percOfBase = x.increaseType == 1 ? x.increaseAmount : (x.payoutFrom > 0 ? Math.round(x.increaseAmount * 100 * 100 / x.payoutFrom)/100 : 0 );
      let supervisor = x.directSupervisor ? x.directSupervisor.firstName + ' ' + x.directSupervisor.lastName : '';

      let bonus = x.requestType == 2 ? x.increaseAmount : 0;
      let bonusEff = x.requestType == 2 ? x.effectiveDate : "";
      let meritAwarded = x.requestType == 2 ? 0 : (x.payoutTo - x.payoutFrom);
      let meritEff = x.requestType == 2 ? "" : x.effectiveDate;
      let payoutTo = x.requestType == 2 ? 0 : x.payoutTo ;

      csvBody += `${x.employeeLastName},${x.employeeFirstName},${x.employeeNumber},${x.employeeJobTitle || ''},${x.department || ''},${x.division || ''},${x.payoutFrom},${score},${x.requestType == 1 ? 'Merit Increase' : 'Additional Earnings'},` +
      `${x.awardDescription},${x.increaseType == 1 ? 'Percent' : 'Flat'},${meritAwarded},${payoutTo},${meritEff},` +
      `${x.completedGoals},${x.totalGoals},${bonus},${bonusEff},${x.target||''},${percOfBase},${this.getApprovalStatusLabel(x.approvalStatusId)},${supervisor},${payProcessed}\n`;

      totCurrentPayout += x.payoutFrom;
      totNewPayout += payoutTo;
      totAdditionalEarning += x.requestType == 2 ? x.increaseAmount : 0;
    });

    if(csvBody){
      // Add total
      csvBody += `,,,,,,,,,,,,,,,,,,,,,,\n`;
      csvBody += `Total Payout,,,,,,,,,,,,,,,,,,,,,,\n`;
      csvBody += `Current monthly Base Pay,${monthlyCostFrom},,,,,,,,,,,,,,,,,,,,,\n`;
      csvBody += `New monthly Base Pay,${monthlyCostTo},,,,,,,,,,,,,,,,,,,,,\n`;
      csvBody += `Total Additional Earning Payout,${additionalEarningsPayout},,,,,,,,,,,,,,,,,,,,,\n`;
    }

    let byteString = `${csvHeader}\n${csvBody}`;

    let ab = new ArrayBuffer(byteString.length);
    let ia = new Uint8Array(ab);

    for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }
    var file = new Blob([ab], { type: 'application/octet-stream' });
    saveAs(file, this.payrollRequest.reviewTemplateName + "_PayrollRequests.csv" );
  }

  private findAdditionalEarningPayout(empSections: Maybe<EmpRequestSection[]>): Maybe<number> {
    return empSections.map(x => x.map(section => section.oneTimeItems).reduce((a, b) => a.concat(b), []).reduce((a, b) => a + b.payoutTo, 0));
  }
}
