import { Component, OnInit, Input, ChangeDetectionStrategy } from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { UserInfo, UserType } from '@ds/core/shared';
import { Observable, of, throwError, Subscription } from 'rxjs';
import { RelativeTimeType } from '@ds/performance/reviews/shared/relative-time-type';
import { EvaluationsApiService } from '@ds/performance/evaluations/shared/evaluations-api.service';
import { tap, concatMap, catchError } from 'rxjs/operators';
import { IRelativeTimeDifference } from '@ds/performance/reviews/shared/relative-time-difference';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';

@Component({
  selector: 'ds-send-complete-eval-reminder',
  templateUrl: './send-complete-eval-reminder.component.html',
  styleUrls: ['./send-complete-eval-reminder.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SendCompleteEvalReminderComponent implements OnInit {

  public userInfo$: Observable<UserInfo>;

  private _lastClickSub: Subscription;

  private _completedDate: Date | string;
  @Input()
  public get completedDate(): Date | string {
    return this._completedDate;
  }
  public set completedDate(value: Date | string) {
    this._completedDate = value;
  }

  private _evaluationId: number;
  @Input()
  public get evaluationId(): number {
    return this._evaluationId;
  }
  public set evaluationId(value: number) {
    this._evaluationId = value;
  }

  private _evalDueDateStatus: IRelativeTimeDifference;
  @Input()
  public get evalDueDateStatus(): IRelativeTimeDifference {
    return this._evalDueDateStatus;
  }
  public set evalDueDateStatus(value: IRelativeTimeDifference) {
    this._evalDueDateStatus = value;
  }

  get UserType(){
    return UserType;
  }

  get RelativeTimeType() {
    return RelativeTimeType;
  }

  constructor(
    private acctSvc: AccountService,
    private evalSvc: EvaluationsApiService,
    private msgSvc: DsMsgService) { }

  ngOnInit() {
    this.userInfo$ = this.acctSvc.getUserInfo();
  }

  SendReminder(evaluationId: number): void {
    if(this._lastClickSub == null){ // make sure we don't send a reminder when we are already trying to send one
      this._lastClickSub = of(evaluationId).pipe(
        tap(() => this.msgSvc.loading(true)),
        concatMap(x => this.evalSvc.sendReminder(x)),
        catchError(e => {
          this.msgSvc.showSingleOpErrorMsg(e)
          return throwError(e);
        })
        )
      .subscribe({
        error: this.UnsetLastClickSub,
        complete: () => {
          this.UnsetLastClickSub();
          this.msgSvc.loading(false);
          this.msgSvc.setTemporarySuccessMessage('Successfully sent reminder.');
        }
      });
    }

  }

  private readonly UnsetLastClickSub = () => this._lastClickSub = null;

}
