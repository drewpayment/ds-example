import { Component, OnInit, ChangeDetectionStrategy, Input, EventEmitter, Output, ViewChild, ElementRef } from '@angular/core';
import { IReview } from '@ds/performance/reviews/shared/review.model';
import * as moment from "moment";
import { DsApiCommonProvider } from '@ajs/core/api/ds-api-common.provider';
import { ReviewsService } from '@ds/performance/reviews/shared/reviews.service';
import { catchError, exhaustMap, concatMap, tap, map } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError, Subscription, fromEvent, of } from 'rxjs';
import { IReviewStatus } from '@ds/performance/performance-manager/shared/review-status.model';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';

/**
 * Uses either the review or reviewStatus for functionality DONT PROVIDE BOTH INPUTS!
 * This is to make sure we don't implement any logic that requires both inputs 
 * when we probably only need to provide one or the other.
 */
@Component({
  selector: 'ds-close-review',
  templateUrl: './close-review.component.html',
  styleUrls: ['./close-review.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CloseReviewComponent implements OnInit {
  private _review: IReview;
  @Input()
  public get review(): IReview {
    return this._review;
  }
  public set review(value: IReview) {
    this._review = value;
    if(this.reviewStatus != null){
      throw new VagueStateError();
    }
  }

  private _reviewStatus?: IReviewStatus;
  @Input()
  public get reviewStatus(): IReviewStatus {
    return this._reviewStatus;
  }
  public set reviewStatus(value: IReviewStatus) {
    this._reviewStatus = value;
    if(this.review != null){
      throw new VagueStateError();
    }
  }

  private _closeReviewBtnSub: Subscription;
  public get closeReviewBtnSub(): Subscription {
    return this._closeReviewBtnSub;
  }
  public set closeReviewBtnSub(value: Subscription) {
    this._closeReviewBtnSub = value;
  }


  private _closeReviewBtn: ElementRef;

  /**  The element that has this name has a mat-menu-item directive.  So to get the actual button we
   * tell Angular that we want the ElementRef and not the directive (our other option was to use a 
   * property of the mat-menu-item directive.  Not sure if that property should be used (the property 
   * has a '_' at the start of it's name)) */
  @ViewChild('closeReviewBtn', { read: ElementRef, static: false }) 
  get closeReviewBtn() {
    return this._closeReviewBtn;
  }
  set closeReviewBtn(val: ElementRef) {
    this._closeReviewBtn = val;

    if (val == null && this.closeReviewBtnSub) {
      this.closeReviewBtnSub.unsubscribe();
    } else if (val && this.closeReviewBtnSub == null) {
      this.closeReviewBtnSub = this.setupBtnHandler(val.nativeElement).subscribe();
    }
  }


@Output() reviewClosed = new EventEmitter<void>();

  constructor(
    private reviewSvc: ReviewsService,
    private msgSvc: DsMsgService
    ) { }

  ngOnInit() {
  }

  get canCloseReview() {
    if(this.reviewStatus != null){
      return !this.reviewStatus.employee.isActive || !this.reviewStatus.review.reviewCompletedDate;
    }
    return !this.review.reviewCompletedDate;
  }

  private readonly setupBtnHandler = (btn: HTMLElement) =>
    fromEvent(btn, 'click').pipe(
      exhaustMap(() => of(this.review || this.reviewStatus.review).pipe(
        tap(() => this.msgSvc.loading(true)),
        concatMap(this.closeReview),
        tap(() => {
          this.msgSvc.setTemporaryMessage("Review updated successfully.");
          this.reviewClosed.next();
        })))
    );

  private readonly closeReview = (review: IReview) =>
    of(review).pipe(
      map(localReview => {
        localReview.reviewCompletedDate = moment().format(DsApiCommonProvider.TimeFormat.DATE_ONLY);
        return localReview;
      }),
      concatMap(localReview => this.reviewSvc
        .saveReview(localReview)
        .pipe(catchError((err: HttpErrorResponse, caught) => {
          this.msgSvc.showWebApiException(err.error);
          return throwError("Error saving review");
        })))
    );

}

class VagueStateError extends Error {
  constructor(){
    super('The close review component is in a vague state.  Please only provide either a ReviewStatus XOR a Review');
  }
}
