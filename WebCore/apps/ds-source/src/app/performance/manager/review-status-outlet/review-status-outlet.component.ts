import { Component, OnInit, OnDestroy } from '@angular/core';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { Router } from '@angular/router';
import { Subscription, Observable, of, Subject, merge, ReplaySubject } from 'rxjs';
import { Maybe } from '@ds/core/shared/Maybe';
import { tap, withLatestFrom, concatMap, filter, finalize, map, switchMap } from 'rxjs/operators';
import { ActiveEvaluationService } from '@ds/performance/evaluations/shared/active-evaluation.service';
import { IReviewGroupStatus, IReviewSearchOptions, IReviewStatusSearchOptions } from '@ds/performance/performance-manager';
import { PerformanceManagerService } from '@ds/performance/performance-manager/performance-manager.service';
import { IEvaluation } from '@ds/performance/evaluations';
import { IReview } from '@ds/performance/reviews';



@Component({
    selector: 'ds-review-status-outlet',
    templateUrl: './review-status-outlet.component.html',
    styleUrls: ['./review-status-outlet.component.scss']
})
export class ReviewStatusOutletComponent implements OnInit {

    reviewGroups: IReviewGroupStatus[];
    successMsg: Subject<string> = new ReplaySubject(1);
    loadReviewStatuses$: Observable<any>;
    private readonly updateReviews: Subject<any> = new Subject();
    isLoading = false;
    filtered = false;
    constructor(
        private manager: PerformanceManagerService,
        private msgSvc: DsMsgService,
        private activeEvaluationSvc: ActiveEvaluationService,
        private router: Router) { }

    ngOnInit() {
        this.successMsg.next(null);
            
        this.loadReviewStatuses$ = this.updateView().pipe(
            filter(options => options.reviewTemplateId > 0),
            tap(() => this.isLoading = true),
            switchMap(x => this.getReviewStatuses(x)),
            withLatestFrom(this.successMsg),
            tap(groups => {
                
                this.isLoading = false;
                this.reviewGroups = groups[0];

                if (groups[1]) {
                    this.msgSvc.setTemporarySuccessMessage(groups[1]);
                    this.successMsg.next(null);
                }

                else
                    //this.msgSvc.loading(false);
                    this.isLoading = false;
            }));
    }

    /**
     * Returns an observable that emits when the view should be updated.
     */
    private updateView(): Observable<IReviewSearchOptions> {
    
        return merge(this.manager.activeReviewSearchOptions$,
            this.updateReviews.pipe(
                tap(() => this.isLoading = false),
                tap(() => this.successMsg.next("Review updated successfully.")),
                withLatestFrom(this.manager.activeReviewSearchOptions$),
                map(x => x[1])));
    }

    private readonly getReviewStatuses: (options: IReviewStatusSearchOptions) => Observable<IReviewGroupStatus[]> = (options: IReviewStatusSearchOptions) =>
        of(options).pipe(
            concatMap(options => this.manager.searchReviewStatus(options)));

    reviewChanged() {
        this.updateReviews.next();
    }

    evaluationSelected(event: {evaluation:IEvaluation, review:IReview}) {
        this.activeEvaluationSvc.setActiveEvaluation(event.evaluation, event.review);
        this.activeEvaluationSvc.returnUrl = ["/performance/manage/status"];
        this.router.navigate(['/performance/evaluations']);
    }

}
