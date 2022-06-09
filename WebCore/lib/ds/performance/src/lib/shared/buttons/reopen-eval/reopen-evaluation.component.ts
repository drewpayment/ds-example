import { Component, OnInit, ChangeDetectionStrategy, Input, Output, EventEmitter } from '@angular/core';
import { UserType, UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { IReview } from '@ds/performance/reviews/shared/review.model';
import { IEvaluation } from '@ds/performance/evaluations/shared/evaluation.model';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { from, throwError, Observable } from 'rxjs';
import { concatMap, catchError, tap, withLatestFrom, map } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { IEvaluationSelectedData } from '@ds/performance/reviews/review-list/review-list.component';
import { EvaluationsApiService } from '@ds/performance/evaluations/shared/evaluations-api.service';
import { EvaluationRoleType } from '@ds/performance/evaluations/shared/evaluation-role-type.enum';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';


/**
 * The purpose of this component is to:
 * 1. Show the reopen button when appropriate
 * 2. Handle reopening the evaluation
 * 3. Notify any parent component that it has reopened the evaluation
 *
 * Putting this functionality into a component allows us to easily add
 * the functionality to a page and reduces copying and pasting implementation details
 */
@Component({
  selector: 'ds-reopen-evaluation',
  templateUrl: './reopen-evaluation.component.html',
  styleUrls: ['./reopen-evaluation.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ReopenEvaluationComponent implements OnInit {
  private _review: IReview;
  private _evaluation:IEvaluation;

@Input()
set review(val: IReview){
this._review = val;
}
get review(){
  return this._review;
}

@Input()
set evaluation(val: IEvaluation){
this._evaluation = val;
}
get evaluation(){
  return this._evaluation;
}

  get UserType(){
    return UserType;
  }

  @Output()
  evaluationReopened = new EventEmitter<IEvaluationSelectedData>();

  canReopen$: Observable<UserInfo>;

  constructor(
    private acctSvc: AccountService,
    private msgSvc: DsMsgService,
    private confirmSvc: DsConfirmService,
    private evalSvc: EvaluationsApiService) { }

  ngOnInit() {
    this.canReopen$ = this.acctSvc.getUserInfo();
  }

  reOpenEval(evaluation: IEvaluation, review: IReview) {
    this.confirmSvc.modalOptions.bodyText = "Are you sure you want to reopen this review?  Reopening a review will change its status from \"Submitted\" to an open state.  Users will have to re-submit the review in order to change the status back to \"Submitted.\"";
    this.confirmSvc.modalOptions.closeButtonText = "Cancel";
    this.confirmSvc.modalOptions.actionButtonText = "Reopen";
    this.confirmSvc.modalOptions.swapOkClose = true;
    from(<PromiseLike<any>>this.confirmSvc.show()).pipe(
      withLatestFrom(this.acctSvc.getUserInfo()),
      map(result => ({event: result[0], user: result[1]})),
        concatMap(result =>
        this.evalSvc.reopenEvalution(evaluation.evaluationId)
        .pipe(catchError((err: HttpErrorResponse, caught) => {
            this.msgSvc.showWebApiException(err.error);
            return throwError("Error updating evaluation");
        }), tap(evl => {
            evaluation.isViewableByEmployee = evaluation.role === EvaluationRoleType.Self;
            evaluation.completedDate = null;
            evaluation.signature = null;

            this.msgSvc.setTemporaryMessage("Evaluation re-opened successfully.");

                this.evaluationReopened.emit({
                    evaluation: evaluation,
                    review: review
                });
        }))))

        .subscribe();
}

}
