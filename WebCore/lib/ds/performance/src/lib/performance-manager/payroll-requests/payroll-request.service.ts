import { Injectable } from '@angular/core';
import { IPayrollRequest } from '../shared/payroll-request.model';
import { filter, map, tap } from 'rxjs/operators';
import { IPayrollRequestItem } from '../shared/payroll-request-item.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IReviewStatusSearchOptions } from '..';
import { Observable, Subject, OperatorFunction, BehaviorSubject } from 'rxjs';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';
import { ContactProfileImageLoader } from '@ds/core/contacts';
import { IReview } from '@ds/performance/reviews';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { AutoSaveThrottleStrategy, LoadingResult } from '@ds/core/shared/save-handler-strategy';
import * as moment from 'moment';
import { ApprovalStatus } from '@ds/performance/evaluations/shared/approval-status.enum';

const defaultTrue: LoadingResult = Object.freeze({input: null, isLoading: true});
const defaultFalse: LoadingResult = Object.freeze({input: null, isLoading: false});

@Injectable({
  providedIn: 'root'
})
export class PayrollRequestService {

  private readonly API = 'api/performance/manager';
  private _payrollRequest: IPayrollRequest;
  private _payrollRequestItems$ = new BehaviorSubject<IPayrollRequestItem[]>(null);
  private _payrollRequestItemForCard$ = new BehaviorSubject<IPayrollRequestItem[]>(null);
  private _payrollRequest$ = new Subject<IPayrollRequest>();
  private _isSaving = new BehaviorSubject<LoadingResult>({input: null, isLoading: false});
  private _paginatedRequests: IPayrollRequestItem[];

  private _meritCount = new BehaviorSubject<number>(0);
  private _otCount = new BehaviorSubject<number>(0);
  private itemChanged = new Subject<IPayrollRequestItem[]>();
  tableAutosave$: Observable<any>;

  pageIsSaving() {
    return this._isSaving.asObservable().pipe(map(x => x.isLoading));
  }

  meritCount() {
    return this._meritCount.asObservable();
  }
  otCount() {
    return this._otCount.asObservable();
  }

  prTableView(requestType: number[]) {
    if (!requestType) return this._payrollRequestItems$.asObservable();

    return this._payrollRequestItems$
      .pipe(map(data => data.filter(d => requestType.includes(d.requestType))));
  }

  perReviewView(reviewId: number) {
    if (!reviewId) return this._payrollRequestItemForCard$.asObservable();

    return this._payrollRequestItemForCard$.pipe(map(data => data.filter(d => (d.reviewId == reviewId) )))
  }

  filterPrTableView(requestTypes: number[]): void {

    if (!requestTypes) return this._payrollRequestItems$.next(null);

    const items = this._payrollRequest.payrollRequestItems.filter(item => {
      return (requestTypes.includes(item.requestType));
    });
    this._payrollRequestItems$.next(items);
  }

  filterPerReviewView(reviewId: number): void {

    if (!reviewId) return this._payrollRequestItemForCard$.next(null);

    const items = this._payrollRequest.payrollRequestItems.filter(item => {
      return (item.reviewId == reviewId);
    });

    this._payrollRequestItemForCard$.next(items);
  }

  payrollRequest() {
    return this._payrollRequest$.asObservable();
  }

  constructor(private http:HttpClient, private msgSvc: DsMsgService) {
    const saveFn = this.getAutoSave();
    this.tableAutosave$ = this.itemChanged.pipe(
      map(x => JSON.parse(JSON.stringify(x))),
      saveFn);
   }

   getAutoSave(): OperatorFunction<IPayrollRequestItem[], IPayrollRequest> {
    const mahFunk = AutoSaveThrottleStrategy(
      (newItems: IPayrollRequestItem[], oldItems: IPayrollRequestItem[]) => {
        newItems = newItems || [];
        oldItems = oldItems || [];
        const differentItems = [];

        if(newItems.length != oldItems.length) return false;

        for(var i = 0; i < newItems.length; i++){
          const newItem = newItems[i];
          const oldItem = oldItems[i];

          const isDifferent = newItem.clientEarningId != oldItem.clientEarningId ||
          newItem.increaseType != oldItem.increaseType ||
          newItem.increaseAmount != oldItem.increaseAmount ||
          !moment(newItem.effectiveDate).isSame(moment(oldItem.effectiveDate), 'day');

          if(isDifferent){
            differentItems.push(newItem);
          }
        }

        return differentItems.length === 0;
      }, this._isSaving);

      return mahFunk<IPayrollRequestItem[], IPayrollRequest>(
        this.msgSvc,
        (requestsToUpdate) => this.massSavePayrollRequestItems(requestsToUpdate),
        'Failed to save Payroll Request',
        false);
   }

   saveForm(items: IPayrollRequestItem[]): void {
     this.itemChanged.next(items);
   }

  getPayrollRequests(options?:IReviewStatusSearchOptions):Observable<IPayrollRequest> {
    options = options || {};

    let params = new HttpParams()
        .set('reviewTemplateId', options.reviewTemplateId == null ? '' : options.reviewTemplateId.toString())
        .set('searchText', options.searchText == null ? '' : options.searchText)
        .set('isActiveOnly', (!!options.isActiveOnly).toString())
        .set('isExcludeTemps', (!!options.isExcludeTemps).toString())
        .set('startDate', convertToMoment(options.startDate).format('YYYY-MM-DDTHH:mm:ss'))
        .set('endDate', convertToMoment(options.endDate).format('YYYY-MM-DDTHH:mm:ss'));

    options.filters && options.filters.length && options.filters.forEach(f => {
      if (isNaN(f as any) && f.$selected) {
        const selectedId = f.$selected.id;
        params = params.append(`${f.filterType}`, `${selectedId}`);

      } else if (!isNaN(f as any)) {
        const filterId = f as any;
        params = params.append(filterId, options.filters[filterId].toString());

      }
    });

    if (typeof options.includeScores !== 'undefined')
        params = params.append('includeScores', (!!options.includeScores).toString());

    return this.http.get<IPayrollRequest>(`${this.API}/payroll-request`, { params: params })
        .pipe(map(result => {
                if (result)
                  result.reviews.forEach(review => {
                    //TODO: this is copied from ReviewService ... consolidate
                    ContactProfileImageLoader(review.reviewedEmployeeContact);
                    ContactProfileImageLoader(review.reviewOwnerContact);
                  });
                return result;
            }),
            tap(result => {
              this._payrollRequest = result;
              if (result) {
                this._payrollRequestItems$.next(result.payrollRequestItems);
                this._payrollRequestItemForCard$.next(result.payrollRequestItems);
                this.setPaginatedItems(0,9);
              }
            })
        );
  }

  savePayrollRequestItem(request: IPayrollRequestItem, reviewId: number) : Observable<IPayrollRequest> {

    let url = `${this.API}/payroll-request/update/${reviewId}`;

    return this.http.post<IPayrollRequest>(url, request);
  }

  massSavePayrollRequestItems(requests: IPayrollRequestItem[]) : Observable<IPayrollRequest> {
    let url = `${this.API}/payroll-request/mass/update`;

    return this.http.post<IPayrollRequest>(url, {payrollRequests: requests});
  }

  reOpenProposal(proposalId, clientId): Observable<IPayrollRequest> {
    let url = `${this.API}/reopen-proposal/proposal/${proposalId}/client/${clientId}`;

    return this.http.post<IPayrollRequest>(url, null);
  }

  updateSelectAll(isSelected: boolean) {
    let existing = this._payrollRequest.payrollRequestItems;
    let filteredExisting = this._payrollRequestItems$.value;

    existing.forEach(request => {
      if (this._paginatedRequests.findIndex(x => (request.foreignKeyId != 0 && x.foreignKeyId == request.foreignKeyId)) > -1)
        if ( (request.isEnabledOnProposal || request.approvalStatusId == 3) && !request.processedByPayroll)
          request.isSelectedOnTableView = isSelected;
    });

    filteredExisting.forEach(request => {
      if (this._paginatedRequests.findIndex(x => (request.foreignKeyId != 0 && x.foreignKeyId == request.foreignKeyId)) > -1)
        if ( (request.isEnabledOnProposal || request.approvalStatusId == 3) && !request.processedByPayroll)
          request.isSelectedOnTableView = isSelected;
    });

    this._payrollRequest.payrollRequestItems = existing;
    this._payrollRequestItems$.next(filteredExisting);
    this._payrollRequestItemForCard$.next(filteredExisting);
    this._payrollRequest$.next(this._payrollRequest);
  }

  paginationChangedDeselectItems() {
    let existing = this._payrollRequest.payrollRequestItems;
    let filteredExisting = this._payrollRequestItems$.value;

    existing.forEach(request => {
      if (request.isSelectedOnTableView)
        request.isSelectedOnTableView = false;
    });

    filteredExisting.forEach(request => {
      if (request.isSelectedOnTableView)
        request.isSelectedOnTableView = false;
    });

    this._payrollRequest.payrollRequestItems = existing;
    this._payrollRequestItems$.next(filteredExisting);
    this._payrollRequestItemForCard$.next(filteredExisting);
    this._payrollRequest$.next(this._payrollRequest);
  }

  updateItem(request: IPayrollRequestItem) {
    this._isSaving.next(defaultTrue);
    let isExisting = false;
    let isMerit = false;
    let isOT = false;

    if (request.foreignKeyId != null && request.foreignKeyId != 0)
      isExisting = true;

    isMerit = (!isExisting && request.requestType == 1);
    isOT = (!isExisting && request.requestType == 2);

    request.payoutTo = this.calculateProposedTo(request);

    this.savePayrollRequestItem(request, request.reviewId).subscribe((result) => {
      this._payrollRequest.payrollRequestItems.forEach(x => {
        if (isExisting) {
          if (x.reviewId == request.reviewId && x.foreignKeyId == request.foreignKeyId) {
            x = request;
            return;
          };
        } else if (isMerit) {
          if (x.reviewId == request.reviewId && x.employeeClientRateId == request.employeeClientRateId) {
            x = request;
            return;
          }
        } else if (isOT) {
          if (x.reviewId == request.reviewId && x.clientEarningId == request.clientEarningId) {
            x = request;
            return;
          }
        }
      });
      this._isSaving.next(defaultFalse);
    }, (err) => {
      this.msgSvc.setTemporaryMessage(err.message, MessageTypes.error);
    })
  }

  capturePayrollRequestStatusChanges(requestItems: IPayrollRequestItem[], newStatus: number, slate = null){
    if(!slate) slate = { mia : 0, mid:  0, mip: 0, aea: 0, aed: 0, aep: 0};

    requestItems.forEach(x => {
        switch (x.approvalStatusId) {
          case ApprovalStatus.Approved : (x.requestType == 1 ? slate.mia-- : slate.aea--); break;
          case ApprovalStatus.Rejected : (x.requestType == 1 ? slate.mid-- : slate.aed--); break;
          case ApprovalStatus.Pending : (x.requestType == 1 ? slate.mip-- : slate.aep--);  break;
        }
        switch (newStatus) {
          case ApprovalStatus.Approved : (x.requestType == 1 ? slate.mia++ : slate.aea++); break;
          case ApprovalStatus.Rejected : (x.requestType == 1 ? slate.mid++ : slate.aed++); break;
          case ApprovalStatus.Pending : (x.requestType == 1 ? slate.mip++ : slate.aep++);  break;
        }
    });

    return slate;
  }

  updatePayrollRequestStatusChanges(slate: any){
    // Merit Increase/ Additional Earning changes
    this._payrollRequest.approvedMeritIncreaseCount += slate.mia;
    this._payrollRequest.declinedMeritIncreaseCount += slate.mid;
    this._payrollRequest.pendingMeritIncreaseCount += slate.mip;
    this._payrollRequest.approvedAdditionalEarningCount += slate.aea;
    this._payrollRequest.declinedAdditionalEarningCount += slate.aed;
    this._payrollRequest.pendingAdditionalEarningCount += slate.aep;
  }

  updateItemApprovalStatus(request: IPayrollRequestItem, newStatus: number) {
    this._isSaving.next(defaultTrue);
    let existing = this._payrollRequest.payrollRequestItems;
    let filteredExisting = this._payrollRequestItemForCard$.value;
    let tableExisting = this._payrollRequestItems$.value;
    let isExisting = false;
    let isMerit = false;
    let isOT = false;
    let isEnabledOnProposal = false;
    let itemReview:IReview = null;

    if (newStatus == 1 || newStatus == 2)
      isEnabledOnProposal = true;

    if (request.foreignKeyId != null && request.foreignKeyId != 0)
      isExisting = true;

    let slate = this.capturePayrollRequestStatusChanges([request],newStatus);

    isMerit = (!isExisting && request.requestType == 1);
    isOT = (!isExisting && request.requestType == 2);
    request.approvalStatusId = newStatus;
    request.isEnabledOnProposal = isEnabledOnProposal;

    let reviewIndex = this._payrollRequest.reviews.findIndex(x => { return x.reviewId == request.reviewId });

    if (reviewIndex > -1) {
      itemReview = this._payrollRequest.reviews[reviewIndex];
    }

    this.savePayrollRequestItem(request, request.reviewId).subscribe(result => {

      if (result.payrollRequestItems && result.payrollRequestItems.length) {
        request = result.payrollRequestItems[0]; // should only be one item
        if (request.approvalStatusId == 1 || request.approvalStatusId == 2)
          request.isEnabledOnProposal = true;
      }

      this.updateList(existing, request, isExisting, isMerit, isOT);
      this.updateList(filteredExisting, request, isExisting, isMerit, isOT);
      this.updateList(tableExisting, request, isExisting, isMerit, isOT);

      this.updatePayrollRequestStatusChanges(slate);

      this._payrollRequest.payrollRequestItems = existing;
      this._payrollRequestItems$.next(tableExisting);
      this._payrollRequestItemForCard$.next(filteredExisting);
      this._payrollRequest$.next(this._payrollRequest);
      this._isSaving.next(defaultFalse);
    }, (err) => {
      this.msgSvc.setTemporaryMessage(err.message, MessageTypes.error);
    });

  }

  private updateList(list: IPayrollRequestItem[], item: IPayrollRequestItem, isExisting: boolean, isMerit: boolean, isOT: boolean): void {
    for(var i = 0; i < list.length; i++){
      const x = list[i];
      if (isExisting) {
        if (x.reviewId == item.reviewId && x.foreignKeyId == item.foreignKeyId) {
          list[i] = item;
          break;
        };
      } else if (isMerit) {
        if (x.reviewId == item.reviewId && x.employeeClientRateId == item.employeeClientRateId) {
          list[i] = item;
          break;
        }
      } else if (isOT) {
        if (x.reviewId == item.reviewId && x.clientEarningId == item.clientEarningId) {
          list[i] = item;
          break;
        }
      }
    }
  }
  massUpdateApprovalStatus(newStatus: number) {
    let existing = this._payrollRequest.payrollRequestItems;
    let filteredExisting = this._payrollRequestItems$.value;

    let slate = this.capturePayrollRequestStatusChanges(
      filteredExisting.filter( x => x.isSelectedOnTableView && x.approvalStatusId != newStatus ), newStatus);
    let requestsToUpdate = filteredExisting.filter(x => {
      if (x.isSelectedOnTableView && x.approvalStatusId != newStatus) {
        x.approvalStatusId = newStatus;
        return true;
      } else {
        return false;
      }
    });

    if (requestsToUpdate == null || requestsToUpdate.length == 0)
      return;

    this._isSaving.next(defaultTrue);
    this.massSavePayrollRequestItems(requestsToUpdate).subscribe((result) => {
      if (result.reviews && result.reviews.length) {
        // should only be one review...
        result.reviews.forEach(review => {
          const idx = this._payrollRequest.reviews.findIndex(rev => { return rev.reviewId == review.reviewId });
          if (idx > -1) {
            let itemReview = this._payrollRequest.reviews[idx];
            this._payrollRequest.reviews[idx].proposal = review.proposal;
          } // end of matching review idx found
        }); // end of foreach review

        result.payrollRequestItems.forEach(request => {
          if (request.approvalStatusId == 1 || request.approvalStatusId == 2)
            request.isEnabledOnProposal = true;

          let isMerit = (request.requestType == 1);
          let isOT = (request.requestType == 2);

          if (isMerit) {
            const eIdx = (request.payType == 1)
              ? existing.findIndex(e => e.reviewId == request.reviewId && request.employeeClientRateId && e.employeeClientRateId == request.employeeClientRateId)
              : existing.findIndex(e => e.reviewId == request.reviewId && e.employeeClientRateId == null && request.employeeClientRateId == null);
            const index =(request.payType == 1)
              ? filteredExisting.findIndex(fe => fe.reviewId == request.reviewId && request.employeeClientRateId && fe.employeeClientRateId == request.employeeClientRateId)
              : filteredExisting.findIndex(e => e.reviewId == request.reviewId && e.employeeClientRateId == null && request.employeeClientRateId == null);
            if (index > -1) filteredExisting[index] = request;
            if (eIdx > - 1) existing[eIdx] = request;
          } else if (isOT) {
            const eIdx = existing.findIndex(e => e.reviewId == request.reviewId && request.clientEarningId != null && e.clientEarningId == request.clientEarningId);
            const index = filteredExisting.findIndex(fe => fe.reviewId == request.reviewId && request.clientEarningId != null && fe.clientEarningId == request.clientEarningId);
            if (index > -1) filteredExisting[index] = request;
            if (eIdx > - 1) existing[eIdx] = request;
          }
        }); // end of foreach request

        this.updatePayrollRequestStatusChanges(slate);

        this._payrollRequest.payrollRequestItems = existing;
        this._payrollRequestItems$.next(filteredExisting);
        this._payrollRequestItemForCard$.next(filteredExisting);
        this._payrollRequest$.next(this._payrollRequest);
        this._isSaving.next(defaultFalse);
      } // end of if result.reviews does not equal null
    }, (err) => {
      this.msgSvc.setTemporaryMessage(err.message, MessageTypes.error);
    }); // end of subscribe
  }


  /******************************************************
   * Approves proposal
   * @Disclaimer this is assuming the filtered data
   * is the data that needs to be saved
   *****************************************************/
  approveProposal() {
    let existing = this._payrollRequest.payrollRequestItems;
    let filteredExisting = this._payrollRequestItemForCard$.value;
    let tableExisting = this._payrollRequestItems$.value;

    let slate = this.capturePayrollRequestStatusChanges(
      filteredExisting.filter(item =>
      (item.approvalStatusId != null && item.approvalStatusId != 4 && item.isEnabledOnProposal)), 2);
    slate = this.capturePayrollRequestStatusChanges(
      filteredExisting.filter(item =>
      (item.approvalStatusId != null && item.approvalStatusId != 4 && !item.isEnabledOnProposal)), 3, slate);
      console.log(JSON.stringify(slate));

    let requestsToUpdate = filteredExisting.filter(item => {
      if (item.approvalStatusId != null && item.approvalStatusId != 4) {
        if (item.isEnabledOnProposal) item.approvalStatusId = 2;
        if (!item.isEnabledOnProposal) item.approvalStatusId = 3;
        return true;
      }
      return false;
    });

    this._isSaving.next(defaultTrue);
    this.massSavePayrollRequestItems(requestsToUpdate).subscribe((result) => {
      if (result.reviews && result.reviews.length) {
        // should only be one review...
        result.reviews.forEach(review => {
          const idx = this._payrollRequest.reviews.findIndex(rev => { return rev.reviewId == review.reviewId });
          if (idx > -1) {
            let itemReview = this._payrollRequest.reviews[idx];
            this._payrollRequest.reviews[idx].proposal = review.proposal;
          } // end of matching review idx found
        }); // end of foreach review

        result.payrollRequestItems.forEach(request => {
          if (request.approvalStatusId == 1 || request.approvalStatusId == 2)
            request.isEnabledOnProposal = true;

          const eIdx = existing.findIndex(e => e.foreignKeyId == request.foreignKeyId);
          const index = filteredExisting.findIndex(e => e.foreignKeyId == request.foreignKeyId);
          const tIdx = tableExisting.findIndex(e => e.foreignKeyId == request.foreignKeyId);
          if (index > -1) filteredExisting[index] = request;
          if (eIdx > - 1) existing[eIdx] = request;
          if (tIdx > - 1) tableExisting[tIdx] = request;
        }); // end of foreach request

        this.updatePayrollRequestStatusChanges( slate );

        this._payrollRequest.payrollRequestItems = existing;
        this._payrollRequestItems$.next(tableExisting);
        this._payrollRequestItemForCard$.next(filteredExisting);
        this._payrollRequest$.next(this._payrollRequest);
        this.msgSvc.setTemporarySuccessMessage("Successfully approved the payroll request.");
        this._isSaving.next(defaultFalse);
      } // end of if result.reviews does not equal null
    }, (err) => {
      this.msgSvc.setTemporaryMessage(err.message, MessageTypes.error);
    }); // end of subscribe

  }

  /******************************************************
   * Declines proposal
   * @Disclaimer this is assuming the filtered data
   * is the data that needs to be saved
   *****************************************************/
  declineProposal() {
    let existing = this._payrollRequest.payrollRequestItems;
    let filteredExisting = this._payrollRequestItemForCard$.value;
    let tableExisting = this._payrollRequestItems$.value;

    let slate = this.capturePayrollRequestStatusChanges(
      filteredExisting.filter(item =>
      (item.approvalStatusId != null && item.approvalStatusId != 4 )), 3);

    let requestsToUpdate = filteredExisting.filter(item => {
      if (item.approvalStatusId != null && item.approvalStatusId != 4) {
        item.approvalStatusId = 3;
        return true;
      }
      return false;
    });

    this._isSaving.next(defaultTrue);
    this.massSavePayrollRequestItems(requestsToUpdate).subscribe((result) => {
      if (result.reviews && result.reviews.length) {
        // should only be one review...
        result.reviews.forEach(review => {
          const idx = this._payrollRequest.reviews.findIndex(rev => { return rev.reviewId == review.reviewId });
          if (idx > -1) {
            let itemReview = this._payrollRequest.reviews[idx];
            this._payrollRequest.reviews[idx].proposal = review.proposal;
          } // end of matching review idx found
        }); // end of foreach review

        result.payrollRequestItems.forEach(request => {
          if (request.approvalStatusId == 1 || request.approvalStatusId == 2)
            request.isEnabledOnProposal = true;

          const eIdx = existing.findIndex(e => e.foreignKeyId == request.foreignKeyId);
          const index = filteredExisting.findIndex(e => e.foreignKeyId == request.foreignKeyId);
          const tIdx = tableExisting.findIndex(e => e.foreignKeyId == request.foreignKeyId);
          if (index > -1) filteredExisting[index] = request;
          if (eIdx > - 1) existing[eIdx] = request;
          if (tIdx > - 1) tableExisting[tIdx] = request;

        }); // end of foreach request

        this.updatePayrollRequestStatusChanges(slate);

        this._payrollRequest.payrollRequestItems = existing;
        this._payrollRequestItems$.next(tableExisting);
        this._payrollRequestItemForCard$.next(filteredExisting);
        this._payrollRequest$.next(this._payrollRequest);
        this.msgSvc.setTemporarySuccessMessage("Successfully declined the payroll request.");
        this._isSaving.next(defaultFalse);
      } // end of if result.reviews does not equal null
    }, (err) => {
      this.msgSvc.setTemporaryMessage(err.message, MessageTypes.error);
    }); // end of subscribe

  }


  /********************************************************
   * Edits proposal
   * @param review The active review we will be reopening
   * @disclaimer This is assuming the active review has
   * a proposal.
   *******************************************************/
  editProposal(review: IReview) {
    let existing = this._payrollRequest.payrollRequestItems;
    let filteredExisting = this._payrollRequestItemForCard$.value;
    let tableExisting = this._payrollRequestItems$.value;
    this._isSaving.next(defaultTrue);

    let slate = this.capturePayrollRequestStatusChanges(filteredExisting.filter(x=>x.reviewId == review.reviewId), 1);

    this.reOpenProposal(review.proposal.proposalID, review.clientId).subscribe((result) => {
      if (result.reviews && result.reviews.length) {
        // should only be one review...
        let k = 0;
        for (k = 0; k < result.reviews.length; k++) {
          let review = result.reviews[k];
          const idx = this._payrollRequest.reviews.findIndex(rev => { return rev.reviewId == review.reviewId });
          if (idx > -1) {
            let itemReview = this._payrollRequest.reviews[idx];
            this._payrollRequest.reviews[idx].proposal = review.proposal;
          } // end of matching review idx found
        } // end of for reviews in result

        let j = 0;
        for (j = 0; j < result.payrollRequestItems.length; j++) {
          let request = result.payrollRequestItems[j];
          if (request.approvalStatusId == 1 || request.approvalStatusId == 2)
            request.isEnabledOnProposal = true;

          const eIdx = existing.findIndex(e => e.foreignKeyId == request.foreignKeyId);
          const index = filteredExisting.findIndex(e => e.foreignKeyId == request.foreignKeyId);
          const tIdx = tableExisting.findIndex(e => e.foreignKeyId == request.foreignKeyId);
          if (index > -1) filteredExisting[index] = request;
          if (eIdx > - 1) existing[eIdx] = request;
          if (tIdx > - 1) tableExisting[tIdx] = request;

        } // end of for payroll request items

        this.updatePayrollRequestStatusChanges(slate);

        this._payrollRequest.payrollRequestItems = existing;
        this._payrollRequestItems$.next(tableExisting);
        this._payrollRequestItemForCard$.next(filteredExisting);
        this._payrollRequest$.next(this._payrollRequest);
        this.msgSvc.setTemporarySuccessMessage("Successfully reopened the payroll request.");
        this._isSaving.next(defaultFalse);
      } // end of if result.reviews does not equal null
    }, (err) => {
      this.msgSvc.setTemporaryMessage(err.message, MessageTypes.error);
    });
  }

  calculateProposedTo(request: IPayrollRequestItem) {
    if (request.increaseType == 2) {
      return request.payoutFrom + request.increaseAmount;
    } else if (request.increaseType == 1) {
      if (request.requestType == 1)
        return request.payoutFrom * ((request.increaseAmount/100) + 1);
      if (request.requestType == 2)
        return request.basePay * (request.increaseAmount/100);
    }
  }

  calculateAnnualizedTotalAmount(request: IPayrollRequestItem) {
    return request.payoutTo * request.annualPayPeriodCount;
  }

  proposalHasItemProcessedByPayroll(reviewId: number) {
    return !this._payrollRequest.payrollRequestItems.some(item => {
      return (item.reviewId == reviewId && !item.processedByPayroll);
    });
  }

  proposalIsApprovedOrDeclined(reviewId: number) {
    return this._payrollRequest.reviews.some(review => {
      return (review.reviewId == reviewId
              && review.proposal != null
              && (review.proposal.approvalStatus == 2 || review.proposal.approvalStatus == 3));
    });
  }

  recommendedPercentIncrease(reviewId: number) {
    const fndItem = this._payrollRequest.payrollRequestItems.find(item => {
      return (item.reviewId == reviewId && item.requestType == 1);
    });

    if (fndItem) return fndItem.percent;
    return 0;
  }

  setPaginatedItems(start: number, end: number) {
    let i = 0;
    let m = 0;
    let o = 0;
    let paginatedItems = this._payrollRequestItems$.value.filter(item => {
      if (i >= start && i <= end) {
        i++;
        if (item.requestType == 1) m++;
        if (item.requestType == 2) o++;
        return true;
      } else {
        i++;
        return false;
      }
    });
    this._meritCount.next(m);
    this._otCount.next(o);

    this._paginatedRequests = paginatedItems;
    this.paginationChangedDeselectItems();
  }

}
