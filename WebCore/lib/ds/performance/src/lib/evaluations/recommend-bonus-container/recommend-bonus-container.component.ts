import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { MeritIncreaseService } from '@ds/performance/performance-manager/shared/merit-increase.service';
import { Subject, Observable, combineLatest, throwError, ReplaySubject } from 'rxjs';
import { filter, switchMap, catchError, tap } from 'rxjs/operators';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { RecommendedBonus } from '../shared/recommended-bonus';

@Component({
  selector: 'ds-recommend-bonus-container',
  templateUrl: './recommend-bonus-container.component.html',
  styleUrls: ['./recommend-bonus-container.component.scss']
})
export class RecommendBonusContainerComponent implements OnInit {

private empId = new ReplaySubject<number>(1);
private review = new ReplaySubject<number>(1);
@Output() target: EventEmitter<RecommendedBonus> = new EventEmitter();


recommendedBonus$: Observable<RecommendedBonus>;

  private _reviewId: number;
  @Input()
  public get reviewId(): number {
    return this._reviewId;
  }
  public set reviewId(value: number) {
    this._reviewId = value;
    this.review.next(value);
  }

private _reviewedEmployeeId: number

@Input() 
set reviewedEmployeeId(val: number){
this._reviewedEmployeeId = val;
this.empId.next(val);
}
get(){
  return this._reviewedEmployeeId;
}

  constructor(private meritIncreaseSvc: MeritIncreaseService,
    private msgSvc: DsMsgService) {
    this.recommendedBonus$ = combineLatest(this.empId, this.review).pipe(
      filter(x => x[0] > 0 && x[1] > 0),
      switchMap(x => this.meritIncreaseSvc.CalculateRecommendedBonus(x[1], x[0])),
      tap(x => this.target.emit(x)),
      catchError(x => {
        this.msgSvc.showErrorMsg('Failed to calculate recommended bonus.');
        return throwError(x);
      }));

  }

  ngOnInit() {

  }

}
